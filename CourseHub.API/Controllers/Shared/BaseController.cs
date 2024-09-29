using Microsoft.AspNetCore.Mvc;

namespace CourseHub.API.Controllers.Shared;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : Controller
{
}