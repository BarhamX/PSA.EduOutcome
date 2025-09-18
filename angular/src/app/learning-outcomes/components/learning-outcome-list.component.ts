import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { ToasterService } from '@abp/ng.theme.shared';
import { PagedResultDto } from '@abp/ng.core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { finalize } from 'rxjs/operators';

import { LearningOutcomeService } from '../../proxy/learning-outcomes/learning-outcome.service';
import { CourseService } from '../../proxy/courses/course.service';
import { LearningOutcomeDto, CreateUpdateLearningOutcomeDto } from '../../proxy/dtos/models';
import { CourseDto, GetCourseListDto } from '../../proxy/courses/dtos/models';
import { PagedAndSortedResultRequestDto } from '@abp/ng.core';

@Component({
  selector: 'app-learning-outcome-list',
  templateUrl: './learning-outcome-list.component.html',
  styleUrls: ['./learning-outcome-list.component.scss']
})
export class LearningOutcomeListComponent implements OnInit {
  data: PagedResultDto<LearningOutcomeDto> = {} as PagedResultDto<LearningOutcomeDto>;
  courses: CourseDto[] = [];
  loading = false;

  // Pagination
  skipCount = 0;
  maxResultCount = 10;
  totalCount = 0;
  currentPage = 1;

  // Search and Filter Form
  filterForm: FormGroup;

  // Categories for filtering
  categories = [
    { value: '', label: 'All Categories' },
    { value: 'Knowledge', label: 'Knowledge' },
    { value: 'Comprehension', label: 'Comprehension' },
    { value: 'Application', label: 'Application' },
    { value: 'Analysis', label: 'Analysis' },
    { value: 'Synthesis', label: 'Synthesis' },
    { value: 'Evaluation', label: 'Evaluation' }
  ];

  constructor(
    private learningOutcomeService: LearningOutcomeService,
    private courseService: CourseService,
    private router: Router,
    private confirmation: ConfirmationService,
    private toaster: ToasterService,
    private fb: FormBuilder
  ) {
    this.createFilterForm();
  }

  ngOnInit(): void {
    this.loadCourses();
    this.getLearningOutcomes();
  }

  createFilterForm(): void {
    this.filterForm = this.fb.group({
      filter: [''],
      courseId: [''],
      category: ['']
    });

    // React to form changes
    this.filterForm.valueChanges.subscribe(() => {
      this.skipCount = 0; // Reset pagination
      this.getLearningOutcomes();
    });
  }

  getLearningOutcomes(): void {
    this.loading = true;

    const request: PagedAndSortedResultRequestDto = {
      skipCount: this.skipCount,
      maxResultCount: this.maxResultCount
    };

    this.learningOutcomeService
      .getList(request)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(
        response => {
          this.data = response;
          this.totalCount = response.totalCount;
        },
        error => {
          this.toaster.error('An error occurred while loading learning outcomes');
        }
      );
  }

  loadCourses(): void {
    const request: GetCourseListDto = {
      maxResultCount: 1000, // Load all courses for filter
      sorting: 'name'
    };

    this.courseService.getList(request).subscribe(
      response => {
        this.courses = response.items;
      },
      error => {
        this.toaster.error('An error occurred while loading courses');
      }
    );
  }

  createNew(): void {
    this.router.navigate(['/learning-outcomes/new']);
  }

  edit(id: string): void {
    this.router.navigate(['/learning-outcomes', id, 'edit']);
  }

  view(id: string): void {
    this.router.navigate(['/learning-outcomes', id]);
  }

  delete(id: string, code: string): void {
    const confirmation = {
      title: 'Confirm Deletion',
      message: `Are you sure you want to delete the learning outcome "${code}"?`,
      severity: 'warning'
    };

    this.confirmation.show(confirmation.message, confirmation.title, confirmation.severity as any).subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.learningOutcomeService.delete(id).subscribe(
          () => {
            this.toaster.success('Learning outcome deleted successfully');
            this.getLearningOutcomes();
          },
          error => {
            this.toaster.error('An error occurred while deleting the learning outcome');
          }
        );
      }
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.skipCount = (page - 1) * this.maxResultCount;
    this.getLearningOutcomes();
  }

  onPageSizeChange(pageSize: number): void {
    this.maxResultCount = pageSize;
    this.skipCount = 0;
    this.getLearningOutcomes();
  }

  clearFilters(): void {
    this.filterForm.reset({
      filter: '',
      courseId: '',
      category: ''
    });
  }

  exportToExcel(): void {
    // Export functionality can be added later
    this.toaster.info('Export functionality will be available soon');
  }

  getCourseName(courseId: string): string {
    const course = this.courses.find(c => c.id === courseId);
    return course ? `${course.code} - ${course.name}` : 'Unknown Course';
  }

  trackByFn(index: number, item: LearningOutcomeDto): string {
    return item.id;
  }

  // Make Math available in template
  Math = Math;
}