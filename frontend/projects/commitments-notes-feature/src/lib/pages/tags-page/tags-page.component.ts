import { Component, inject, signal, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { TagsService } from '../../data/tags.service';
import { Tag } from '../../data/note';

@Component({
  selector: 'commitments-tags-page',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './tags-page.component.html',
})
export class TagsPageComponent implements OnInit {
  private readonly _service = inject(TagsService);
  readonly tags = signal<Tag[]>([]);

  async ngOnInit(): Promise<void> {
    const { tags } = await this._service.list();
    this.tags.set(tags);
  }
}
