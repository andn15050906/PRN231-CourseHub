using CourseHub.API.Controllers.Shared;
using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.RequestDtos.Course.CategoryDtos;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CourseHub.API.Controllers.CourseControllers;

[Consumes(contentType: MediaTypeNames.Application.Json)]
[Produces(contentType: MediaTypeNames.Application.Json)]
public class CategoriesController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }



    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _categoryService.GetAllAsync();
        return result.AsResponse();
    }

    [HttpPost]
    [Authorize(Roles = RoleConstants.SYSADMIN)]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var result = await _categoryService.CreateAsync(dto);
        return result.AsResponse();
    }

    [HttpPatch]
    [Authorize(Roles = RoleConstants.SYSADMIN)]
    public async Task<IActionResult> Update(UpdateCategoryDto dto)
    {
        var result = await _categoryService.UpdateAsync(dto);
        return result.AsResponse();
    }

    [HttpDelete]
    [Authorize(Roles = RoleConstants.SYSADMIN)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _categoryService.DeleteAsync(id);
        return result.AsResponse();
    }
}
