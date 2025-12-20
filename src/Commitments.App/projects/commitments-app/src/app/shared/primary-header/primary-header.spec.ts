// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { TestBed, ComponentFixture } from '@angular/core/testing';
import { PrimaryHeader } from './primary-header';

describe('PrimaryHeader', () => {
  let component: PrimaryHeader;
  let fixture: ComponentFixture<PrimaryHeader>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PrimaryHeader]
    }).compileComponents();

    fixture = TestBed.createComponent(PrimaryHeader);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render content projection', () => {
    const testContent = 'Test Header Content';
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('h2')).toBeTruthy();
  });

  it('should clean up on destroy', () => {
    const onDestroySpy = jest.spyOn(component.onDestroy, 'next');
    component.ngOnDestroy();
    expect(onDestroySpy).toHaveBeenCalled();
  });
});
