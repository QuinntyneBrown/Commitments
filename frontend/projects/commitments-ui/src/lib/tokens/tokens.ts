// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

// TS mirrors of CSS custom properties from _tokens.scss, used by canvas-drawn
// artefacts (e.g. Chart.js gradients) where CSS variables can't reach.
// Only the constants actually consumed by such code live here — the rest of
// the design system is expressed exclusively as CSS custom properties.

export const BG_APP = '#121212';
export const ACCENT_CHART = '#42A5F5';
export const TEXT_MUTED = '#666666';

export type DashboardMode = 'live' | 'review';
