using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Helpers.Http;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.Models.Course.EnrollmentModels;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Helpers.Utils;
using CourseHub.UI.Services.Contracts.CourseServices;

namespace CourseHub.UI.Services.Implementations.CourseServices;

public class CourseApiService : ICourseApiService
{
    private readonly HttpClient _client;

    public CourseApiService(HttpClient client)
    {
        _client = client;
    }






    public async Task<CourseModel?> GetAsync(Guid id)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<CourseModel>(
                $"api/courses/{id}", SerializeOptions.JsonOptions);

            if (result is null)
                return null;

            if (!ResourceHelper.IsRemote(result.ThumbUrl))
                result.ThumbUrl = Configurer.GetApiClientOptions().ApiServerPath + $"/api/courses/Resource/{result.Id}/local-thumb";
            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<CourseOverviewModel?> GetBySectionIdAsync(Guid sectionId)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<CourseOverviewModel>(
                $"api/courses/BySection?sectionId={sectionId}", SerializeOptions.JsonOptions);

            if (result is null)
                return null;

            if (!ResourceHelper.IsRemote(result.ThumbUrl))
                result.ThumbUrl = Configurer.GetApiClientOptions().ApiServerPath + $"/api/courses/Resource/{result.Id}/local-thumb";
            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<PagedResult<CourseOverviewModel>> GetPagedAsync(QueryCourseDto dto)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<PagedResult<CourseOverviewModel>>(
                $"api/courses?{QueryBuilder.Build(dto)}", SerializeOptions.JsonOptions);

            foreach (var item in result!.Items)
            {
                if (!ResourceHelper.IsRemote(item.ThumbUrl))
                    item.ThumbUrl = Configurer.GetApiClientOptions().ApiServerPath + $"/api/courses/Resource/{item.Id}/local-thumb";
            }

            return result;
        }
        catch
        {
            return PagedResult<CourseOverviewModel>.GetEmpty();
        }
    }

    public async Task<List<CourseOverviewModel>?> GetMultipleAsync(IEnumerable<Guid> ids)
    {
        try
        {
            string url = QueryBuilder.BuildWithArray("api/courses/multiple?", "ids={0}&", ids.Select(_ => _.ToString()));

            var result = await _client.GetFromJsonAsync<List<CourseOverviewModel>>(url, SerializeOptions.JsonOptions);

            foreach (var item in result!)
            {
                if (!ResourceHelper.IsRemote(item.ThumbUrl))
                    item.ThumbUrl = Configurer.GetApiClientOptions().ApiServerPath + $"/api/courses/Resource/{item.Id}/local-thumb";
            }

            return result;
        }
        catch
        {
            return new();
        }
    }

    public async Task<List<CourseOverviewModel>?> GetSimilarAsync(Guid id)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<List<CourseOverviewModel>>($"api/{id}/similar", SerializeOptions.JsonOptions);

            foreach (var item in result!)
            {
                if (!ResourceHelper.IsRemote(item.ThumbUrl))
                    item.ThumbUrl = Configurer.GetApiClientOptions().ApiServerPath + $"/api/courses/Resource/{item.Id}/local-thumb";
            }

            return result;
        }
        catch
        {
            return new();
        }
    }

    public async Task<PagedResult<CourseMinModel>?> GetMinAsync(QueryCourseDto dto)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<PagedResult<CourseMinModel>>(
                $"api/courses?{QueryBuilder.Build(dto)}", SerializeOptions.JsonOptions);

            foreach (var item in result!.Items)
            {
                if (!ResourceHelper.IsRemote(item.ThumbUrl))
                    item.ThumbUrl = Configurer.GetApiClientOptions().ApiServerPath + $"/api/courses/Resource/{item.Id}/local-thumb";
            }

            return result;
        }
        catch
        {
            return PagedResult<CourseMinModel>.GetEmpty();
        }
    }






    public async Task<HttpResponseMessage> CreateAsync(CreateCourseDto dto, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.PostAsync("/api/courses", ToFormData(dto));
        return result;
    }

    public async Task<HttpResponseMessage> UpdateAsync(UpdateCourseDto dto, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.PatchAsync("/api/courses", ToFormData(dto));
        return result;
    }

    public async Task<HttpResponseMessage> DeleteAsync(Guid id, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.DeleteAsync($"/api/courses/{id}");
        return result;
    }






    public async Task<bool> IsEnrolled(Guid courseId, HttpContext context)
    {
        try
        {
            _client.AddBearerHeader(context);
            var result = await _client.GetFromJsonAsync<bool>(
                $"api/enrollments?courseId={courseId}", SerializeOptions.JsonOptions);
            return result!;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<EnrollmentModel>> GetEnrollmentsAsync(HttpContext context)
    {
        try
        {
            _client.AddBearerHeader(context);
            var result = await _client.GetFromJsonAsync<List<EnrollmentModel>>(
                "api/enrollments/courses", SerializeOptions.JsonOptions);

            foreach (var item in result!)
            {
                if (item.Course is not null && !ResourceHelper.IsRemote(item.Course.ThumbUrl))
                    item.Course.ThumbUrl = Configurer.GetApiClientOptions().ApiServerPath + $"/api/courses/Resource/{item.Course.Id}/local-thumb";
            }

            return result;
        }
        catch
        {
            return new();
        }
    }

    public async Task<EnrollmentFullModel?> GetEnrollmentAsync(HttpContext context, Guid courseId)
    {
        try
        {
            _client.AddBearerHeader(context);
            var result = await _client.GetFromJsonAsync<EnrollmentFullModel>(
                $"api/enrollments/ByCourse/{courseId}", SerializeOptions.JsonOptions);
            return result!;
        }
        catch
        {
            return null;
        }
    }

    public async Task<HttpResponseMessage> GrantEnrollmentAsync(Guid courseId, Guid userId, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.PostAsJsonAsync("/api/courses", new { courseId = courseId, userId = userId });
        return result;
    }






    private MultipartFormDataContent ToFormData(CreateCourseDto dto)
    {
        FormDataHelper helper = new()
        {
            KeyValuePairs = new()
            {
                { nameof(dto.Title), dto.Title },
                { "Thumb.Url", dto.Thumb.Url },
                { nameof(dto.Intro), dto.Intro },
                { nameof(dto.Description), dto.Description },
                { nameof(dto.Price), dto.Price.ToString() },
                { nameof(dto.Level), ((byte)dto.Level).ToString() },
                { nameof(dto.Outcomes), dto.Outcomes },
                { nameof(dto.Requirements), dto.Requirements },
                { nameof(dto.LeafCategoryId), dto.LeafCategoryId.ToString() }
            }
        };

        if (dto.SectionNames is not null && dto.SectionNames.Count > 0)
        {
            for (int i = 0; i < dto.SectionNames.Count; i++)
                helper.KeyValuePairs.Add($"SectionNames[{i}]", dto.SectionNames[i]);
        }

        if (dto.Thumb.File is not null)
        {
            helper.Files = new List<(Stream, string, string)>
            {
                (dto.Thumb.File.OpenReadStream(), "Thumb.File", dto.Thumb.File.FileName)
            };
        }

        /*if (dto.Metas is not null)
        {
            for (int i = 0; i < dto.Metas.Count; i++)
                helper.KeyValuePairs.Add($"Metas[{i}]", dto)
        }*/

        return helper.ToFormData();
    }


    private MultipartFormDataContent ToFormData(UpdateCourseDto dto)
    {
        FormDataHelper helper = new()
        {
            KeyValuePairs = new()
            {
                { nameof(dto.Id), dto.Id.ToString() },
                { nameof(dto.Title), dto.Title },
                { "Thumb.Url", dto.Thumb?.Url },
                { nameof(dto.Intro), dto.Intro },
                { nameof(dto.Description), dto.Description },
                { nameof(dto.Status), dto.Status is not null ? ((byte)dto.Status).ToString() : null },
                { nameof(dto.Price), dto.Price.ToString() },
                { nameof(dto.Discount), dto.Discount.ToString() },
                { nameof(dto.DiscountExpiry), (dto.DiscountExpiry is not null ? dto.DiscountExpiry.ToString() : null) },
                { nameof(dto.Level), dto.Level is not null ? ((byte)dto.Level).ToString() : null },
                { nameof(dto.Outcomes), dto.Outcomes },
                { nameof(dto.Requirements), dto.Requirements },
                { nameof(dto.LeafCategoryId), dto.LeafCategoryId.ToString() }
            }
        };

        if (dto.AddedSections is not null && dto.AddedSections.Count > 0)
        {
            for (int i = 0; i < dto.AddedSections.Count; i++)
                helper.KeyValuePairs.Add($"AddedSections[{i}]", dto.AddedSections[i]);
        }
        if (dto.RemovedSections is not null && dto.RemovedSections.Count > 0)
        {
            for (int i = 0; i < dto.RemovedSections.Count; i++)
                helper.KeyValuePairs.Add($"RemovedSections[{i}]", dto.RemovedSections[i].ToString());
        }

        if (dto.Thumb?.File is not null)
        {
            helper.Files = new List<(Stream, string, string)>
            {
                (dto.Thumb.File.OpenReadStream(), "Thumb.File", dto.Thumb.File.FileName)
            };
        }

        /*if (dto.Metas is not null)
        {
            for (int i = 0; i < dto.Metas.Count; i++)
                helper.KeyValuePairs.Add($"Metas[{i}]", dto)
        }*/

        return helper.ToFormData();
    }
}
