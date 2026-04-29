import { Routes } from '@angular/router';
import { FrequenciesPageComponent } from './pages/frequencies-page/frequencies-page.component';
import { EditFrequencyPageComponent } from './pages/edit-frequency-page/edit-frequency-page.component';

export const frequenciesRoutes: Routes = [
  { path: 'frequencies',                   component: FrequenciesPageComponent },
  { path: 'edit-frequency',                component: EditFrequencyPageComponent },
  { path: 'edit-frequency/:frequencyId',   component: EditFrequencyPageComponent },
];
