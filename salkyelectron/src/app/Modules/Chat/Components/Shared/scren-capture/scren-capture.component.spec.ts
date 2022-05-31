import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScrenCaptureComponent } from './scren-capture.component';

describe('ScrenCaptureComponent', () => {
  let component: ScrenCaptureComponent;
  let fixture: ComponentFixture<ScrenCaptureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScrenCaptureComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScrenCaptureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
