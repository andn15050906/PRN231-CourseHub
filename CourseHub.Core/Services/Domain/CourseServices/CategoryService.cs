using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.RequestDtos.Course.CategoryDtos;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;

namespace CourseHub.Core.Services.Domain.CourseServices;

public class CategoryService : DomainService, ICategoryService
{
    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }



    public async Task<ServiceResult<List<Category>>> GetAllAsync()
    {
        var result = await _uow.CategoryRepo.GetAllAsync();
        return ToQueryResult(result);
    }



    public async Task<ServiceResult<Guid>> CreateAsync(CreateCategoryDto dto)
    {
        Category? parent = null;
        if (dto.ParentId is not null)
        {
            parent = await _uow.CategoryRepo.Find(dto.ParentId);
            if (parent is null)
                return BadRequest<Guid>(CourseDomainMessages.INVALID_PARENT);
        }

        Category entity = Adapt(dto, parent);
        await _uow.CategoryRepo.Insert(entity);
        await _uow.CommitAsync();
        return Created(entity.Id);
    }

    public async Task<ServiceResult> UpdateAsync(UpdateCategoryDto dto)
    {
        Category? parent = null;
        if (dto.ParentId is not null)
        {
            parent = await _uow.CategoryRepo.Find(dto.ParentId);
            if (parent is null)
                return BadRequest<Guid>(CourseDomainMessages.INVALID_PARENT);
        }

        var entity = await _uow.CategoryRepo.Find(dto.Id);
        if (entity is null)
            return BadRequest();

        ApplyChanges(dto, entity, parent);
        await _uow.CommitAsync();
        return Ok();
    }

    public async Task<ServiceResult> DeleteAsync(Guid id)
    {
        try
        {
            await _uow.CategoryRepo.ExecuteDeleteAsync(id);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }






    private Category Adapt(CreateCategoryDto dto, Category? parent)
    {
        return new Category(dto.Title, dto.Description, dto.IsLeaf, parent);
    }

    private void ApplyChanges(UpdateCategoryDto dto, Category entity, Category? parent)
    {
        if (dto.Title is not null)
            entity.Title = dto.Title;
        if (dto.Description is not null)
            entity.Description = dto.Description;

        if (parent is not null)
            entity.SetPath(parent);
    }
}
