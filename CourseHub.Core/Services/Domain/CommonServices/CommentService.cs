using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.CommentModels;
using CourseHub.Core.RequestDtos.Common.CommentDtos;
using CourseHub.Core.RequestDtos.Course.CourseReviewDtos;
using CourseHub.Core.Services.Domain.CommonServices.Contracts;
using CourseHub.Core.Services.Storage.Utils;
using CourseHub.Core.Services.Storage;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace CourseHub.Core.Services.Domain.CommonServices;

public class CommentService : DomainService, ICommentService
{
    public CommentService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }

    public async Task<ServiceResult<PagedResult<CommentModel>>> GetPagedAsync(QueryCommentDto dto)
    {
        var query = _uow.CommentRepo.GetPagingQuery(GetPredicate(dto), dto.PageIndex, dto.PageSize);
        var result = await query.ExecuteWithOrderBy(_ => _.CreationTime, ascending: false, asNoTracking: true);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<Guid>> CreateAsync(CreateCommentDto dto, Guid? client)
    {
        if (client is null)
            return Unauthorized<Guid>();
        var author = await _uow.UserRepo.Find(client);
        if (author is null)
            return Unauthorized<Guid>();

        var entity = await Adapt(dto, author);
        try
        {
            await _uow.CommentRepo.Insert(entity);
            await _uow.CommitAsync();
            return Created(entity.Id);
        }
        catch
        {
            return ServerError<Guid>();
        }
    }

    public async Task<ServiceResult> UpdateAsync(UpdateCommentDto dto, Guid? client)
    {
        if (client is null)
            return Unauthorized();
        var author = await _uow.UserRepo.Find(client);
        if (author is null)
            return Unauthorized();

        var entity = await _uow.CommentRepo.Find(dto.Id);
        await ApplyChanges(dto, entity);
        try
        {
            await _uow.CommitAsync();
            return Ok();
        }
        catch
        {
            return ServerError();
        }
    }

    public async Task<ServiceResult> DeleteAsync(Guid id, Guid? client)
    {
        if (client is null || client == default)
            return Unauthorized();

        var entity = await _uow.CommentRepo.Find(id);
        if (entity is null)
            return NotFound();
        if (entity.CreatorId != client)
            return Unauthorized();

        _uow.CommentRepo.Delete(entity);
        await _uow.CommitAsync();
        return Ok();
    }







    private Expression<Func<Comment, bool>>? GetPredicate(QueryCommentDto dto)
    {
        if (dto.LectureId is not null)
            return _ => _.LectureId == dto.LectureId;
        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId;
        return null;
    }

    private async Task<Comment> Adapt(CreateCommentDto dto, User author)
    {
        Guid id = Guid.NewGuid();

        List<CommentMedia>? medias = null;

        if (dto.Medias is not null)
        {
            medias = new();
            foreach (var media in dto.Medias)
            {
                string url = string.Empty;

                if (media.Dto.File is not null)
                    url = await SaveMedia(media.Dto.File, id, Guid.NewGuid());
                else if (media.Dto.Url is not null)
                    url = media.Dto.Url;

                medias.Add(new CommentMedia { Url = url, Type = media.Type });
            }
        }

        return new Comment(id, author, dto.Content, dto.SourceType, dto.SourceId, medias);
    }

    private async Task ApplyChanges(UpdateCommentDto dto, Comment entity)
    {
        entity.Content = dto.Content;
    }

    private async Task<string> SaveMedia(IFormFile file, Guid commentId, Guid fileName)
    {
        Stream stream = await new FileConverter().ToJpg(file);
        string path = CommentStorage.GetMediaPath(commentId, fileName, FileConverter.EXTENSION_JPG);
        await ServerStorage.SaveFile(stream, path, _logger);
        return path;
    }
}
