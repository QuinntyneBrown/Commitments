// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

// Color tokens mirror the CSS custom properties in _tokens.scss so canvas-drawn
// artefacts (e.g. Chart.js gradients) can use the same palette.

export const BG_APP = '#121212';
export const BG_TOOLBAR = '#1F2233';
export const BG_SIDEBAR = '#181A24';
export const SURFACE_TILE = '#242424';
export const SURFACE_RAISED = '#1E1E1E';
export const SURFACE_3 = '#2C2C2C';
export const SURFACE_4 = '#333333';
export const DIVIDER = '#3A3A3A';
export const DIVIDER_SOFT = '#2A2A2A';

export const ACCENT_LIVE = '#FF4081';
export const ACCENT_LIVE_STRONG = '#F50057';
export const ACCENT_REVIEW = '#9FA8DA';
export const ACCENT_REVIEW_DIM = '#3F51B5';
export const ACCENT_REVIEW_STRONG = '#7986CB';
export const ACCENT_CHART = '#42A5F5';
export const ACCENT_SUCCESS = '#66BB6A';
export const ACCENT_WARN = '#F44336';
export const ACCENT_WARNING = '#FFA726';

export const TEXT_PRIMARY = '#FFFFFF';
export const TEXT_SECONDARY = '#B0B0B0';
export const TEXT_MUTED = '#666666';
export const TEXT_ON_DARK_ACCENT = '#1A1B23';

export const RADIUS_SM = 4;
export const RADIUS_MD = 8;
export const RADIUS_TILE = 12;
export const RADIUS_XL = 16;
export const RADIUS_FULL = 9999;
export const SPACE_TILE_PAD = 20;
export const TOOLBAR_HEIGHT = 64;
export const HEADER_HEIGHT = 80;
export const SIDENAV_WIDTH = 250;

export type DashboardMode = 'live' | 'review';
