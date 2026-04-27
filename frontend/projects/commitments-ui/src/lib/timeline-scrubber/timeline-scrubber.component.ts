import { ChangeDetectionStrategy, Component, input, model, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatSliderModule } from '@angular/material/slider';

export interface CuiTimelineTick {
  label: string;
  active?: boolean;
}

@Component({
  selector: 'cui-timeline-scrubber',
  standalone: true,
  imports: [MatButtonModule, MatCardModule, MatIconModule, MatSliderModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './timeline-scrubber.component.html',
  styleUrls: ['./timeline-scrubber.component.scss']
})
export class CuiTimelineScrubberComponent {
  readonly label = input('Reviewing');
  readonly dateLabel = input('');
  readonly min = input(0);
  readonly max = input(100);
  readonly step = input(1);
  readonly disabled = input(false);
  readonly ticks = input<readonly CuiTimelineTick[]>([]);
  readonly value = model(0);
  readonly previous = output<void>();
  readonly next = output<void>();
  readonly play = output<void>();
  readonly jumpToday = output<void>();

  protected updateValue(nextValue: number): void {
    this.value.set(nextValue);
  }
}
