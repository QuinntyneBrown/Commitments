import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { NotesService } from '../../data/notes.service';
import { Note } from '../../data/note';

@Component({
  selector: 'commitments-notes-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './notes-page.component.html',
})
export class NotesPageComponent implements OnInit {
  private readonly _service = inject(NotesService);
  readonly notes = signal<Note[]>([]);

  async ngOnInit(): Promise<void> {
    const { notes } = await this._service.list();
    this.notes.set(notes);
  }
}
