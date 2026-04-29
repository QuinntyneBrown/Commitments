import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TagsService } from '../../data/tags.service';
import { Tag } from '../../data/tag';

@Component({
  selector: 'commitments-tags-page',
  standalone: true,
  imports: [CommonModule],
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
