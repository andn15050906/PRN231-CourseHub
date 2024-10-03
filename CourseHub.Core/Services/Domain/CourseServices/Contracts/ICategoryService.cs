using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.RequestDtos.Course.CategoryDtos;

namespace CourseHub.Core.Services.Domain.CourseServices.Contracts;

public interface ICategoryService
{
    Task<ServiceResult<List<Category>>> GetAllAsync();

    Task<ServiceResult<Guid>> CreateAsync(CreateCategoryDto dto);
    Task<ServiceResult> UpdateAsync(UpdateCategoryDto dto);
    Task<ServiceResult> DeleteAsync(Guid id);
}
