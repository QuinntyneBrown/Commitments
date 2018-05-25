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
import { PrimaryHeaderComponent } from './primary-header.component';
import { QuillTextEditorComponent } from './quill-text-editor.component';
import { CommonModule } from '@angular/common';
import { AgGridModule } from 'ag-grid-angular';
import { DeleteCellComponent } from './delete-cell.component';
import { AutoCompleteChipListComponent } from './auto-complete-chip-list.component';
import { TranslateModule } from '@ngx-translate/core';
import { EditCellComponent } from './edit-cell.component';
import { CheckboxCellComponent } from './checkbox-cell.component';
import { StarCellComponent } from './star-cell.component';

@NgModule({
  declarations: [
    AutoCompleteChipListComponent,
    CheckboxCellComponent,
    DeleteCellComponent,
    EditCellComponent,
    PrimaryHeaderComponent,
    QuillTextEditorComponent,
    StarCellComponent
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
      CheckboxCellComponent,
      DeleteCellComponent,
      EditCellComponent,
      StarCellComponent
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

    AutoCompleteChipListComponent,
    CheckboxCellComponent,
    DeleteCellComponent,
    PrimaryHeaderComponent,
    QuillTextEditorComponent,
    PrimaryHeaderComponent
  ]
})
export class SharedModule {}
