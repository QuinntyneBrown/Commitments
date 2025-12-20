// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { User } from "../users/user";

export class Profile {
  public profileId: number;
  public name: string;
  public user: User = new User();
  public avatarUrl: string;
}

