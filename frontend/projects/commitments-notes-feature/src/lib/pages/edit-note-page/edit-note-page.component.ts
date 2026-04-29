import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute } from '@angular/router';
import { NotesService } from '../../data/notes.service';
import { Note } from '../../data/note';

@Component({
  selector: 'commitments-edit-note-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './edit-note-page.component.html',
})
export class EditNotePageComponent implements OnInit {
  private readonly _service = inject(NotesService);
  private readonly _route = inject(ActivatedRoute);
  readonly note = signal<Note | null>(null);
  readonly form = new FormGroup({
    title: new FormControl<string>(''),
    body: new FormControl<string>(''),
  });

  async ngOnInit(): Promise<void> {
    const resolved = this._route.snapshot.data['note'] as Note | undefined;
    if (resolved) {
      this.note.set(resolved);
      this.form.patchValue({ title: resolved.title, body: resolved.body });
    }
  }

  async save(): Promise<void> {
    const existing = this.note();
    await this._service.save({ note: { ...existing, ...this.form.value } });
  }
}
