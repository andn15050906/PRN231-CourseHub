using CourseHub.Core.Models.Assignment.SubmissionModels;
using CourseHub.Core.RequestDtos.Assignment.SubmissionDtos;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.AssignmentServices;

namespace CourseHub.UI.Services.Implementations.AssignmentServices;

public class SubmissionApiService : ISubmissionApiService
{
    private readonly HttpClient _client;

    public SubmissionApiService(HttpClient client)
    {
        _client = client;
    }



    public async Task<SubmissionModel?> GetAsync(Guid id, HttpContext context)
    {
        try
        {
            _client.AddBearerHeader(context);
            var result = await _client.GetFromJsonAsync<SubmissionModel>(
                $"api/submissions/{id}", SerializeOptions.JsonOptions);
            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<SubmissionMinModel>> GetByAssignmentAsync(Guid assignmentId, HttpContext context)
    {
        try
        {
            _client.AddBearerHeader(context);
            var result = await _client.GetFromJsonAsync<List<SubmissionMinModel>>(
                $"api/submissions/ByAssignment?assignmentId={assignmentId}", SerializeOptions.JsonOptions);
            return result!;
        }
        catch
        {
            return new();
        }
    }



    public async Task<HttpResponseMessage> CreateAsync(CreateSubmissionDto dto, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.PostAsJsonAsync("/api/submissions", dto);
        return result;
    }
}
