// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCommitmentComponent } from './edit-commitment.component';

describe('EditCommitmentComponent', () => {
  let component: EditCommitmentComponent;
  let fixture: ComponentFixture<EditCommitmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ EditCommitmentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditCommitmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

