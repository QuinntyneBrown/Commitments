// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyCommitmentsComponent } from './my-commitments.component';

describe('MyCommitmentsComponent', () => {
  let component: MyCommitmentsComponent;
  let fixture: ComponentFixture<MyCommitmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ MyCommitmentsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyCommitmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

