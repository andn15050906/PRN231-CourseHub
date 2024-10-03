using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.RequestDtos.Course.LectureDtos;
using CourseHub.Core.Services.Storage;
using Microsoft.AspNetCore.Http;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;
using CourseHub.Core.Models.Course.LectureModels;

namespace CourseHub.Core.Services.Domain.CourseServices;

public class LectureService : DomainService, ILectureService
{
    public LectureService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }

    public async Task<ServiceResult<Lecture>> GetAsync(Guid id)
    {
        var result = await _uow.LectureRepo.Find(id);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<Guid>> CreateAsync(CreateLectureDto dto, Guid client)
    {
        try
        {
            var instructorId = await _uow.InstructorRepo.GetIdByUserId(client);
            if (instructorId == default)
                return ServerError<Guid>();

            var section = await _uow.SectionRepo.Find(dto.SectionId);
            if (section is null)
                return BadRequest<Guid>(CourseDomainMessages.INVALID_SECTION);

            var entity = await Adapt(dto, section.CourseId);
            await _uow.LectureRepo.Insert(entity);
            await _uow.CommitAsync();
            return Created(entity.Id);
        }
        catch (Exception ex)
        {
            return ServerError<Guid>(/*ex.InnerException is null ? ex.Message : ex.InnerException.Message*/);
        }
    }

    public async Task<ServiceResult> UpdateAsync(UpdateLectureDto dto, Guid client)
    {
        try
        {
            var instructorId = await _uow.InstructorRepo.GetIdByUserId(client);
            if (instructorId == default)
                return ServerError();

            var lecture = await _uow.LectureRepo.Find(dto.Id);
            if (lecture is null)
                return BadRequest<Guid>(CourseDomainMessages.INVALID_LECTURE);

            await ApplyChanges(dto, lecture);
            await _uow.CommitAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError();
        }
    }

    public Task<ServiceResult> DeleteAsync(Guid id, Guid client)
    {
        throw new NotImplementedException();
    }






    private async Task<Lecture> Adapt(CreateLectureDto dto, Guid courseId)
    {
        List<LectureMaterial> materials = new();

        List<Task<string>> tasks = new();
        foreach (var material in dto.Materials)
        {
            Task<string> url = material.File is not null
                ? SaveMaterial(material.File, courseId)
                : Task.FromResult(material.Url!);
            tasks.Add(url);
        }

        await Task.WhenAll(tasks);

        for (int i = 0; i < dto.Materials.Count; i++)
        {
            materials.Add(new LectureMaterial
            {
                Type = dto.Materials[i].Type,
                Url = tasks[i].Result
            });
        }

        return new Lecture(dto.SectionId, dto.Title, dto.Content, materials);
    }

    private async Task ApplyChanges(UpdateLectureDto dto, Lecture entity)
    {
        if (dto.Title is not null)
            entity.Title = dto.Title;
        if (dto.Content is not null)
            entity.Content = dto.Content;
        if (dto.IsPreviewable is not null)
            entity.IsPreviewable = (bool)dto.IsPreviewable;

        // Materials
        if (dto.RemovedMaterials is not null)
        {
            for (int i = 0; i < entity.Materials.Count; i++)
                entity.Materials.RemoveAll(_ => dto.RemovedMaterials.Any(r => r == _.Url));
        }
        if (dto.AddedMaterials is not null)
        {
            List<LectureMaterial> materials = new();
            List<Task<string>> tasks = new();
            foreach (var material in dto.AddedMaterials)
            {
                Task<string> url = material.File is not null
                    ? SaveMaterial(material.File, dto.CourseId)
                    : Task.FromResult(material.Url!);
                tasks.Add(url);
            }
            await Task.WhenAll(tasks);
            for (int i = 0; i < dto.AddedMaterials.Count; i++)
            {
                materials.Add(new LectureMaterial
                {
                    Type = dto.AddedMaterials[i].Type,
                    Url = tasks[i].Result
                });
            }



            entity.Materials.AddRange(
                dto.AddedMaterials.Select(_ => new LectureMaterial { Type = _.Type, Url = _.Url })
            );
        }
    }

    private async Task<string> SaveMaterial(IFormFile file, Guid courseId)
    {
        Guid fileName = Guid.NewGuid();
        string path = CourseStorage.GetCourseMediaPath(courseId, fileName, Path.GetExtension(file.FileName));
        await ServerStorage.SaveFile(file.OpenReadStream(), path, _logger);
        return path;
    }
}
