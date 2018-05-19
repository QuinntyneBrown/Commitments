import { CommitmentFrequency } from "./commitment-frequency.model";

export class Commitment {
  public commitmentId: number;
  public behaviourId: number;
  public profileId: number;
  public commitmentFrequencies: Array<CommitmentFrequency> = [];
}
