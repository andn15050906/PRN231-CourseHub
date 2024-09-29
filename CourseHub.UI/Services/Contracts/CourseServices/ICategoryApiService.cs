using CourseHub.Core.Entities.CourseDomain;

namespace CourseHub.UI.Services.Contracts.CourseServices;

public interface ICategoryApiService
{
	Task<List<Category>> GetAsync();
	Task ForgeGet(List<Category> result);
}
