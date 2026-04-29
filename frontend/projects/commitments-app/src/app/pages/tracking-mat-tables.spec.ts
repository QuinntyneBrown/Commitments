import { Type, Provider } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { TranslateModule } from '@ngx-translate/core';
import { of } from 'rxjs';

import { Activity } from '../models/activity';
import { Commitment } from '../models/commitment';
import { ToDo } from '../models/to-do';
import { ActivityService } from '../services/activity.service';
import { CommitmentService } from '../services/commitment.service';
import { EditActivityDialogService } from '../services/edit-activity-dialog.service';
import { EditCommitmentDialogService } from '../services/edit-commitment-dialog.service';
import { EditToDoDialogService } from '../services/edit-to-do-dialog.service';
import { ToDoService } from '../services/to-do.service';
import { ActivitiesPageComponent } from './activities/activities-page/activities-page.component';
import { CommitmentsPageComponent } from './commitments/commitments-page/commitments-page.component';
import { ToDosPageComponent } from './to-dos/to-dos-page/to-dos-page.component';

interface TrackingCase {
  name: string;
  component: Type<unknown>;
  providers: Provider[];
  expectedText: string;
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

describe('tracking mat-table pages', () => {
  const activities = [
    Object.assign(new Activity(), {
      activityId: 1,
      behaviour: { name: 'Write journal' },
      performedOn: '2026-04-29T09:00:00Z',
    }),
  ];
  const toDos = [
    Object.assign(new ToDo(), {
      toDoId: 1,
      name: 'Renew passport',
      dueOn: '2026-05-01T00:00:00Z',
      completedOn: '2026-05-02T00:00:00Z',
    }),
  ];
  const commitments = [
    Object.assign(new Commitment(), {
      commitmentId: 1,
      behaviour: {
        name: 'Run 5k',
        behaviourType: { name: 'Fitness' },
      },
    }),
  ];

  const cases: TrackingCase[] = [
    {
      name: 'ActivitiesPageComponent',
      component: ActivitiesPageComponent,
      providers: [
        {
          provide: ActivityService,
          useValue: {
            get: jest.fn(() => of(activities)),
            remove: jest.fn(() => of(void 0)),
          },
        },
        dialogProvider(EditActivityDialogService),
      ],
      expectedText: 'Write journal',
    },
    {
      name: 'ToDosPageComponent',
      component: ToDosPageComponent,
      providers: [
        {
          provide: ToDoService,
          useValue: {
            get: jest.fn(() => of(toDos)),
            remove: jest.fn(() => of(void 0)),
          },
        },
        dialogProvider(EditToDoDialogService),
      ],
      expectedText: 'Renew passport',
    },
    {
      name: 'CommitmentsPageComponent',
      component: CommitmentsPageComponent,
      providers: [
        {
          provide: CommitmentService,
          useValue: {
            getPersonal: jest.fn(() => of(commitments)),
            remove: jest.fn(() => of(void 0)),
          },
        },
        dialogProvider(EditCommitmentDialogService),
      ],
      expectedText: 'Fitness',
    },
  ];

  it.each(cases)('$name renders tracking data through app-data-table', async (testCase) => {
    const fixture = await render(testCase.component, testCase.providers);
    const host = fixture.nativeElement as HTMLElement;

    expect(host.querySelector('app-data-table')).not.toBeNull();
    expect(host.querySelector('mat-paginator')).not.toBeNull();
    expect(host.textContent).toContain(testCase.expectedText);
  });
});
