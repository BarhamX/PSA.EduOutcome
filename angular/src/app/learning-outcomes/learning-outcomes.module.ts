import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { LearningOutcomesRoutingModule } from './learning-outcomes-routing.module';
import { LearningOutcomeListComponent } from './components/learning-outcome-list.component';
import { LearningOutcomeCreateEditComponent } from './components/learning-outcome-create-edit.component';
import { LearningOutcomeDetailComponent } from './components/learning-outcome-detail.component';

@NgModule({
  declarations: [
    LearningOutcomeListComponent,
    LearningOutcomeCreateEditComponent,
    LearningOutcomeDetailComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    LearningOutcomesRoutingModule
  ]
})
export class LearningOutcomesModule { }