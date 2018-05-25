import { User } from "../users/user.model";

export class Profile {
  public profileId: number;
  public name: string;
  public user: User = new User();
}
