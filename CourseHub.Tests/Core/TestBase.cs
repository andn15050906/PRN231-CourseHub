using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace CourseHub.Tests.Core;

public class TestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly ITestOutputHelper _output;
    protected readonly HttpClient _client;

    public TestBase(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _output = output;
        _client = factory.CreateClient();
    }
}
