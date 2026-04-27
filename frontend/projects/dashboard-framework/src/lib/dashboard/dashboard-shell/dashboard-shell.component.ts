import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModeToggleComponent, PrimaryHeaderComponent } from '@commitments/ui';

import { AddTileDialogComponent } from '../add-tile-dialog/add-tile-dialog.component';
import { DashboardGridComponent } from '../dashboard-grid/dashboard-grid.component';
import { DashboardModeService } from '../dashboard-mode.service';
import { ReviewScrubberComponent } from '../review-scrubber/review-scrubber.component';
import { DashboardMode } from '../../tile-registration';

@Component({
  selector: 'commitments-dashboard-shell',
  standalone: true,
  imports: [DashboardGridComponent, ModeToggleComponent, PrimaryHeaderComponent, ReviewScrubberComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './dashboard-shell.component.html',
  styleUrls: ['./dashboard-shell.component.scss']
})
export class DashboardShellComponent {
  protected readonly modeService = inject(DashboardModeService);
  private readonly dialog = inject(MatDialog);

  protected onModeChange(mode: DashboardMode): void {
    this.modeService.setMode(mode);
  }

  protected openAddTileDialog(): void {
    this.dialog.open(AddTileDialogComponent, {
      panelClass: 'add-tile-dialog__panel',
      backdropClass: 'add-tile-dialog__backdrop',
      autoFocus: false
    });
  }
}
