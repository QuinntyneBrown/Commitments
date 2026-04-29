import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { ToDo } from './to-do';

@Injectable({ providedIn: 'root' })
export class ToDoService {
  private readonly _backend = inject(DashboardBackendService);
  list(): Promise<{ toDos: ToDo[] }>                   { return this._backend.get('api/v1.0/toDos'); }
  create(input: Partial<ToDo>): Promise<{ toDo: ToDo }> { return this._backend.post('api/v1.0/toDos', input); }
  remove(id: number | string): Promise<void>            { return this._backend.delete(`api/v1.0/toDos/${id}`); }
}
