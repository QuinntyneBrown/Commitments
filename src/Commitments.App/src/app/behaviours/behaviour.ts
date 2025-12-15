// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { BehaviourType } from "./behaviour-type";

export class Behaviour {
  public behaviourId: number;
  public name: string;
  public isDesired: boolean;
  public description: string;
  public behaviourTypeId: number;
  public behaviourType: BehaviourType;
}

