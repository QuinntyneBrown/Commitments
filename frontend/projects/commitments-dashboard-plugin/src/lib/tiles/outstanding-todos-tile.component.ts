import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TileMetadata } from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

@Component({
  selector: 'commitments-outstanding-todos-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <commitments-tile-shell title="Outstanding To Dos" eyebrow="Queue" status="4 open">
      <div class="todo-count">4</div>
      <p class="todo-copy">items need attention before the next review.</p>
    </commitments-tile-shell>
  `,
  styles: [
    `
      .todo-count {
        color: #8b3f10;
        font-size: 58px;
        font-weight: 800;
        line-height: 1;
      }

      .todo-copy {
        max-width: 220px;
        margin: 10px 0 0;
        color: #5b6b84;
        font-size: 13px;
        line-height: 1.4;
      }
    `
  ]
})
export class OutstandingTodosTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.outstanding-todos',
    displayName: 'Outstanding To Dos',
    description: 'Open tasks that still need action.',
    category: 'Tasks',
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 9, y: 0 },
    includeByDefault: true
  };
}
