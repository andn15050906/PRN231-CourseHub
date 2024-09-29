using AutoMapper;
using CourseHub.Core.Entities.CourseDomain.Enums;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Models.Assignment.SubmissionModels;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.RequestDtos.Assignment.SubmissionDtos;
using CourseHub.Core.Services.Domain.AssignmentServices.Contracts;
using System.Text.Json;

namespace CourseHub.Core.Services.Domain.AssignmentServices;

public class SubmissionService : DomainService, ISubmissionService
{
    public SubmissionService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }



    public async Task<ServiceResult<SubmissionModel>> GetAsync(Guid id)
    {
        var result = await _uow.SubmissionRepo.Get(id);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<List<SubmissionMinModel>>> GetByAssignmentId(Guid assignmentId, Guid clientId)
    {
        var result = await _uow.SubmissionRepo.GetByAssignmentId(assignmentId, clientId);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<Guid>> CreateAsync(CreateSubmissionDto dto, Guid client)
    {
        try
        {
            // A lot of queries
            //...
            var assignment = await _uow.AssignmentRepo.Find(dto.AssignmentId);
            if (assignment is null)
                return NotFound<Guid>();
            var choices = await _uow.McqChoiceRepo.GetMultiple(dto.Answers.Select(_ => _.MCQChoiceId));

            var entity = Adapt(dto, client, choices, assignment);
            await _uow.SubmissionRepo.Insert(entity);

            // Trigger
            if (entity.Mark > assignment.GradeToPass)
            {
                var courseSections = await _uow.CourseRepo.GetCourseSections(assignment.SectionId);
                /*var section = await _uow.SectionRepo.GetWithCourse(assignment.SectionId);
                if (section is null)
                    return ServerError<Guid>(AssignmentDomainMessages.INVALID_SECTION);
                var course = await _uow.CourseRepo.GetMinAsync(section.CourseId);
                if (course is null)
                    return ServerError<Guid>(AssignmentDomainMessages.INVALID_COURSE);*/

                if (courseSections is null)
                    return ServerError<Guid>(AssignmentDomainMessages.INVALID_SECTION);

                var enrollment = await _uow.EnrollmentRepo.Find(client, courseSections.Id);
                if (enrollment is null)
                    return ServerError<Guid>(AssignmentDomainMessages.INVALID_ENROLLMENT);
                await AddAssignmentMilestone(enrollment, assignment.Id, courseSections);
            }

            await _uow.CommitAsync();

            return Created(entity.Id);
        }
        catch
        {
            return ServerError<Guid>();
        }
    }






    private Submission Adapt(CreateSubmissionDto _, Guid client, List<McqChoice> choices, Assignment assignment)
    {
        Guid id = Guid.NewGuid();

        List<McqUserAnswer> answers = choices.Select(_ => new McqUserAnswer
        {
            SubmissionId = id,
            MCQChoiceId = _.Id
        }).ToList();

        int correctChoices = 0;
        foreach (var choice in choices)
            if (choice.IsCorrect)
                correctChoices++;

        return new Submission
        {
            Id = id,
            CreatorId = client,
            AssignmentId = _.AssignmentId,
            TimeSpentInSec = _.TimeSpentInSec,
            Answers = answers,
            Mark = GetMarkByChoices(correctChoices, assignment.QuestionCount)
        };
    }

    private double GetMarkByChoices(int correctChoices, int questionCount)
    {
        return (correctChoices / (double)questionCount) * 10;
    }

    private async Task AddAssignmentMilestone(Enrollment enrollment, Guid assignmentId, CourseSectionsModel courseSections)
    {
        List<Guid> currentMilestones;
        if (string.IsNullOrEmpty(enrollment.AssignmentMilestones))
        {
            currentMilestones = new() { assignmentId };
            enrollment.AssignmentMilestones = JsonSerializer.Serialize(currentMilestones);
        }
        else
        {
            currentMilestones = JsonSerializer.Deserialize<List<Guid>>(enrollment.AssignmentMilestones);
            if (currentMilestones is null)
                throw new Exception(CourseDomainMessages.INTERNAL_BAD_MILESTONES);
            if (!currentMilestones.Contains(assignmentId))
            {
                currentMilestones.Add(assignmentId);
                enrollment.AssignmentMilestones = JsonSerializer.Serialize(currentMilestones);
            }
        }

        //... delete case
        var allAssignments = await _uow.AssignmentRepo.GetIdsBySectionsAsync(courseSections.Sections);
        var allAssignmentsIsCompleted = !allAssignments.Except(currentMilestones).Any();
        if (allAssignmentsIsCompleted)
            enrollment.Status = CourseStatus.Completed;
    }
}
