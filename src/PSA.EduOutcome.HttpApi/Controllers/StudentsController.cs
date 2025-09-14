using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using PSA.EduOutcome.Controllers;
using PSA.EduOutcome.Students;
using PSA.EduOutcome.Students.Dtos;

namespace PSA.EduOutcome.Controllers;

[Route("api/students")]
public class StudentsController : EduOutcomeController
{
    private readonly IStudentAppService _studentAppService;

    public StudentsController(IStudentAppService studentAppService)
    {
        _studentAppService = studentAppService;
    }

    [HttpGet("{id}")]
    public async Task<StudentDto> Get(Guid id)
    {
        // Temporary fix - cast to base interface to access GetAsync
        var crudService = _studentAppService as ICrudAppService<StudentDto, Guid>;
        return await crudService.GetAsync(id);
    }
}
