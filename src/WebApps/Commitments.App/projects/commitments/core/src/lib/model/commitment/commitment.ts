// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { CommitmentFrequency } from "./commitment-frequency";

export type Commitment = {
  commitmentId: string;
  behaviourId: string;
  profileId: string;
  commitmentFrequencies: Array<CommitmentFrequency>;
};


