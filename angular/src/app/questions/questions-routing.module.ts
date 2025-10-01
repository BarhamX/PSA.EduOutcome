import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionGuard } from '@abp/ng.core';
import { QuestionListComponent } from './components/question-list.component';
import { QuestionCreateEditComponent } from './components/question-create-edit.component';

const routes: Routes = [
  {
    path: '',
    component: QuestionListComponent,
    canActivate: [PermissionGuard],
    data: { requiredPolicy: 'EduOutcome.Questions' },
  },
  {
    path: 'new',
    component: QuestionCreateEditComponent,
    canActivate: [PermissionGuard],
    data: { requiredPolicy: 'EduOutcome.Questions.Create' },
  },
  {
    path: ':id/edit',
    component: QuestionCreateEditComponent,
    canActivate: [PermissionGuard],
    data: { requiredPolicy: 'EduOutcome.Questions.Edit' },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class QuestionsRoutingModule {}
