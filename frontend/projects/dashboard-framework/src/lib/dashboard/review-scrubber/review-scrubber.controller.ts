// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Inject, Injectable, InjectionToken, Optional, computed, signal } from '@angular/core';

import { DashboardModeService } from '../dashboard-mode.service';

import { clampIndex, formatFullDate, indexOfDate, isoDateAtIndex, tickLabels } from './review-scrubber.helpers';

const DEFAULT_WINDOW_DAYS = 90;

export const REVIEW_INITIAL_WINDOW_DAYS = new InjectionToken<number>(
  'REVIEW_INITIAL_WINDOW_DAYS',
  { factory: () => DEFAULT_WINDOW_DAYS }
);
const DEFAULT_DEBOUNCE_MS = 100;
const PLAY_INTERVAL_MS = 1000;

function todayIsoDate(): string {
  return new Date().toISOString().slice(0, 10);
}

function startOfWindow(end: string, windowDays: number): string {
  return isoDateAtIndex(end, -(windowDays - 1));
}

@Injectable()
export class ReviewScrubberController {
  private _scrubTimer: ReturnType<typeof setTimeout> | null = null;
  private _playInterval: ReturnType<typeof setInterval> | null = null;

  readonly windowDays = signal(DEFAULT_WINDOW_DAYS);
  readonly windowEnd = signal(todayIsoDate());
  readonly windowStart = computed(() => startOfWindow(this.windowEnd(), this.windowDays()));
  readonly isPlaying = signal(false);

  readonly selectedDate = computed(() =>
    this._modeService.selectedReviewDate() ?? this.windowEnd()
  );

  readonly selectedDateLabel = computed(() => formatFullDate(this.selectedDate()));

  readonly selectedIndex = computed(() => {
    const idx = indexOfDate(this.windowStart(), this.selectedDate());
    return clampIndex(idx, this.windowDays() - 1);
  });

  readonly tickLabels = computed(() =>
    tickLabels(this.windowStart(), this.windowDays(), 600)
  );

  constructor(
    private readonly _modeService: DashboardModeService,
    @Optional() @Inject(REVIEW_INITIAL_WINDOW_DAYS) initialWindowDays: number | null = DEFAULT_WINDOW_DAYS
  ) {
    this.windowDays.set(initialWindowDays ?? DEFAULT_WINDOW_DAYS);
  }

  prev(): void {
    this.scrubTo(this.selectedIndex() - 1, 0);
  }

  next(): void {
    this.scrubTo(this.selectedIndex() + 1, 0);
  }

  jumpToStart(): void {
    this.scrubTo(0, 0);
  }

  jumpToToday(): void {
    this.scrubTo(this.windowDays() - 1, 0);
  }

  scrubTo(index: number, debounceMs: number = DEFAULT_DEBOUNCE_MS): void {
    const clamped = clampIndex(index, this.windowDays() - 1);
    const date = isoDateAtIndex(this.windowStart(), clamped);
    if (this._scrubTimer !== null) clearTimeout(this._scrubTimer);
    if (debounceMs <= 0) {
      this._modeService.setSelectedReviewDate(date);
      return;
    }
    this._scrubTimer = setTimeout(() => this._modeService.setSelectedReviewDate(date), debounceMs);
  }

  playToggle(): void {
    if (this.isPlaying()) {
      this.stop();
      return;
    }
    this.isPlaying.set(true);
    this._playInterval = setInterval(() => {
      if (this.selectedIndex() >= this.windowDays() - 1) {
        this.stop();
        return;
      }
      this.next();
    }, PLAY_INTERVAL_MS);
  }

  stop(): void {
    if (this._playInterval !== null) {
      clearInterval(this._playInterval);
      this._playInterval = null;
    }
    this.isPlaying.set(false);
  }

  ngOnDestroy(): void {
    this.stop();
    if (this._scrubTimer !== null) clearTimeout(this._scrubTimer);
  }
}
