import { Component, inject, signal, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { NotesService } from '../../data/notes.service';
import { Note } from '../../data/note';

@Component({
  selector: 'commitments-edit-note-page',
  standalone: true,
  imports: [ReactiveFormsModule, MatButtonModule, MatFormFieldModule, MatInputModule],
  templateUrl: './edit-note-page.component.html',
})
export class EditNotePageComponent implements OnInit {
  private readonly _service = inject(NotesService);
  private readonly _route = inject(ActivatedRoute);
  private readonly _router = inject(Router);

  readonly titleControl = new FormControl<string>('');
  readonly note = signal<Note | null>(null);

  ngOnInit(): void {
    const note = this._route.snapshot.data['note'] as Note | undefined;
    if (note) {
      this.note.set(note);
      this.titleControl.setValue(note.title);
    }
  }

  async save(): Promise<void> {
    const existing = this.note();
    await this._service.save({ note: { ...existing, title: this.titleControl.value ?? '' } });
    this._router.navigate(['/notes']);
  }
}
