import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { TranslateModule } from '@ngx-translate/core';

import { Frequency } from '../../models/frequency';
import { FrequencyType } from '../../models/frequency-type';
import { FrequenciesEditorComponent } from './frequencies-editor.component';

describe('FrequenciesEditorComponent mat-table', () => {
  let fixture: ComponentFixture<FrequenciesEditorComponent>;
  let frequencies: Frequency[];
  let frequencyTypes: FrequencyType[];

  beforeEach(async () => {
    frequencies = [
      Object.assign(new Frequency(), {
        frequency: 3,
        frequencyTypeId: 1,
        isDesired: true,
      }),
    ];
    frequencyTypes = [Object.assign(new FrequencyType(), { frequencyTypeId: 1, name: 'Weekly' })];

    await TestBed.configureTestingModule({
      imports: [FrequenciesEditorComponent, TranslateModule.forRoot(), NoopAnimationsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(FrequenciesEditorComponent);
    fixture.componentRef.setInput('frequencies', frequencies);
    fixture.componentRef.setInput('frequencyTypes', frequencyTypes);
    fixture.detectChanges();
    await fixture.whenStable();
    fixture.detectChanges();
  });

  it('renders local frequency rows through app-data-table', () => {
    const host = fixture.nativeElement as HTMLElement;

    expect(host.querySelector('app-data-table')).not.toBeNull();
    expect(host.querySelector('mat-paginator')).not.toBeNull();
    expect(host.textContent).toContain('3');
    expect(host.textContent).toContain('1');
  });

  it('adds and removes rows through the mat-table delete template', () => {
    const added = Object.assign(new Frequency(), {
      frequency: 5,
      frequencyTypeId: 1,
      isDesired: false,
    });

    fixture.componentInstance.handleFrequencySave({ frequency: added });
    fixture.detectChanges();
    expect(fixture.componentInstance.rows).toContain(added);

    const host = fixture.nativeElement as HTMLElement;
    const deleteButton = host.querySelector<HTMLButtonElement>(
      'button[aria-label="Delete frequency 5"]',
    );
    deleteButton?.click();
    fixture.detectChanges();

    expect(fixture.componentInstance.rows).not.toContain(added);
  });
});
