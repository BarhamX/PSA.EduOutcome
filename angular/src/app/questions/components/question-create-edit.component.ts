import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { ToasterService } from '@abp/ng.theme.shared';

import { QuestionService } from '../../proxy/questions/question.service';
import { AssessmentService } from '../../proxy/assessments/assessment.service';
import { AssessmentDto, CreateUpdateQuestionDto } from '../../proxy/dtos/models';

@Component({
  selector: 'app-question-create-edit',
  templateUrl: './question-create-edit.component.html',
  styleUrls: ['./question-create-edit.component.scss']
})
export class QuestionCreateEditComponent implements OnInit {
  form: FormGroup;
  assessments: AssessmentDto[] = [];
  loading = false;
  saving = false;
  isEdit = false;
  questionId?: string;

  questionTypes = [
    { value: 'MultipleChoice', label: 'Multiple Choice' },
    { value: 'Essay', label: 'Essay' },
    { value: 'ShortAnswer', label: 'Short Answer' },
    { value: 'TrueFalse', label: 'True/False' },
    { value: 'Numerical', label: 'Numerical' },
    { value: 'Matching', label: 'Matching' },
    { value: 'FillInTheBlank', label: 'Fill in the Blank' },
  ];

  constructor(
    private readonly fb: FormBuilder,
    private readonly questionService: QuestionService,
    private readonly assessmentService: AssessmentService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly toaster: ToasterService,
  ) {
    this.form = this.fb.group({
      assessmentId: ['', Validators.required],
      questionNumber: [1, [Validators.required, Validators.min(1)]],
      questionType: ['', Validators.required],
      maxMark: [1, [Validators.required, Validators.min(0.5)]],
      text: ['', [Validators.required, Validators.maxLength(2000)]],
      description: [''],
    });
  }

  ngOnInit(): void {
    this.questionId = this.route.snapshot.paramMap.get('id') ?? undefined;
    this.isEdit = !!this.questionId;

    this.loadAssessments({ onComplete: () => {
      if (this.isEdit && this.questionId) {
        this.loadQuestion(this.questionId);
      }
    }});
  }

  loadAssessments({ onComplete }: { onComplete?: () => void } = {}): void {
    this.loading = true;
    this.assessmentService
      .getList({ skipCount: 0, maxResultCount: 100, sorting: 'title' })
      .pipe(finalize(() => {
        this.loading = false;
        onComplete?.();
      }))
      .subscribe({
        next: response => (this.assessments = response.items ?? []),
        error: () => this.toaster.error('Failed to load assessments'),
      });
  }

  loadQuestion(id: string): void {
    this.loading = true;
    this.questionService.get(id)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: question => {
          this.form.patchValue({
            assessmentId: question.assessmentId,
            questionNumber: question.questionNumber,
            questionType: question.questionType,
            maxMark: question.maxMark,
            text: question.text,
            description: question.description,
          });
        },
        error: () => {
          this.toaster.error('Failed to load question details');
          this.router.navigate(['/questions']);
        },
      });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const input: CreateUpdateQuestionDto = {
      assessmentId: this.form.value.assessmentId,
      questionNumber: this.form.value.questionNumber,
      questionType: this.form.value.questionType,
      maxMark: this.form.value.maxMark,
      text: this.form.value.text?.trim(),
      description: this.form.value.description?.trim(),
    };

    this.saving = true;
    const request$ = this.isEdit && this.questionId
      ? this.questionService.update(this.questionId, input)
      : this.questionService.create(input);

    request$
      .pipe(finalize(() => (this.saving = false)))
      .subscribe({
        next: () => {
          this.toaster.success(`Question ${this.isEdit ? 'updated' : 'created'} successfully`);
          this.router.navigate(['/questions']);
        },
        error: error => {
          const message = error?.error?.error?.message || 'Failed to save question';
          this.toaster.error(message);
        },
      });
  }

  cancel(): void {
    this.router.navigate(['/questions']);
  }

  getAssessmentTitle(assessmentId?: string): string {
    const assessment = this.assessments.find(a => a.id === assessmentId);
    return assessment ? `${assessment.title} (${assessment.type})` : '';
  }
}
