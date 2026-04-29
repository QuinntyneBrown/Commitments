import { Type, Provider } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { TranslateModule } from '@ngx-translate/core';
import { of } from 'rxjs';

import { BehaviourType } from '../models/behaviour-type';
import { Behaviour } from '../models/behaviour';
import { CardLayout } from '../models/card-layout';
import { Card } from '../models/card';
import { Frequency } from '../models/frequency';
import { Profile } from '../models/profile';
import { BehaviourTypeService } from '../services/behaviour-type.service';
import { BehaviourService } from '../services/behaviour.service';
import { CardLayoutService } from '../services/card-layout.service';
import { CardService } from '../services/card.service';
import { CreateProfileDialogService } from '../services/create-profile-dialog.service';
import { EditBehaviourDialogService } from '../services/edit-behaviour-dialog.service';
import { EditBehaviourTypeDialogService } from '../services/edit-behaviour-type-dialog.service';
import { EditCardDialogService } from '../services/edit-card-dialog.service';
import { EditCardLayoutDialogService } from '../services/edit-card-layout-dialog.service';
import { EditFrequencyDialogService } from '../services/edit-frequency-dialog.service';
import { FrequencyService } from '../services/frequency.service';
import { ProfileService } from '../services/profile.service';
import { BehaviourTypesPageComponent } from './behaviour-types/behaviour-types-page/behaviour-types-page.component';
import { BehavioursPageComponent } from './behaviours/behaviours-page/behaviours-page.component';
import { CardLayoutsPageComponent } from './card-layouts/card-layouts-page/card-layouts-page.component';
import { CardsPageComponent } from './cards/cards-page/cards-page.component';
import { FrequenciesPageComponent } from './frequencies/frequencies-page/frequencies-page.component';
import { ProfilesPageComponent } from './profiles/profiles-page/profiles-page.component';

interface CatalogCase {
  name: string;
  component: Type<unknown>;
  providers: Provider[];
  firstRow: string;
  sixthRow: string;
}

function names(prefix: string): string[] {
  return Array.from({ length: 6 }, (_value, index) => `${prefix} ${index + 1}`);
}

function collectionProvider(token: Type<unknown>, rows: unknown[]): Provider {
  return {
    provide: token,
    useValue: {
      get: jest.fn(() => of(rows)),
      remove: jest.fn(() => of(void 0)),
    },
  };
}

function dialogProvider(token: Type<unknown>): Provider {
  return {
    provide: token,
    useValue: {
      create: jest.fn(() => of(undefined)),
    },
  };
}

async function render(
  component: Type<unknown>,
  providers: Provider[],
): Promise<ComponentFixture<unknown>> {
  await TestBed.configureTestingModule({
    imports: [component, TranslateModule.forRoot(), NoopAnimationsModule],
    providers,
  }).compileComponents();

  const fixture = TestBed.createComponent(component);
  fixture.detectChanges();
  await fixture.whenStable();
  fixture.detectChanges();
  return fixture;
}

describe('simple catalog mat-table pages', () => {
  const profiles = names('Profile').map((name, index) =>
    Object.assign(new Profile(), { profileId: index + 1, name }),
  );
  const behaviourTypes = names('Behaviour type').map((name, index) =>
    Object.assign(new BehaviourType(), { behaviourTypeId: index + 1, name }),
  );
  const frequencies = names('Frequency').map((name, index) =>
    Object.assign(new Frequency(), {
      frequencyId: index + 1,
      frequency: index + 1,
      frequencyTypeId: 1,
      isDesired: true,
      name,
    }),
  );
  const behaviours = names('Behaviour').map((name, index) =>
    Object.assign(new Behaviour(), { behaviourId: index + 1, name }),
  );
  const cards = names('Card').map((name, index) =>
    Object.assign(new Card(), { cardId: index + 1, name }),
  );
  const cardLayouts = names('Card layout').map((name, index) =>
    Object.assign(new CardLayout(), { cardLayoutId: index + 1, name }),
  );

  const cases: CatalogCase[] = [
    {
      name: 'ProfilesPageComponent',
      component: ProfilesPageComponent,
      providers: [
        collectionProvider(ProfileService, profiles),
        dialogProvider(CreateProfileDialogService),
      ],
      firstRow: 'Profile 1',
      sixthRow: 'Profile 6',
    },
    {
      name: 'BehaviourTypesPageComponent',
      component: BehaviourTypesPageComponent,
      providers: [
        collectionProvider(BehaviourTypeService, behaviourTypes),
        dialogProvider(EditBehaviourTypeDialogService),
      ],
      firstRow: 'Behaviour type 1',
      sixthRow: 'Behaviour type 6',
    },
    {
      name: 'FrequenciesPageComponent',
      component: FrequenciesPageComponent,
      providers: [
        collectionProvider(FrequencyService, frequencies),
        dialogProvider(EditFrequencyDialogService),
      ],
      firstRow: 'Frequency 1',
      sixthRow: 'Frequency 6',
    },
    {
      name: 'BehavioursPageComponent',
      component: BehavioursPageComponent,
      providers: [
        collectionProvider(BehaviourService, behaviours),
        dialogProvider(EditBehaviourDialogService),
      ],
      firstRow: 'Behaviour 1',
      sixthRow: 'Behaviour 6',
    },
    {
      name: 'CardsPageComponent',
      component: CardsPageComponent,
      providers: [collectionProvider(CardService, cards), dialogProvider(EditCardDialogService)],
      firstRow: 'Card 1',
      sixthRow: 'Card 6',
    },
    {
      name: 'CardLayoutsPageComponent',
      component: CardLayoutsPageComponent,
      providers: [
        collectionProvider(CardLayoutService, cardLayouts),
        dialogProvider(EditCardLayoutDialogService),
      ],
      firstRow: 'Card layout 1',
      sixthRow: 'Card layout 6',
    },
  ];

  it.each(cases)('$name renders 5 rows and a paginator', async (testCase) => {
    const fixture = await render(testCase.component, testCase.providers);
    const host = fixture.nativeElement as HTMLElement;

    expect(host.querySelector('app-data-table')).not.toBeNull();
    expect(host.querySelector('mat-paginator')).not.toBeNull();
    expect(host.querySelectorAll('tbody tr')).toHaveLength(5);
    expect(host.textContent).toContain(testCase.firstRow);
    expect(host.textContent).not.toContain(testCase.sixthRow);
  });
});
