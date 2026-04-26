import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'commitments-tile-shell',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <article class="tile-shell" data-testid="tile-shell">
      <header class="tile-shell__header">
        <div>
          @if (eyebrow) {
            <p class="tile-shell__eyebrow">{{ eyebrow }}</p>
          }
          <h2 class="tile-shell__title" data-testid="tile-title">{{ title }}</h2>
        </div>
        @if (status) {
          <span class="tile-shell__status">{{ status }}</span>
        }
      </header>

      <section class="tile-shell__body">
        <ng-content></ng-content>
      </section>
    </article>
  `,
  styles: [
    `
      :host {
        display: block;
        height: 100%;
        min-height: 0;
      }

      .tile-shell {
        display: flex;
        flex-direction: column;
        height: 100%;
        min-height: 0;
        color: #172033;
        background: #ffffff;
      }

      .tile-shell__header {
        display: flex;
        align-items: flex-start;
        justify-content: space-between;
        gap: 12px;
        padding-bottom: 12px;
        border-bottom: 1px solid #e1e7f0;
      }

      .tile-shell__eyebrow {
        margin: 0 0 4px;
        color: #5b6b84;
        font-size: 11px;
        font-weight: 700;
        text-transform: uppercase;
      }

      .tile-shell__title {
        margin: 0;
        color: #172033;
        font-size: 18px;
        font-weight: 700;
        line-height: 1.2;
      }

      .tile-shell__status {
        flex: 0 0 auto;
        padding: 4px 8px;
        border: 1px solid #cfe0f7;
        border-radius: 999px;
        color: #245d9f;
        background: #edf5ff;
        font-size: 11px;
        font-weight: 700;
      }

      .tile-shell__body {
        flex: 1 1 auto;
        min-height: 0;
        padding-top: 14px;
      }
    `
  ]
})
export class TileShellComponent {
  @Input() title = '';
  @Input() eyebrow = '';
  @Input() status = '';
}
