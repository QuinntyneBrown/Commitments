import { BehaviourType } from "./behaviour-type.model";

export class Behaviour {
  public behaviourId: number;
  public name: string;
  public isDesired: boolean;
  public description: string;
  public behaviourTypeId: number;
  public behaviourType: BehaviourType;
}
