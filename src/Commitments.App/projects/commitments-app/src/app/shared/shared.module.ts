// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import {
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatDividerModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatStepperModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule
} from '@angular/material';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { PrimaryHeader } from './primary-header/primary-header';
import { QuillTextEditor } from './quill-text-editor/quill-text-editor';
import { CommonModule } from '@angular/common';
import { AgGridModule } from 'ag-grid-angular';
import { DeleteCell } from './delete-cell/delete-cell';
import { AutoCompleteChipList } from './auto-complete-chip-list/auto-complete-chip-list';
import { TranslateModule } from '@ngx-translate/core';
import { EditCell } from './edit-cell/edit-cell';
import { CheckboxCell } from './checkbox-cell/checkbox-cell';
import { StarCell } from './star-cell/star-cell';

@NgModule({
  declarations: [
    AutoCompleteChipList,
    CheckboxCell,
    DeleteCell,
    EditCell,
    PrimaryHeader,
    QuillTextEditor,
    StarCell
  ],
  imports: [
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatStepperModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,

    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule,

    AgGridModule.withComponents([
      CheckboxCell,
      DeleteCell,
      EditCell,
      StarCell
    ])
  ],
  exports: [
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatStepperModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,

    AgGridModule,
    FormsModule,
    ReactiveFormsModule,

    AutoCompleteChipList,
    CheckboxCell,
    DeleteCell,
    PrimaryHeader,
    QuillTextEditor,
    PrimaryHeader
  ]
})
export class SharedModule {}
