import { signal } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { of } from 'rxjs';

import { Store } from '../../core/store';
import { Note } from '../../models/note';
import { NotesService } from '../../services/notes.service';
import { NotesPageComponent } from './notes-page/notes-page.component';

describe('NotesPageComponent mat-table', () => {
  let notes: Array<Note & { slug: string }>;
  let notesSignal: ReturnType<typeof signal<Note[]>>;
  let fixture: ComponentFixture<NotesPageComponent>;
  let notesService: {
    get: jest.Mock;
    remove: jest.Mock;
  };

  beforeEach(async () => {
    notes = [
      Object.assign(new Note(), {
        noteId: 'note-1',
        title: 'Planning Note',
        slug: 'planning-note',
      }),
    ];
    notesSignal = signal<Note[]>(notes);
    notesService = {
      get: jest.fn(() => of({ notes })),
      remove: jest.fn(() => of(void 0)),
    };

    await TestBed.configureTestingModule({
      imports: [NotesPageComponent, TranslateModule.forRoot(), NoopAnimationsModule],
      providers: [
        provideRouter([]),
        { provide: Store, useValue: { notes: notesSignal } },
        { provide: NotesService, useValue: notesService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(NotesPageComponent);
    fixture.detectChanges();
    await fixture.whenStable();
    fixture.detectChanges();
  });

  it('renders note titles as edit-note router links through app-data-table', () => {
    const host = fixture.nativeElement as HTMLElement;
    const link = host.querySelector<HTMLAnchorElement>('a.note-title-link');

    expect(host.querySelector('app-data-table')).not.toBeNull();
    expect(link?.textContent).toContain('Planning Note');
    expect(link?.getAttribute('href')).toContain('/edit-note/planning-note');
  });

  it('removes notes through the delete cell template', () => {
    const host = fixture.nativeElement as HTMLElement;
    const button = host.querySelector<HTMLButtonElement>(
      'button[aria-label="Delete Planning Note"]',
    );

    button?.click();
    fixture.detectChanges();

    expect(notesService.remove).toHaveBeenCalledWith({ note: notes[0] });
    expect(notesSignal()).toEqual([]);
  });
});
