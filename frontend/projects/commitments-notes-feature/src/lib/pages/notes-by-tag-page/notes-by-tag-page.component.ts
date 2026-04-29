import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { NotesService } from '../../data/notes.service';
import { Note } from '../../data/note';

@Component({
  selector: 'commitments-notes-by-tag-page',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './notes-by-tag-page.component.html',
})
export class NotesByTagPageComponent implements OnInit {
  private readonly _service = inject(NotesService);
  private readonly _route = inject(ActivatedRoute);
  readonly notes = signal<Note[]>([]);

  async ngOnInit(): Promise<void> {
    const slug = this._route.snapshot.paramMap.get('slug')!;
    const { notes } = await this._service.getByTagSlug(slug);
    this.notes.set(notes);
  }
}
