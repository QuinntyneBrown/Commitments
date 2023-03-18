// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject } from "@angular/core";
import { CardStore } from "@dashboard/core";
import { map,of, tap } from "rxjs";

export function createDashboardPageViewModel() {

  const cardStore = inject(CardStore);
  
  return of("dashboard-page works!").pipe(
    tap(_ => cardStore.load()),
    map(message => ({ message }))
  );
  
};


