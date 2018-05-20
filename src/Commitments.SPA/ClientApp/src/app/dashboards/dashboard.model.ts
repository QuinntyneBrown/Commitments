import { DashboardCard } from "../dashboard-cards/dashboard-card.model";

export class Dashboard {
  public dashboardId: number;
  public name: string;
  public dashboardCards: DashboardCard[] = [];
}
