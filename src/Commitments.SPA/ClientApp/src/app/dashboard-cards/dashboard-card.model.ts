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
