import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageRenderComponent } from './message-render.component';

describe('MessageRenderComponent', () => {
  let component: MessageRenderComponent;
  let fixture: ComponentFixture<MessageRenderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MessageRenderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MessageRenderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
