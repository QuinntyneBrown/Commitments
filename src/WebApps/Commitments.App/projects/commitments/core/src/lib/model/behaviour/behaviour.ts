// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { BehaviourType } from "../behaviour-type";

export type Behaviour = {
  behaviourId: string;
  name: string;
  isDesired: boolean;
  description: string;
  behaviourTypeId: number;
  behaviourType: BehaviourType;
};


