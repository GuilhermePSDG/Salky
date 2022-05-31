import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  trigger,
  style,
  animate,
  transition,
  AUTO_STYLE,
} from '@angular/animations';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
  animations: [
    trigger('overlay', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('200ms', style({ opacity: 0.6 })),
      ]),
      transition(':leave', [animate('200ms', style({ opacity: 0 }))]),
    ]),

    trigger('modal', [
      transition(':enter', [
        style({
          transform: 'translate(-50%,-50%) scale(0)',
        } ),
        animate('50ms', style({ transform: 'translate(-50%, -50%)' })),
      ]),

      transition(':leave', [
        animate('50ms', style({ transform: 'translate(-50%,-50%) scale(0.0001)' })),
      ]),
    ]),
  ],
})
export class ModalComponent implements OnInit {
  @Input() Enabled = false;
  @Input() styles = '';
  constructor() {}

  ngOnInit(): void {}
  changeState() {
    this.Enabled ? this.hide() : this.show();
  }
  hide() {
    this.Enabled = false;
  }

  show() {
    this.Enabled = true;
  }
}
