import { InjectionToken, Type } from '@angular/core';

export const PLUGIN_TILES = new InjectionToken<ReadonlyArray<ReadonlyArray<Type<unknown>>>>('PLUGIN_TILES');
