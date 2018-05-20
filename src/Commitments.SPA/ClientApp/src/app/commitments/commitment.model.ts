import { Frequency } from "../frequencies/frequency.model";

export class Commitment {
  public commitmentId: number;
  public behaviourId: number;
  public profileId: number;
  public frequencies: Array<Frequency> = [];
}
