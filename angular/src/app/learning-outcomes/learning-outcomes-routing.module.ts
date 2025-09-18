import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LearningOutcomeListComponent } from './components/learning-outcome-list.component';
import { LearningOutcomeCreateEditComponent } from './components/learning-outcome-create-edit.component';
import { LearningOutcomeDetailComponent } from './components/learning-outcome-detail.component';
import { PermissionGuard } from '@abp/ng.core';

const routes: Routes = [
  {
    path: '',
    component: LearningOutcomeListComponent,
    canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'EduOutcome.LearningOutcomes'
    }
  },
  {
    path: 'new',
    component: LearningOutcomeCreateEditComponent,
    canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'EduOutcome.LearningOutcomes.Create'
    }
  },
  {
    path: ':id',
    component: LearningOutcomeDetailComponent,
    canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'EduOutcome.LearningOutcomes'
    }
  },
  {
    path: ':id/edit',
    component: LearningOutcomeCreateEditComponent,
    canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'EduOutcome.LearningOutcomes.Edit'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LearningOutcomesRoutingModule { }