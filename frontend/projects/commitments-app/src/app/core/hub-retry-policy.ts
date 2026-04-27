// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { IRetryPolicy, RetryContext } from '@microsoft/signalr';

const BACKOFF_MS = [0, 2_000, 5_000, 10_000, 30_000];
const STEADY_MS = 60_000;
const GIVE_UP_MS = 5 * 60_000;

export const HubRetryPolicy: IRetryPolicy = {
  nextRetryDelayInMilliseconds(ctx: RetryContext): number | null {
    if (ctx.previousRetryCount < BACKOFF_MS.length) return BACKOFF_MS[ctx.previousRetryCount];
    if (ctx.elapsedMilliseconds > GIVE_UP_MS) return null;
    return STEADY_MS;
  }
};
