import { FrequencyType } from "./frequency-type.model";

export class CommitmentFrequency {
  public commitmentFrequencyId: number;
  public frequencyTypeId: number;
  public frequencyType: FrequencyType;
  public frequency: string;
}
