import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';

import { QuestionsRoutingModule } from './questions-routing.module';
import { QuestionListComponent } from './components/question-list.component';
import { QuestionCreateEditComponent } from './components/question-create-edit.component';

@NgModule({
  declarations: [
    QuestionListComponent,
    QuestionCreateEditComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    SharedModule,
    QuestionsRoutingModule,
  ],
})
export class QuestionsModule {}
