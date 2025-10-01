using AutoMapper;
using PSA.EduOutcome.Courses.Dtos;
using PSA.EduOutcome.Courses;
using PSA.EduOutcome.Students.Dtos;
using PSA.EduOutcome.Students;
using PSA.EduOutcome.DTOs;
using PSA.EduOutcome.Entities;

namespace PSA.EduOutcome;

public class EduOutcomeApplicationAutoMapperProfile : Profile
{
    public EduOutcomeApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        
        // Student mappings
        CreateMap<Student, Students.Dtos.StudentDto>();
        CreateMap<Students.Dtos.CreateStudentDto, Student>();
        CreateMap<Students.Dtos.UpdateStudentDto, Student>();
        CreateMap<GetStudentListDto, Student>();
        
        // Course mappings
        CreateMap<Course, Courses.Dtos.CourseDto>();
        CreateMap<CreateCourseDto, Course>();
        CreateMap<UpdateCourseDto, Course>();
        CreateMap<GetCourseListDto, Course>();
        
        // Learning Outcome mappings
        CreateMap<LearningOutcome, LearningOutcomeDto>();
        CreateMap<CreateUpdateLearningOutcomeDto, LearningOutcome>();
        CreateMap<LearningOutcome, LearningOutcomeDetailDto>();
        
        // Assessment mappings
        CreateMap<Assessment, AssessmentDto>();
        CreateMap<CreateUpdateAssessmentDto, Assessment>();
        CreateMap<Assessment, AssessmentDetailDto>();
        
        // Question mappings
        CreateMap<Question, QuestionDto>();
        CreateMap<CreateUpdateQuestionDto, Question>();
        
        // Question-Learning Outcome mapping
        CreateMap<QuestionLearningOutcome, QuestionMappingDto>()
            .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
            .ForMember(dest => dest.AllocatedMark, opt => opt.MapFrom(src => src.AllocatedMark))
            .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.Percentage));
    }
}

