import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToasterService } from '@abp/ng.theme.shared';
import { finalize } from 'rxjs/operators';

import { LearningOutcomeService } from '../../proxy/learning-outcomes/learning-outcome.service';
import { CourseService } from '../../proxy/courses/course.service';
import { LearningOutcomeDetailDto } from '../../proxy/dtos/models';
import { CourseDto, GetCourseListDto } from '../../proxy/courses/dtos/models';

@Component({
  selector: 'app-learning-outcome-detail',
  templateUrl: './learning-outcome-detail.component.html',
  styleUrls: ['./learning-outcome-detail.component.scss']
})
export class LearningOutcomeDetailComponent implements OnInit {
  learningOutcome: LearningOutcomeDetailDto;
  course: CourseDto;
  loading = false;
  learningOutcomeId: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private learningOutcomeService: LearningOutcomeService,
    private courseService: CourseService,
    private toaster: ToasterService
  ) {}

  ngOnInit(): void {
    this.learningOutcomeId = this.route.snapshot.params['id'];
    this.loadLearningOutcome();
  }

  loadLearningOutcome(): void {
    this.loading = true;

    this.learningOutcomeService
      .getDetail(this.learningOutcomeId)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(
        response => {
          this.learningOutcome = response;
          this.loadCourse(response.courseId);
        },
        error => {
          this.toaster.error('An error occurred while loading the learning outcome');
          this.router.navigate(['/learning-outcomes']);
        }
      );
  }

  loadCourse(courseId: string): void {
    this.courseService.get(courseId).subscribe(
      response => {
        this.course = response;
      },
      error => {
        // Don't show error, just leave course undefined
      }
    );
  }

  edit(): void {
    this.router.navigate(['/learning-outcomes', this.learningOutcomeId, 'edit']);
  }

  back(): void {
    this.router.navigate(['/learning-outcomes']);
  }

  getStatusClass(isActive: boolean): string {
    return isActive ? 'bg-success' : 'bg-secondary';
  }

  getStatusText(isActive: boolean): string {
    return isActive ? 'Active' : 'Inactive';
  }
}