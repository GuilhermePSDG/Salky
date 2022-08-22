import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendMessageBoxComponent } from './send-message-input.component';

describe('SendMessageBoxComponent', () => {
  let component: SendMessageBoxComponent;
  let fixture: ComponentFixture<SendMessageBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SendMessageBoxComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SendMessageBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
