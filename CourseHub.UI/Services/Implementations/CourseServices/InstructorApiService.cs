﻿using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Models.Course.InstructorModels;
using CourseHub.Core.RequestDtos.Course.InstructorDtos;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CourseServices;
using Microsoft.Extensions.Options;

namespace CourseHub.UI.Services.Implementations.CourseServices;

public class InstructorApiService : IInstructorApiService
{
    private readonly HttpClient _client;

    public InstructorApiService(HttpClient client)
    {
        _client = client;
    }

    public async Task<InstructorModel?> GetByUserId(Guid userId)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<InstructorModel>(
                $"api/instructors/ByUser/{userId}");
            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<HttpResponseMessage> Update(UpdateInstructorDto dto, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.PatchAsync("api/instructors", JsonContent.Create(dto));
        return result;
    }
}
