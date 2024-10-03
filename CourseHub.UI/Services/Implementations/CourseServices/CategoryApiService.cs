using CourseHub.Core.Entities.CourseDomain;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Cache;
using CourseHub.UI.Services.Contracts.CourseServices;

namespace CourseHub.UI.Services.Implementations.CourseServices;

public class CategoryApiService : ICategoryApiService
{
    private readonly HttpClient _client;
    //private readonly CacheService _cache;

    public CategoryApiService(HttpClient client/*, CacheService cache*/)
    {
        _client = client;
        //_cache = cache;
    }



    public async Task ForgeGet(List<Category> result)
    {
        try
        {
            result.Clear();
            result = await GetAsync();
        }
        catch { }
    }

    public async Task<List<Category>> GetAsync()
	{
        //if (_cache.TryGetCategories(out var result))
		//	return result!;

		try
        {
            var result = await _client.GetFromJsonAsync<List<Category>>(
                $"api/categories", SerializeOptions.JsonOptions);

            //_cache.Set(result!);

            return result!;
        }
        catch
        {
            return new List<Category>();
        }
    }
}
