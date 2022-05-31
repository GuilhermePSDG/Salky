import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeadSetComponent } from './head-set.component';

describe('HeadSetComponent', () => {
  let component: HeadSetComponent;
  let fixture: ComponentFixture<HeadSetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HeadSetComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HeadSetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
