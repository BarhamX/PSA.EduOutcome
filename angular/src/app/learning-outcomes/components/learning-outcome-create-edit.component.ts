import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToasterService } from '@abp/ng.theme.shared';
import { finalize } from 'rxjs/operators';

import { LearningOutcomeService } from '../../proxy/learning-outcomes/learning-outcome.service';
import { CourseService } from '../../proxy/courses/course.service';
import { LearningOutcomeDto, CreateUpdateLearningOutcomeDto } from '../../proxy/dtos/models';
import { CourseDto, GetCourseListDto } from '../../proxy/courses/dtos/models';
import { PagedAndSortedResultRequestDto } from '@abp/ng.core';

@Component({
  selector: 'app-learning-outcome-create-edit',
  templateUrl: './learning-outcome-create-edit.component.html',
  styleUrls: ['./learning-outcome-create-edit.component.scss']
})
export class LearningOutcomeCreateEditComponent implements OnInit {
  form: FormGroup;
  courses: CourseDto[] = [];
  loading = false;
  saving = false;

  isEditMode = false;
  learningOutcomeId: string;
  learningOutcome: LearningOutcomeDto;

  // Categories for dropdown
  categories = [
    { value: 'Knowledge', label: 'Knowledge' },
    { value: 'Comprehension', label: 'Comprehension' },
    { value: 'Application', label: 'Application' },
    { value: 'Analysis', label: 'Analysis' },
    { value: 'Synthesis', label: 'Synthesis' },
    { value: 'Evaluation', label: 'Evaluation' }
  ];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private learningOutcomeService: LearningOutcomeService,
    private courseService: CourseService,
    private toaster: ToasterService
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.learningOutcomeId = this.route.snapshot.params['id'];
    this.isEditMode = !!this.learningOutcomeId;

    this.loadCourses();

    if (this.isEditMode) {
      this.loadLearningOutcome();
    } else {
      // Pre-select course if coming from course detail page
      const courseId = this.route.snapshot.queryParams['courseId'];
      if (courseId) {
        this.form.patchValue({ courseId });
      }
    }
  }

  createForm(): void {
    this.form = this.fb.group({
      code: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.required, Validators.maxLength(500)]],
      maxMark: [0, [Validators.required, Validators.min(1), Validators.max(100)]],
      courseId: ['', Validators.required],
      category: ['Knowledge', Validators.required],
      displayOrder: [1, [Validators.required, Validators.min(1)]]
    });
  }

  loadCourses(): void {
    const request: GetCourseListDto = {
      maxResultCount: 1000,
      sorting: 'name'
    };

    this.courseService.getList(request).subscribe(
      response => {
        this.courses = response.items.filter(c => c.status === 'Active');
      },
      error => {
        this.toaster.error('An error occurred while loading courses');
      }
    );
  }

  loadLearningOutcome(): void {
    this.loading = true;

    this.learningOutcomeService
      .get(this.learningOutcomeId)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(
        response => {
          this.learningOutcome = response;
          this.form.patchValue({
            code: response.code,
            description: response.description,
            maxMark: response.maxMark,
            courseId: response.courseId,
            category: response.category,
            displayOrder: response.displayOrder
          });
        },
        error => {
          this.toaster.error('An error occurred while loading the learning outcome');
          this.router.navigate(['/learning-outcomes']);
        }
      );
  }

  save(): void {
    if (this.form.invalid) {
      Object.keys(this.form.controls).forEach(key => {
        this.form.get(key)?.markAsTouched();
      });
      return;
    }

    this.saving = true;
    const request: CreateUpdateLearningOutcomeDto = this.form.value;

    const saveOperation = this.isEditMode
      ? this.learningOutcomeService.update(this.learningOutcomeId, request)
      : this.learningOutcomeService.create(request);

    saveOperation
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(
        response => {
          const message = this.isEditMode
            ? 'Learning outcome updated successfully'
            : 'Learning outcome created successfully';

          this.toaster.success(message);
          this.router.navigate(['/learning-outcomes']);
        },
        error => {
          const errorMessage = this.getErrorMessage(error);
          this.toaster.error(errorMessage);
        }
      );
  }

  cancel(): void {
    this.router.navigate(['/learning-outcomes']);
  }

  onCourseChange(): void {
    const selectedCourseId = this.form.get('courseId')?.value;
    if (selectedCourseId) {
      // Auto-suggest next display order
      this.learningOutcomeService.getByCourse(selectedCourseId).subscribe(
        outcomes => {
          const maxOrder = Math.max(...outcomes.map(o => o.displayOrder), 0);
          this.form.patchValue({ displayOrder: maxOrder + 1 });
        },
        error => {
          // Ignore error, keep default value
        }
      );
    }
  }

  // Validation helpers
  isFieldInvalid(fieldName: string): boolean {
    const field = this.form.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.form.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) {
        return `${this.getFieldLabel(fieldName)} is required`;
      }
      if (field.errors['maxlength']) {
        return `${this.getFieldLabel(fieldName)} cannot exceed ${field.errors['maxlength'].requiredLength} characters`;
      }
      if (field.errors['min']) {
        return `${this.getFieldLabel(fieldName)} must be at least ${field.errors['min'].min}`;
      }
      if (field.errors['max']) {
        return `${this.getFieldLabel(fieldName)} cannot exceed ${field.errors['max'].max}`;
      }
    }
    return '';
  }

  private getFieldLabel(fieldName: string): string {
    const labels: { [key: string]: string } = {
      code: 'Code',
      description: 'Description',
      maxMark: 'Max Mark',
      courseId: 'Course',
      category: 'Category',
      displayOrder: 'Display Order'
    };
    return labels[fieldName] || fieldName;
  }

  private getErrorMessage(error: any): string {
    if (error?.error?.message) {
      return error.error.message;
    }

    if (error?.message) {
      return error.message;
    }

    return 'An unexpected error occurred. Please try again.';
  }

  get pageTitle(): string {
    return this.isEditMode ? 'Edit Learning Outcome' : 'Create Learning Outcome';
  }

  get submitButtonText(): string {
    if (this.saving) {
      return this.isEditMode ? 'Updating...' : 'Creating...';
    }
    return this.isEditMode ? 'Update' : 'Create';
  }
}