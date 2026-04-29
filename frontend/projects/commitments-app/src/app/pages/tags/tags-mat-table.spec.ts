import { signal } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OverlayModule } from '@angular/cdk/overlay';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { TranslateModule } from '@ngx-translate/core';
import { of } from 'rxjs';

import { Store } from '../../core/store';
import { Tag } from '../../models/tag';
import { TagsService } from '../../services/tags.service';
import { TagsPageComponent } from './tags-page/tags-page.component';

describe('TagsPageComponent mat-table', () => {
  let tags: Tag[];
  let fixture: ComponentFixture<TagsPageComponent>;
  let tagsService: {
    get: jest.Mock;
    save: jest.Mock;
    remove: jest.Mock;
  };

  beforeEach(async () => {
    tags = [
      Object.assign(new Tag(), {
        tagId: 'tag-1',
        name: 'Planning',
        slug: 'planning',
      }),
    ];

    tagsService = {
      get: jest.fn(() => of({ tags })),
      save: jest.fn(() => of({ tag: tags[0] })),
      remove: jest.fn(() => of({ tags: [] })),
    };

    await TestBed.configureTestingModule({
      imports: [TagsPageComponent, TranslateModule.forRoot(), NoopAnimationsModule, OverlayModule],
      providers: [
        { provide: Store, useValue: { tags: signal(tags) } },
        { provide: TagsService, useValue: tagsService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(TagsPageComponent);
    fixture.detectChanges();
    await fixture.whenStable();
    fixture.detectChanges();
  });

  it('renders tags through app-data-table', () => {
    const host = fixture.nativeElement as HTMLElement;
    const input = host.querySelector('input') as HTMLInputElement;

    expect(host.querySelector('app-data-table')).not.toBeNull();
    expect(host.querySelector('mat-paginator')).not.toBeNull();
    expect(input.value).toBe('Planning');
  });

  it('saves the mutated tag when the name input blurs', () => {
    const host = fixture.nativeElement as HTMLElement;
    const input = host.querySelector('input') as HTMLInputElement;

    input.value = 'Planning Updated';
    input.dispatchEvent(new Event('input', { bubbles: true }));
    fixture.detectChanges();
    input.dispatchEvent(new FocusEvent('blur'));

    expect(tags[0].name).toBe('Planning Updated');
    expect(tagsService.save).toHaveBeenCalledWith({ tag: tags[0] });
  });
});
