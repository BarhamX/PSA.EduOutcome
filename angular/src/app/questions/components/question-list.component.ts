import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';

import { QuestionService } from '../../proxy/questions/question.service';
import { AssessmentService } from '../../proxy/assessments/assessment.service';
import { AssessmentDto, GetQuestionListDto, QuestionDto } from '../../proxy/dtos/models';

@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.scss']
})
export class QuestionListComponent implements OnInit {
  data: PagedResultDto<QuestionDto> = {} as PagedResultDto<QuestionDto>;
  assessments: AssessmentDto[] = [];
  loading = false;

  skipCount = 0;
  maxResultCount = 10;
  totalCount = 0;
  currentPage = 1;

  filterForm: FormGroup;

  questionTypes = [
    { value: '', label: 'All Types' },
    { value: 'MultipleChoice', label: 'Multiple Choice' },
    { value: 'Essay', label: 'Essay' },
    { value: 'ShortAnswer', label: 'Short Answer' },
    { value: 'TrueFalse', label: 'True/False' },
    { value: 'Numerical', label: 'Numerical' },
    { value: 'Matching', label: 'Matching' },
    { value: 'FillInTheBlank', label: 'Fill in the Blank' },
  ];

  mappingStatusOptions = [
    { value: '', label: 'All Questions' },
    { value: 'withMappings', label: 'With Learning Outcome Mappings' },
    { value: 'withoutMappings', label: 'Without Learning Outcome Mappings' },
  ];

  constructor(
    private readonly questionService: QuestionService,
    private readonly assessmentService: AssessmentService,
    private readonly router: Router,
    private readonly confirmation: ConfirmationService,
    private readonly toaster: ToasterService,
    private readonly fb: FormBuilder,
  ) {
    this.filterForm = this.fb.group({
      filter: [''],
      assessmentId: [''],
      questionType: [''],
      mappingStatus: [''],
    });
  }

  ngOnInit(): void {
    this.loadAssessments();
    this.getQuestions();

    this.filterForm.valueChanges.subscribe(() => {
      this.skipCount = 0;
      this.getQuestions();
    });
  }

  loadAssessments(): void {
    this.assessmentService
      .getList({ skipCount: 0, maxResultCount: 100, sorting: 'Title' })
      .subscribe({
        next: response => (this.assessments = response.items ?? []),
        error: () => this.toaster.error('Failed to load assessments'),
      });
  }

  getQuestions(): void {
    this.loading = true;
    const { filter, assessmentId, questionType, mappingStatus } = this.filterForm.value;

    const request: GetQuestionListDto = {
      skipCount: this.skipCount,
      maxResultCount: this.maxResultCount,
      sorting: 'QuestionNumber',
      filter: filter?.trim() || undefined,
      assessmentId: assessmentId || undefined,
      questionType: questionType || undefined,
      hasLearningOutcomeMappings:
        mappingStatus === 'withMappings' ? true : mappingStatus === 'withoutMappings' ? false : undefined,
    };

    this.questionService
      .getList(request)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: response => {
          this.data = response;
          this.totalCount = response.totalCount;
        },
        error: () => this.toaster.error('Failed to load questions'),
      });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.skipCount = (page - 1) * this.maxResultCount;
    this.getQuestions();
  }

  onPageSizeChange(pageSize: number): void {
    this.maxResultCount = pageSize;
    this.skipCount = 0;
    this.getQuestions();
  }

  clearFilters(): void {
    this.filterForm.reset({
      filter: '',
      assessmentId: '',
      questionType: '',
      mappingStatus: '',
    });
  }

  createNew(): void {
    this.router.navigate(['/questions/new']);
  }

  edit(id: string): void {
    this.router.navigate(['/questions', id, 'edit']);
  }

  delete(id: string, questionLabel: string): void {
    const confirmation: Confirmation.Options = {
      title: 'Delete Question',
      message: `Are you sure you want to delete question "${questionLabel}"?`,
      severity: 'warning',
    };

    this.confirmation
      .show(confirmation.message, confirmation.title, confirmation.severity as any)
      .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          this.questionService.delete(id).subscribe({
            next: () => {
              this.toaster.success('Question deleted successfully');
              this.getQuestions();
            },
            error: error => {
              const message = error?.error?.error?.message || 'Failed to delete question';
              this.toaster.error(message);
            },
          });
        }
      });
  }

  getAssessmentLabel(assessmentId?: string): string {
    const assessment = this.assessments.find(a => a.id === assessmentId);
    return assessment ? `${assessment.title} (${assessment.type})` : 'Unknown Assessment';
  }

  trackByFn(index: number, item: QuestionDto): string {
    return item.id;
  }

  Math = Math;
}
