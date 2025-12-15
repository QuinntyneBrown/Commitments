// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

export class Options {
  public top: number;
  public left: number;
  public height: number;
  public width: number;
}

export class DashboardCard {
  public dashboardCardId: number;
  public name: string;
  public cardId: number;
  public dashboardId: number;
  public options: Options = new Options();
}

