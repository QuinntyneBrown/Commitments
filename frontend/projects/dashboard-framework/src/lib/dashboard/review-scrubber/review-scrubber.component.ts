// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { IconButtonComponent } from '@commitments/ui';

import { ReviewScrubberController } from './review-scrubber.controller';

@Component({
  selector: 'df-review-scrubber',
  standalone: true,
  imports: [IconButtonComponent],
  providers: [ReviewScrubberController],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './review-scrubber.component.html',
  styleUrls: ['./review-scrubber.component.scss']
})
export class ReviewScrubberComponent implements OnInit, OnDestroy {
  @Input() windowDays?: number;

  constructor(readonly controller: ReviewScrubberController) {}

  ngOnInit(): void {
    if (this.windowDays !== undefined) {
      this.controller.windowDays.set(this.windowDays);
    }
  }

  ngOnDestroy(): void {
    this.controller.ngOnDestroy();
  }

  onSliderInput(event: Event): void {
    this.controller.scrubTo(Number((event.target as HTMLInputElement).value));
  }

  onKeyDown(event: KeyboardEvent): void {
    switch (event.key) {
      case 'ArrowLeft':
        this.controller.prev();
        break;
      case 'ArrowRight':
        this.controller.next();
        break;
      case 'PageUp':
        this.controller.scrubTo(this.controller.selectedIndex() - 7, 0);
        break;
      case 'PageDown':
        this.controller.scrubTo(this.controller.selectedIndex() + 7, 0);
        break;
      case 'Home':
        this.controller.jumpToStart();
        break;
      case 'End':
        this.controller.jumpToToday();
        break;
      default:
        return;
    }
    event.preventDefault();
  }
}
