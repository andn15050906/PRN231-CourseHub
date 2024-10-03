using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.CommentModels;
using CourseHub.Core.Models.Course.CourseReviewModels;
using CourseHub.Core.RequestDtos.Course.CourseReviewDtos;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CourseServices;

namespace CourseHub.UI.Services.Implementations.CourseServices;

public class CourseReviewApiService : ICourseReviewApiService
{
    private readonly HttpClient _client;

    public CourseReviewApiService(HttpClient client)
    {
        _client = client;
    }



    public async Task<PagedResult<CourseReviewModel>> GetAsync(QueryCourseReviewDto dto)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<PagedResult<CourseReviewModel>>(
                $"api/CourseReviews?{QueryBuilder.Build(dto)}", SerializeOptions.JsonOptions);
            return result;
        }
        catch
        {
            return PagedResult<CourseReviewModel>.GetEmpty();
        }
    }

    public async Task<HttpResponseMessage> CreateAsync(CreateCourseReviewDto dto, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.PostAsJsonAsync("/api/CourseReviews", dto);
        return result;
    }

    public Task<HttpResponseMessage> UpdateAsync(UpdateCourseReviewDto dto, HttpContext context)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> DeleteAsync(Guid id, HttpContext context)
    {
        throw new NotImplementedException();
    }
}
