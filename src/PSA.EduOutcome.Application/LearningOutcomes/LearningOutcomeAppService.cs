using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;
using PSA.EduOutcome.DTOs;
using PSA.EduOutcome.Entities;

namespace PSA.EduOutcome.LearningOutcomes
{
    public class LearningOutcomeAppService : ApplicationService, ILearningOutcomeAppService
    {
        private readonly IRepository<LearningOutcome, Guid> _learningOutcomeRepository;
        private readonly IRepository<Course, Guid> _courseRepository;
        private readonly IRepository<QuestionLearningOutcome, Guid> _questionMappingRepository;

        public LearningOutcomeAppService(
            IRepository<LearningOutcome, Guid> learningOutcomeRepository,
            IRepository<Course, Guid> courseRepository,
            IRepository<QuestionLearningOutcome, Guid> questionMappingRepository)
        {
            _learningOutcomeRepository = learningOutcomeRepository;
            _courseRepository = courseRepository;
            _questionMappingRepository = questionMappingRepository;
        }

        public async Task<LearningOutcomeDto> GetAsync(Guid id)
        {
            var learningOutcome = await _learningOutcomeRepository.GetAsync(id);
            var learningOutcomeDto = ObjectMapper.Map<LearningOutcome, LearningOutcomeDto>(learningOutcome);
            
            // Get course name
            var course = await _courseRepository.GetAsync(learningOutcome.CourseId);
            learningOutcomeDto.CourseName = course.Name;
            
            return learningOutcomeDto;
        }

        public async Task<PagedResultDto<LearningOutcomeDto>> GetListAsync(GetLearningOutcomeListDto input)
        {
            var queryable = await _learningOutcomeRepository.WithDetailsAsync(lo => lo.Course);

            queryable = queryable
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    lo => lo.Code.Contains(input.Filter) || lo.Description.Contains(input.Filter))
                .WhereIf(input.CourseId.HasValue, lo => lo.CourseId == input.CourseId.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Category), lo => lo.Category == input.Category);

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var sorting = string.IsNullOrWhiteSpace(input.Sorting)
                ? nameof(LearningOutcome.DisplayOrder)
                : input.Sorting;

            var items = await AsyncExecuter.ToListAsync(
                queryable
                    .OrderBy(sorting)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            var dtos = items.Select(lo =>
            {
                var dto = ObjectMapper.Map<LearningOutcome, LearningOutcomeDto>(lo);
                dto.CourseName = lo.Course?.Name;
                return dto;
            }).ToList();

            return new PagedResultDto<LearningOutcomeDto>(totalCount, dtos);
        }

        public async Task<LearningOutcomeDto> CreateAsync(CreateUpdateLearningOutcomeDto input)
        {
            // Validate course exists
            var course = await _courseRepository.GetAsync(input.CourseId);
            
            // Validate code uniqueness within course
            await ValidateCodeUniqueness(input.Code, input.CourseId);
            
            // Validate course total marks won't exceed 100
            await ValidateCourseTotalMarks(input.CourseId, input.MaxMark);

            var learningOutcome = new LearningOutcome(
                GuidGenerator.Create(),
                input.Code,
                input.Description,
                input.MaxMark,
                input.CourseId,
                input.Category,
                input.DisplayOrder
            );

            await _learningOutcomeRepository.InsertAsync(learningOutcome);
            await CurrentUnitOfWork.SaveChangesAsync();

            var result = ObjectMapper.Map<LearningOutcome, LearningOutcomeDto>(learningOutcome);
            result.CourseName = course.Name;
            
            return result;
        }

        public async Task<LearningOutcomeDto> UpdateAsync(Guid id, CreateUpdateLearningOutcomeDto input)
        {
            var learningOutcome = await _learningOutcomeRepository.GetAsync(id);
            var course = await _courseRepository.GetAsync(input.CourseId);

            // Validate code uniqueness within course (excluding current)
            await ValidateCodeUniqueness(input.Code, input.CourseId, id);
            
            // Check if mark reduction would invalidate existing mappings
            if (input.MaxMark < learningOutcome.MaxMark)
            {
                await ValidateMarkReduction(id, input.MaxMark);
            }

            // Validate course total marks
            await ValidateCourseTotalMarks(input.CourseId, input.MaxMark - learningOutcome.MaxMark);

            learningOutcome.SetCode(input.Code);
            learningOutcome.SetDescription(input.Description);
            learningOutcome.SetMaxMark(input.MaxMark);
            learningOutcome.SetCategory(input.Category);
            learningOutcome.SetDisplayOrder(input.DisplayOrder);

            await _learningOutcomeRepository.UpdateAsync(learningOutcome);
            await CurrentUnitOfWork.SaveChangesAsync();

            var result = ObjectMapper.Map<LearningOutcome, LearningOutcomeDto>(learningOutcome);
            result.CourseName = course.Name;
            
            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            var learningOutcome = await _learningOutcomeRepository.GetAsync(id);
            
            // Check if there are active question mappings
            var hasActiveMappings = await _questionMappingRepository.AnyAsync(qlo => qlo.LearningOutcomeId == id);
            if (hasActiveMappings)
            {
                throw new BusinessException("Cannot delete learning outcome with active question mappings.");
            }

            await _learningOutcomeRepository.DeleteAsync(learningOutcome);
        }

        public async Task<List<LearningOutcomeDto>> GetByCourseAsync(Guid courseId)
        {
            var learningOutcomes = await _learningOutcomeRepository.GetListAsync(lo => lo.CourseId == courseId);
            var course = await _courseRepository.GetAsync(courseId);

            return learningOutcomes.Select(lo =>
            {
                var dto = ObjectMapper.Map<LearningOutcome, LearningOutcomeDto>(lo);
                dto.CourseName = course.Name;
                return dto;
            }).OrderBy(dto => dto.DisplayOrder).ToList();
        }

        public async Task<LearningOutcomeDetailDto> GetDetailAsync(Guid id)
        {
            var learningOutcome = await _learningOutcomeRepository.GetAsync(id);
            var course = await _courseRepository.GetAsync(learningOutcome.CourseId);
            
            var detailDto = ObjectMapper.Map<LearningOutcome, LearningOutcomeDetailDto>(learningOutcome);
            detailDto.CourseName = course.Name;

            // Get question mappings for this learning outcome
            var mappings = await _questionMappingRepository.GetListAsync(qm => qm.LearningOutcomeId == id);
            
            detailDto.QuestionMappings = new List<QuestionMappingDto>();
            
            foreach (var mapping in mappings)
            {
                // Get question and assessment details  
                var questionRepo = LazyServiceProvider.LazyGetRequiredService<IRepository<Question, Guid>>();
                var assessmentRepo = LazyServiceProvider.LazyGetRequiredService<IRepository<Assessment, Guid>>();
                var question = await questionRepo.GetAsync(mapping.QuestionId);
                var assessment = await assessmentRepo.GetAsync(question.AssessmentId);
                
                detailDto.QuestionMappings.Add(new QuestionMappingDto
                {
                    QuestionId = question.Id,
                    QuestionText = question.Text,
                    AssessmentId = assessment.Id,
                    AssessmentTitle = assessment.Title,
                    AllocatedMark = mapping.AllocatedMark,
                    Percentage = mapping.Percentage
                });
            }

            detailDto.TotalAllocatedMarks = detailDto.QuestionMappings.Sum(qm => qm.AllocatedMark);
            detailDto.TotalQuestions = detailDto.QuestionMappings.Count;

            return detailDto;
        }

        public async Task<CourseValidationResultDto> ValidateCourseOutcomesAsync(Guid courseId)
        {
            var result = new CourseValidationResultDto { IsValid = true };
            var learningOutcomes = await _learningOutcomeRepository.GetListAsync(lo => lo.CourseId == courseId && lo.IsActive);

            result.TotalMarks = learningOutcomes.Sum(lo => lo.MaxMark);

            // Validate total marks equal 100
            if (Math.Abs(result.TotalMarks - 100) > 0.01m)
            {
                result.IsValid = false;
                result.ValidationErrors.Add($"Course learning outcomes total marks ({result.TotalMarks}) must equal 100.");
            }

            // Validate each learning outcome
            foreach (var outcome in learningOutcomes)
            {
                var validation = new LearningOutcomeValidationDto
                {
                    Id = outcome.Id,
                    Code = outcome.Code,
                    MaxMark = outcome.MaxMark
                };

                // Check if outcome has question mappings
                validation.HasQuestionMappings = await _questionMappingRepository.AnyAsync(qm => qm.LearningOutcomeId == outcome.Id);

                if (!validation.HasQuestionMappings)
                {
                    validation.Issues.Add("No questions mapped to this learning outcome");
                    result.IsValid = false;
                }

                result.OutcomeValidations.Add(validation);
            }

            return result;
        }

        private async Task ValidateCodeUniqueness(string code, Guid courseId, Guid? excludeId = null)
        {
            var existingOutcomes = await _learningOutcomeRepository.GetListAsync(
                lo => lo.Code == code && lo.CourseId == courseId && (excludeId == null || lo.Id != excludeId)
            );

            if (existingOutcomes.Any())
            {
                throw new BusinessException($"Learning outcome code '{code}' already exists in this course.");
            }
        }

        private async Task ValidateCourseTotalMarks(Guid courseId, decimal additionalMarks = 0)
        {
            var existingOutcomes = await _learningOutcomeRepository.GetListAsync(lo => lo.CourseId == courseId && lo.IsActive);
            var existingTotal = existingOutcomes.Sum(lo => lo.MaxMark);

            var newTotal = existingTotal + additionalMarks;

            if (newTotal > 100.01m) // Allow small floating point tolerance
            {
                throw new BusinessException($"Total course learning outcome marks would exceed 100. Current: {existingTotal}, Adding: {additionalMarks}, Total: {newTotal}");
            }
        }

        private async Task ValidateMarkReduction(Guid learningOutcomeId, decimal newMaxMark)
        {
            var mappings = await _questionMappingRepository.GetListAsync(qm => qm.LearningOutcomeId == learningOutcomeId);
            var totalAllocated = mappings.Sum(qm => qm.AllocatedMark);

            if (totalAllocated > newMaxMark)
            {
                throw new BusinessException($"Cannot reduce max mark to {newMaxMark}. Total allocated marks in question mappings: {totalAllocated}");
            }
        }
    }
}









