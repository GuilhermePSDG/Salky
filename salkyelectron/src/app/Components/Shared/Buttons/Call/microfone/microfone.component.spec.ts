import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MicrofoneComponent } from './microfone.component';

describe('MicrofoneComponent', () => {
  let component: MicrofoneComponent;
  let fixture: ComponentFixture<MicrofoneComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MicrofoneComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MicrofoneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
