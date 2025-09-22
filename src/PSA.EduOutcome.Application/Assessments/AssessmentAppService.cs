using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using PSA.EduOutcome.DTOs;
using PSA.EduOutcome.Entities;

namespace PSA.EduOutcome.Assessments
{
    public class AssessmentAppService : ApplicationService, IAssessmentAppService
    {
        private readonly IRepository<Assessment, Guid> _assessmentRepository;
        private readonly IRepository<Course, Guid> _courseRepository;
        private readonly IRepository<Question, Guid> _questionRepository;
        private readonly IRepository<QuestionLearningOutcome, Guid> _questionMappingRepository;

        public AssessmentAppService(
            IRepository<Assessment, Guid> assessmentRepository,
            IRepository<Course, Guid> courseRepository,
            IRepository<Question, Guid> questionRepository,
            IRepository<QuestionLearningOutcome, Guid> questionMappingRepository)
        {
            _assessmentRepository = assessmentRepository;
            _courseRepository = courseRepository;
            _questionRepository = questionRepository;
            _questionMappingRepository = questionMappingRepository;
        }

        public async Task<AssessmentDto> GetAsync(Guid id)
        {
            var assessment = await _assessmentRepository.GetAsync(id);
            var assessmentDto = ObjectMapper.Map<Assessment, AssessmentDto>(assessment);
            
            // Get course name and question count
            var course = await _courseRepository.GetAsync(assessment.CourseId);
            assessmentDto.CourseName = course.Name;
            assessmentDto.QuestionCount = await _questionRepository.CountAsync(q => q.AssessmentId == id);
            
            return assessmentDto;
        }

        public async Task<PagedResultDto<AssessmentDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _assessmentRepository.GetQueryableAsync();
            
            var query = from assessment in queryable
                        join course in await _courseRepository.GetQueryableAsync() on assessment.CourseId equals course.Id
                        select new { Assessment = assessment, Course = course };

            var totalCount = await AsyncExecuter.CountAsync(query);

            var sortedQuery = string.IsNullOrEmpty(input.Sorting) 
                ? query.OrderBy(x => x.Assessment.DueDate)
                : query.OrderBy(x => x.Assessment.DueDate); // TODO: Implement dynamic sorting
            
            var pagedQuery = sortedQuery.Skip(input.SkipCount).Take(input.MaxResultCount);

            var items = await AsyncExecuter.ToListAsync(pagedQuery);

            var assessmentDtos = new List<AssessmentDto>();
            
            foreach (var item in items)
            {
                var dto = ObjectMapper.Map<Assessment, AssessmentDto>(item.Assessment);
                dto.CourseName = item.Course.Name;
                dto.QuestionCount = await _questionRepository.CountAsync(q => q.AssessmentId == item.Assessment.Id);
                assessmentDtos.Add(dto);
            }

            return new PagedResultDto<AssessmentDto>(totalCount, assessmentDtos);
        }

        public async Task<AssessmentDto> CreateAsync(CreateUpdateAssessmentDto input)
        {
            // Validate course exists
            var course = await _courseRepository.GetAsync(input.CourseId);
            
            // Validate due date is in future
            if (input.DueDate <= DateTime.Now)
            {
                throw new BusinessException("Due date must be in the future.");
            }

            // Validate course assessment weights don't exceed 100%
            await ValidateCourseWeights(input.CourseId, input.Weight);

            var assessment = new Assessment(
                GuidGenerator.Create(),
                input.Title,
                input.Type,
                input.TotalMarks,
                input.Weight,
                input.CourseId,
                input.DueDate,
                input.Description
            );

            // Configure late submission if provided
            if (input.AllowLateSubmission)
            {
                assessment.ConfigureLateSubmission(input.AllowLateSubmission, input.LatePenaltyPercentage);
            }

            await _assessmentRepository.InsertAsync(assessment);
            await CurrentUnitOfWork.SaveChangesAsync();

            var result = ObjectMapper.Map<Assessment, AssessmentDto>(assessment);
            result.CourseName = course.Name;
            result.QuestionCount = 0;
            
            return result;
        }

        public async Task<AssessmentDto> UpdateAsync(Guid id, CreateUpdateAssessmentDto input)
        {
            var assessment = await _assessmentRepository.GetAsync(id);
            
            // Cannot modify published assessments
            if (assessment.IsPublished)
            {
                throw new BusinessException("Cannot modify published assessment. Unpublish first.");
            }

            var course = await _courseRepository.GetAsync(input.CourseId);

            // Validate due date
            if (input.DueDate <= DateTime.Now)
            {
                throw new BusinessException("Due date must be in the future.");
            }

            // Validate course assessment weights (excluding current assessment)
            await ValidateCourseWeights(input.CourseId, input.Weight - assessment.Weight);

            assessment.SetTitle(input.Title);
            assessment.SetType(input.Type);
            assessment.SetTotalMarks(input.TotalMarks);
            assessment.SetWeight(input.Weight);
            assessment.SetDueDate(input.DueDate);
            assessment.ConfigureLateSubmission(input.AllowLateSubmission, input.LatePenaltyPercentage);

            await _assessmentRepository.UpdateAsync(assessment);
            await CurrentUnitOfWork.SaveChangesAsync();

            var result = ObjectMapper.Map<Assessment, AssessmentDto>(assessment);
            result.CourseName = course.Name;
            result.QuestionCount = await _questionRepository.CountAsync(q => q.AssessmentId == id);
            
            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            var assessment = await _assessmentRepository.GetAsync(id);
            
            // Cannot delete published assessments
            if (assessment.IsPublished)
            {
                throw new BusinessException("Cannot delete published assessment. Unpublish first.");
            }

            // Check if there are questions
            var hasQuestions = await _questionRepository.AnyAsync(q => q.AssessmentId == id);
            if (hasQuestions)
            {
                throw new BusinessException("Cannot delete assessment with questions. Remove questions first.");
            }

            await _assessmentRepository.DeleteAsync(assessment);
        }

        public async Task<List<AssessmentDto>> GetByCourseAsync(Guid courseId)
        {
            var assessments = await _assessmentRepository.GetListAsync(a => a.CourseId == courseId);
            var course = await _courseRepository.GetAsync(courseId);

            var assessmentDtos = new List<AssessmentDto>();
            
            foreach (var assessment in assessments.OrderBy(a => a.DueDate))
            {
                var dto = ObjectMapper.Map<Assessment, AssessmentDto>(assessment);
                dto.CourseName = course.Name;
                dto.QuestionCount = await _questionRepository.CountAsync(q => q.AssessmentId == assessment.Id);
                assessmentDtos.Add(dto);
            }

            return assessmentDtos;
        }

        public async Task<AssessmentDetailDto> GetDetailAsync(Guid id)
        {
            var assessment = await _assessmentRepository.GetAsync(id);
            var course = await _courseRepository.GetAsync(assessment.CourseId);
            
            var detailDto = ObjectMapper.Map<Assessment, AssessmentDetailDto>(assessment);
            detailDto.CourseName = course.Name;

            // Get questions for this assessment
            var questions = await _questionRepository.GetListAsync(q => q.AssessmentId == id);
            detailDto.Questions = questions.Select(q => ObjectMapper.Map<Question, QuestionDto>(q))
                                          .OrderBy(q => q.QuestionNumber)
                                          .ToList();

            detailDto.TotalAllocatedMarks = questions.Sum(q => q.MaxMark);
            detailDto.IsValid = Math.Abs(detailDto.TotalAllocatedMarks - assessment.TotalMarks) < 0.01m;

            // Validate assessment and populate validation errors
            var validation = await ValidateAssessmentInternal(assessment);
            detailDto.ValidationErrors = validation.ValidationErrors;

            return detailDto;
        }

        public async Task<AssessmentDto> PublishAsync(Guid id)
        {
            var assessment = await _assessmentRepository.GetAsync(id);
            
            if (assessment.IsPublished)
            {
                throw new BusinessException("Assessment is already published.");
            }

            // Validate assessment before publishing
            var validation = await ValidateAssessmentInternal(assessment);
            if (!validation.IsValid)
            {
                throw new BusinessException($"Cannot publish invalid assessment. Issues: {string.Join(", ", validation.ValidationErrors)}");
            }

            assessment.Publish();
            await _assessmentRepository.UpdateAsync(assessment);

            var result = await GetAsync(id);
            return result;
        }

        public async Task<AssessmentDto> UnpublishAsync(Guid id)
        {
            var assessment = await _assessmentRepository.GetAsync(id);
            
            if (!assessment.IsPublished)
            {
                throw new BusinessException("Assessment is not published.");
            }

            assessment.Unpublish();
            await _assessmentRepository.UpdateAsync(assessment);

            var result = await GetAsync(id);
            return result;
        }

        private static string NormalizeSorting(string sorting)
        {
            if (string.IsNullOrWhiteSpace(sorting))
            {
                return "Assessment.DueDate";
            }

            if (sorting.Contains("CourseName", StringComparison.OrdinalIgnoreCase))
            {
                return sorting.Replace("CourseName", "Course.Name", StringComparison.OrdinalIgnoreCase);
            }

            return sorting.StartsWith("Assessment.", StringComparison.OrdinalIgnoreCase)
                ? sorting
                : $"Assessment.{sorting}";
        }
        public async Task<AssessmentValidationResultDto> ValidateAssessmentAsync(Guid id)
        {
            var assessment = await _assessmentRepository.GetAsync(id);
            return await ValidateAssessmentInternal(assessment);
        }

        private async Task<AssessmentValidationResultDto> ValidateAssessmentInternal(Assessment assessment)
        {
            var result = new AssessmentValidationResultDto
            {
                IsValid = true,
                AssessmentTotalMarks = assessment.TotalMarks
            };

            // Get all questions for this assessment
            var questions = await _questionRepository.GetListAsync(q => q.AssessmentId == assessment.Id);
            result.TotalQuestionMarks = questions.Sum(q => q.MaxMark);

            // Validate total marks match
            if (Math.Abs(result.TotalQuestionMarks - result.AssessmentTotalMarks) > 0.01m)
            {
                result.IsValid = false;
                result.ValidationErrors.Add($"Total question marks ({result.TotalQuestionMarks}) do not match assessment total marks ({result.AssessmentTotalMarks}).");
            }

            // Validate each question has learning outcome mappings
            foreach (var question in questions)
            {
                var questionValidation = new QuestionValidationDto
                {
                    QuestionId = question.Id,
                    QuestionText = question.Text.Length > 50 ? question.Text.Substring(0, 50) + "..." : question.Text
                };

                var mappings = await _questionMappingRepository.GetListAsync(qm => qm.QuestionId == question.Id);
                questionValidation.HasMappings = mappings.Any();

                if (!questionValidation.HasMappings)
                {
                    questionValidation.Issues.Add("No learning outcome mappings");
                    result.IsValid = false;
                    result.QuestionsWithoutMappings++;
                }
                else
                {
                    // Validate mapping completeness (percentages should total 100%)
                    var totalPercentage = mappings.Sum(m => m.Percentage);
                    var totalAllocated = mappings.Sum(m => m.AllocatedMark);

                    if (Math.Abs(totalPercentage - 100) > 0.01m)
                    {
                        questionValidation.Issues.Add($"Mapping percentages total {totalPercentage}% (should be 100%)");
                        result.IsValid = false;
                    }

                    if (Math.Abs(totalAllocated - question.MaxMark) > 0.01m)
                    {
                        questionValidation.Issues.Add($"Allocated marks ({totalAllocated}) don't match question marks ({question.MaxMark})");
                        result.IsValid = false;
                    }

                    questionValidation.MappingsComplete = questionValidation.Issues.Count == 0;
                }

                result.QuestionValidations.Add(questionValidation);
            }

            // Check if assessment is overdue for new assessments
            if (assessment.DueDate <= DateTime.Now && !assessment.IsPublished)
            {
                result.ValidationErrors.Add("Due date has passed. Consider updating the due date or publishing immediately.");
            }

            return result;
        }

        private async Task ValidateCourseWeights(Guid courseId, decimal additionalWeight)
        {
            var existingAssessments = await _assessmentRepository.GetListAsync(a => a.CourseId == courseId);
            var existingTotalWeight = existingAssessments.Sum(a => a.Weight);

            var newTotal = existingTotalWeight + additionalWeight;

            if (newTotal > 100.01m) // Allow small floating point tolerance
            {
                throw new BusinessException($"Total course assessment weights would exceed 100%. Current: {existingTotalWeight}%, Adding: {additionalWeight}%, Total: {newTotal}%");
            }
        }
    }
}