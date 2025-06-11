using AutoMapper;
using PSA.EduOutcome.Courses.Dtos;
using PSA.EduOutcome.Courses;
using PSA.EduOutcome.Students.Dtos;
using PSA.EduOutcome.Students;

namespace PSA.EduOutcome;

public class EduOutcomeApplicationAutoMapperProfile : Profile
{
    public EduOutcomeApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Student, StudentDto>();
        CreateMap<CreateStudentDto, Student>();
        CreateMap<UpdateStudentDto, Student>();
        CreateMap<GetStudentListDto, Student>();
        CreateMap<Course, CourseDto>();
        CreateMap<CreateCourseDto, Course>();
        CreateMap<UpdateCourseDto, Course>();
        CreateMap<GetCourseListDto, Course>();
    }
}
