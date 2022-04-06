/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { InputBaseComponent } from './InputBase.component';

describe('InputBaseComponent', () => {
  let component: InputBaseComponent;
  let fixture: ComponentFixture<InputBaseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InputBaseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InputBaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
