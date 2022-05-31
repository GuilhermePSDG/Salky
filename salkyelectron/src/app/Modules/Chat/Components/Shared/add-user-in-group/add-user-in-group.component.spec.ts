import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserInGroupComponent } from './add-user-in-group.component';

describe('AddUserInGroupComponent', () => {
  let component: AddUserInGroupComponent;
  let fixture: ComponentFixture<AddUserInGroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddUserInGroupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUserInGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
