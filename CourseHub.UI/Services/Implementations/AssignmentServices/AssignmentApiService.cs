using CourseHub.Core.Models.Assignment.AssignmentModels;
using CourseHub.Core.RequestDtos.Assignment.AssignmentDtos;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.AssignmentServices;

namespace CourseHub.UI.Services.Implementations.AssignmentServices;

public class AssignmentApiService : IAssignmentApiService
{
    private readonly HttpClient _client;

    public AssignmentApiService(HttpClient client)
    {
        _client = client;
    }



    public async Task<AssignmentModel?> GetAsync(Guid id, HttpContext context)
    {
        try
        {
            _client.AddBearerHeader(context);
            var result = await _client.GetFromJsonAsync<AssignmentModel>(
                $"api/assignments/{id}", SerializeOptions.JsonOptions);
            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<AssignmentMinModel?> GetMinAsync(Guid id)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<AssignmentMinModel>(
                $"api/assignments/{id}/min", SerializeOptions.JsonOptions);
            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<AssignmentMinModel>> GetBySectionsAsync(IEnumerable<Guid> sections)
    {
        try
        {
            string url = QueryBuilder.BuildWithArray("api/assignments/BySections?", "sections={0}&", sections.Select(_ => _.ToString()));

            var result = await _client.GetFromJsonAsync<List<AssignmentMinModel>>(url, SerializeOptions.JsonOptions);
            return result;
        }
        catch
        {
            return new List<AssignmentMinModel>();
        }
    }

    public async Task<List<AssignmentMinModel>?> GetByCourseAsync(Guid courseId)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<List<AssignmentMinModel>>(
                $"api/assignments/ByCourse?courseId={courseId}");
            return result;
        }
        catch
        {
            return null;
        }
    }



    public async Task<HttpResponseMessage> CreateAsync(CreateAssignmentDto dto, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.PostAsJsonAsync("/api/assignments", dto);
        return result;
    }

    public async Task<HttpResponseMessage> UpdateAsync(UpdateAssignmentDto dto, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.PatchAsync("/api/assignments", JsonContent.Create(dto));
        return result;
    }

    public async Task<HttpResponseMessage> DeleteAsync(Guid id, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.DeleteAsync($"/api/assignments/{id}");
        return result;
    }
}
