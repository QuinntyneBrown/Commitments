// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { FrequencyType } from "../frequency-type";

export type Frequency = {
  frequencyId: string;
  frequency: number;
  frequencyTypeId: string;
  isDesired: boolean;
  frequencyType: FrequencyType;
};
