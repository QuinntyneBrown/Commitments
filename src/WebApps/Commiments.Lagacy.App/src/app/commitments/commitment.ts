// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { CommitmentFrequency } from "./commitment-frequency";

export class Commitment {
  public commitmentId: number;
  public behaviourId: number;
  public profileId: number;
  public commitmentFrequencies: Array<CommitmentFrequency> = [];
}

