using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    public Task<StudentDto> Get(Guid id)
    {
        return _studentAppService.GetAsync(id);
    }
}
