// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { DashboardCard } from "../dashboard-cards/dashboard-card";

export class Dashboard {
  public dashboardId: number;
  public name: string;
  public dashboardCards: DashboardCard[] = [];
}

