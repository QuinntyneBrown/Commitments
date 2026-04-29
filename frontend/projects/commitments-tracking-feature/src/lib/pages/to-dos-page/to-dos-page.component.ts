import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToDoService } from '../../data/to-do.service';
import { ToDo } from '../../data/to-do';

@Component({
  selector: 'commitments-to-dos-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './to-dos-page.component.html',
})
export class ToDosPageComponent implements OnInit {
  private readonly _service = inject(ToDoService);
  readonly toDos = signal<ToDo[]>([]);

  async ngOnInit(): Promise<void> {
    const { toDos } = await this._service.list();
    this.toDos.set(toDos);
  }
}
