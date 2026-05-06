import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '../backend';
import { Tag } from './note';

@Injectable({ providedIn: 'root' })
export class TagsService {
  private readonly _backend = inject(DashboardBackendService);
  list(): Promise<{ tags: Tag[] }>                { return this._backend.get('api/v1.0/tags'); }
  save(input: Partial<Tag>): Promise<{ tag: Tag }> { return this._backend.post('api/v1.0/tags', input); }
  remove(id: number | string): Promise<void>      { return this._backend.delete(`api/v1.0/tags/${id}`); }
}
