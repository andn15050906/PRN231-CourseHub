using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.NotificationModels;
using CourseHub.Core.RequestDtos.Common.NotificationDtos;
using System.Linq.Expressions;
using CourseHub.Core.Entities.CommonDomain.Enums;
using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.Entities.UserDomain;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Services.Domain.CommonServices.Contracts;
using CourseHub.Core.RequestDtos.Course.InstructorDtos;
using System.Text.Json;
using CourseHub.Core.RequestDtos.Social.ConversationDtos;
using CourseHub.Core.RequestDtos.Payment;
using CourseHub.Core.Services.Domain.CommonServices.TempModels;

namespace CourseHub.Core.Services.Domain.CommonServices;

public class NotificationService : DomainService, INotificationService
{
    public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }

    public async Task<ServiceResult<PagedResult<NotificationModel>>> GetPagedAsync(QueryNotificationDto dto)
    {
        var query = _uow.NotificationRepo.GetPagingQuery(GetPredicate(dto), dto.PageIndex, dto.PageSize);
        var result = await query.ExecuteWithOrderBy(_ => _.CreationTime, ascending: false);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<Guid>> CreateAsync(CreateNotificationDto dto, Guid? client)
    {
        if (client is null)
            return Unauthorized<Guid>();

        var entity = Adapt(dto, (Guid)client);
        await _uow.NotificationRepo.Insert(entity);
        await _uow.CommitAsync();
        return Created(entity.Id);
    }

    public async Task<ServiceResult> CreateAsync(CreateMultipleNotificationDto dto, Guid? client)
    {
        if (client is null)
            return Unauthorized();

        var entity = Adapt(dto, (Guid)client);
        await _uow.NotificationRepo.Insert(entity);
        await _uow.CommitAsync();
        return Created();
    }

    public async Task<ServiceResult> UpdateAsync(UpdateNotificationDto dto, Guid? client)
    {
        if (client is null)
            return Unauthorized();

        var entity = await _uow.NotificationRepo.Find(dto.Id);
        if (entity is null)
            return BadRequest();

        //... outer
        var user = await _uow.UserRepo.Find(client);
        if (user is null)
            return Unauthorized();
        if (entity.Type == NotificationType.RequestToBecomeInstructor && user.Role < Role.Admin)
            return Unauthorized();
        try
        {
            await ApplyChanges(dto, entity, (Guid)client);
            await _uow.CommitAsync();
        }
        catch (Exception ex)
        {
            return ServerError();
        }
        return Ok();
    }






    private Notification Adapt(CreateNotificationDto dto, Guid client)
    {
        return new Notification(client, dto.Message, dto.Type, dto.ReceiverId);
    }

    private List<Notification> Adapt(CreateMultipleNotificationDto dto, Guid client)
    {
        List<Notification> result = new();
        foreach (var receiver in dto.ReceiverIds)
            result.Add(new Notification(client, dto.Message, dto.Type, receiver));
        return result;
    }

    private async Task ApplyChanges(UpdateNotificationDto dto, Notification entity, Guid client)
    {
        switch (entity.Type)
        {
            case NotificationType.RequestToBecomeInstructor:
				var user = await _uow.UserRepo.Find(entity.CreatorId);
				if (user is null)
					throw new Exception(UserDomainMessages.NOT_FOUND);

				if (dto.Status == NotificationStatus.Approved)
                {
					var instructorDto = JsonSerializer.Deserialize<CreateInstructorDto>(entity.Message);
					if (instructorDto is null)
						throw new Exception(CourseDomainMessages.INVALID_INSTRUCTOR);

					Guid instructorId = Guid.NewGuid();
					Instructor instructorEntity = new(instructorId, user.Id, instructorDto.Intro, instructorDto.Experience);
					await _uow.InstructorRepo.Insert(instructorEntity);
					user.SetInstructor(instructorId);

                    CreateNotificationDto approvedNotificationDto = new()
                    {
                        Message = JsonSerializer.Serialize(new InstructorResponseModel()
                        {
                            IsApproved = true,
                            Message = string.Empty
                        }),
                        Type = NotificationType.InstructorResponse,
                        ReceiverId = user.Id
                    };
                    var approvedNotification = Adapt(approvedNotificationDto, client);
                    await _uow.NotificationRepo.Insert(approvedNotification);
				}
                else
				{
					CreateNotificationDto dismissNotificationDto = new()
					{
						Message = JsonSerializer.Serialize(new InstructorResponseModel()
                        {
                            IsApproved = false,
                            Message = string.Empty
                        }),
						Type = NotificationType.InstructorResponse,
						ReceiverId = user.Id
					};
                    var dismissNotification = Adapt(dismissNotificationDto, client);
                    await _uow.NotificationRepo.Insert(dismissNotification);
				}
                break;



            case NotificationType.InviteMember:
                if (dto.Status == NotificationStatus.Approved)
                {
                    var invitationDto = JsonSerializer.Deserialize<CreateInvitationDto>(entity.Message);
                    if (invitationDto is null)
                        throw new Exception(NotificationDomainMessages.INTERNAL_BAD_MESSAGE);

                    var conversation = await _uow.ConversationRepo.FindWithMembers(invitationDto.Conversation);
                    if (conversation is null)
                        throw new Exception(NotificationDomainMessages.NOTFOUND_NOTIFICATION);
                    conversation.Members.Add(new ConversationMember(conversation.Id, client, false));
                }
                break;



            case NotificationType.RequestWithdrawal:
                if (dto.Status == NotificationStatus.Approved)
                {
                    var withdrawalDto = JsonSerializer.Deserialize<CreateWithdrawalDto>(entity.Message);
                    if (withdrawalDto is null)
                        throw new Exception(NotificationDomainMessages.INTERNAL_BAD_MESSAGE);

                    var instructor = await _uow.InstructorRepo.FindEntityByUserIdAsync(entity.CreatorId);
                    if (instructor is null)
                        throw new Exception(NotificationDomainMessages.NOTFOUND_NOTIFICATION);
                    instructor.Withdraw(withdrawalDto.Amount);
                }
                break;



            case NotificationType.ReportCourse:
                if (dto.Status == NotificationStatus.Approved)
                {
                    var reportDto = JsonSerializer.Deserialize<CreateCourseReportDto>(entity.Message);
                    if (reportDto is null)
                        throw new Exception(NotificationDomainMessages.INTERNAL_BAD_MESSAGE);

                    var course = await _uow.CourseRepo.Find(reportDto.Course);
                    if (course is null)
                        throw new Exception(NotificationDomainMessages.NOTFOUND_NOTIFICATION);

                    CreateNotificationDto newNotificationDto = new()
                    {
                        Message = entity.Message,
                        Type = NotificationType.InstructorReportedCourse,
                        ReceiverId = course.CreatorId
                    };
                    var instructorNotification = Adapt(newNotificationDto, client);
                    await _uow.NotificationRepo.Insert(instructorNotification);
                }
                break;



            case NotificationType.ReportGroup:
                if (dto.Status == NotificationStatus.Approved)
                {
                    var reportDto = JsonSerializer.Deserialize<CreateConversationReportDto>(entity.Message);
                    if (reportDto is null)
                        throw new Exception(NotificationDomainMessages.INTERNAL_BAD_MESSAGE);

                    var conversation = await _uow.ConversationRepo.FindWithMembers(reportDto.Conversation);
                    if (conversation is null)
                        throw new Exception(NotificationDomainMessages.NOTFOUND_NOTIFICATION);

                    //...
                    CreateMultipleNotificationDto newNotificationDto = new()
                    {
                        Message = entity.Message,
                        Type = NotificationType.GroupAdminReportedGroup,
                        ReceiverIds = conversation.Members.Where(_ => _.IsAdmin).Select(_ => _.CreatorId).ToList()
                    };
                    var groupAdminNotifications = Adapt(newNotificationDto, client);
                    await _uow.NotificationRepo.Insert(groupAdminNotifications);
                }
                break;
        }
        entity.Status = dto.Status;
    }

    private Expression<Func<Notification, bool>>? GetPredicate(QueryNotificationDto dto)
    {
        if (dto.CreatorId is not null && dto.Type is not null)
            return _ => _.CreatorId == dto.CreatorId && _.Type == dto.Type;

        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId;
        if (dto.ReceiverId is not null)
            return _ => _.ReceiverId == dto.ReceiverId;
        if (dto.Type is not null)
            return _ => _.Type == dto.Type;
        if (dto.Status is not null)
            return _ => _.Status == dto.Status;
        return null;
    }
}
