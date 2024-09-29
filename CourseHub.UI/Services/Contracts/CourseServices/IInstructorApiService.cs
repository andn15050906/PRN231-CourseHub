﻿using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Models.Course.InstructorModels;
using CourseHub.Core.RequestDtos.Course.InstructorDtos;

namespace CourseHub.UI.Services.Contracts.CourseServices;

public interface IInstructorApiService
{
    Task<InstructorModel?> GetByUserId(Guid userId);
    Task<HttpResponseMessage> Update(UpdateInstructorDto dto, HttpContext context);
}
