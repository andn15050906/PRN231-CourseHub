using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Helpers.Http;
using CourseHub.Core.RequestDtos.Course.LectureDtos;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CourseServices;

namespace CourseHub.UI.Services.Implementations.CourseServices;

public class LectureApiService : ILectureApiService
{
    private readonly HttpClient _client;

    public LectureApiService(HttpClient client)
    {
        _client = client;
    }






    public async Task<Lecture?> GetAsync(Guid id, HttpContext context)
    {
        try
        {
            _client.AddBearerHeader(context);
            var result = await _client.GetFromJsonAsync<Lecture>($"api/lectures/{id}");

            if (result == null)
                return null;
            foreach (var item in result.Materials)
                item.Url = Configurer.GetApiClientOptions().ApiServerPath + $"/api/courses/Resource/Media/{item.Url}";

            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<HttpResponseMessage> Create(CreateLectureDto dto, HttpContext context)
    {
        FormDataHelper helper = new()
        {
            KeyValuePairs = new()
            {
                { nameof(dto.SectionId), dto.SectionId.ToString() },
                { nameof(dto.Title), dto.Title },
                { nameof(dto.Content), dto.Content }
            },
            Files = new()
        };
        for (int i = 0; i < dto.Materials.Count; i++)
        {
            helper.KeyValuePairs.Add($"Materials[{i}].Type", ((int)dto.Materials[i].Type).ToString());
            var file = dto.Materials[i].File;
            helper.Files.Add((file!.OpenReadStream(), $"Materials[{i}].File", file.FileName));
        }

        var formData = helper.ToFormData();
        _client.AddBearerHeader(context);
        return await _client.PostAsync("api/lectures", formData);
    }

    public Task<HttpResponseMessage> Update(UpdateLectureDto dto, HttpContext context)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> Delete(Guid id, HttpContext context)
    {
        throw new NotImplementedException();
    }
}
