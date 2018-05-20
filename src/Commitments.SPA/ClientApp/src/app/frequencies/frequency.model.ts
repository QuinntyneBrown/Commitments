import { FrequencyType } from "./frequency-type.model";

export class Frequency {  
  public frequencyId: number;
  public frequency: number;
  public frequencyTypeId: number;
  public isDesired: boolean;
  public frequencyType: FrequencyType;
}
