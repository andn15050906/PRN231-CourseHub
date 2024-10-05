using CourseHub.Core.Entities.CourseDomain.Enums;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using CourseHub.Core.RequestDtos.Course.InstructorDtos;
using CourseHub.Tests.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using Xunit;
using CourseHub.UI.Services.Contracts.CourseServices;
using CourseHub.UI.Services.Implementations.CourseServices;
using CourseHub.Core.RequestDtos.Course.CourseReviewDtos;
using CourseHub.Tests.Helpers;

namespace CourseHub.Tests.Tests;

public class CourseAPITests : TestBase
{
    private readonly Dictionary<string, Guid> _course_ids =
        new() {
            { "The Complete 2023 Web Development Bootcamp", Guid.Parse("69746C85-6109-4370-9334-1490CD2334B0") },
            { "Title", Guid.Parse("C538B99B-F724-4788-879F-ADFE3B1A90EA") }
        };

    private readonly Dictionary<string, Guid> _user_ids =
        new() {
            { "#1", Guid.Parse("522D2265-0532-4142-8079-0AA7E9C7D3CB") },
            { "#2", Guid.Parse("8C3D6D81-2D70-4B5D-87BD-0A9B2D4DA4ED") },
            { "Ho Hoang Loc", Guid.Parse("39dcb58a-5a86-4220-8366-518be3efe406") }
        };







    public CourseAPITests(WebApplicationFactory<Program> factory, ITestOutputHelper output) : base(factory, output)
    {
    }






    [Fact]
    public async Task GetCategories()
    {
        var result = await new CategoryApiService(_client).GetAsync();
        Assert.True(result.Count >= 0);
    }



    [Fact]
    public async Task GetCourseReviewsBySourceId()
    {
        var sourceId = _course_ids.First().Value;
        var result = await new CourseReviewApiService(_client).GetAsync(new QueryCourseReviewDto { CourseId = sourceId });
        Assert.True(result.TotalCount >= 0);
    }

    [Fact]
    public async Task GetCourseReviewsByCreatorId()
    {
        var creatorId = _user_ids["#1"];
        var result = await new CourseReviewApiService(_client).GetAsync(new QueryCourseReviewDto { CreatorId = creatorId });
        Assert.True(result.TotalCount > 0);
    }



    [Fact]
    public async Task GetCourses()
    {
        var result = await new CourseApiTestService(_client).GetPagedAsync(new QueryCourseDto());
        Assert.True(result.TotalCount > 0);
    }

    [Fact]
    public async Task GetCoursesByTitle()
    {
        var title = "The";
        var result = await new CourseApiTestService(_client).GetPagedAsync(new QueryCourseDto { Title = title });
        Assert.True(result.TotalCount > 0);
    }

    [Fact]
    public async Task GetCoursesByStatus()
    {
        CourseStatus status = CourseStatus.Ongoing;
        var result = await new CourseApiTestService(_client).GetPagedAsync(new QueryCourseDto { Status = status });
        Assert.True(result.TotalCount > 0);
    }

    [Fact]
    public async Task GetCoursesByLevel()
    {
        CourseLevel level = CourseLevel.Intermediate;
        var result = await new CourseApiTestService(_client).GetPagedAsync(new QueryCourseDto { Level = level });
        Assert.True(result.TotalCount > 0);
    }



    [Fact]
    public async Task GetInstructors()
    {
        var result = await new InstructorApiService(_client).Get();
        Assert.True(result.TotalCount > 0);
    }

    [Fact]
    public async Task GetInstructorsByCreatorId()
    {
        var creatorId = _user_ids["Ho Hoang Loc"];
        var result = await new InstructorApiService(_client).GetByUserId(creatorId);
        Assert.True(result.Id != Guid.Empty);
    }
}