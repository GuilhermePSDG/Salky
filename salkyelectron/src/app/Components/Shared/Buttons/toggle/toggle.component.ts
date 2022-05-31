import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-buttons-toggle',
  templateUrl: './toggle.component.html',
  styleUrls: ['./toggle.component.scss'],
})
export class ToggleComponent implements OnInit {
  @Output() CheckedChange = new EventEmitter<boolean>();
  @Input() ChekedValue : boolean = false;

  get checked(): boolean {
    return this.ChekedValue;
  }
  set checked(value: boolean) {
    if (this.ChekedValue != value) this.CheckedChange.emit(value);
    this.ChekedValue = value;
  }

  constructor() {}


  ngOnInit(): void {}
}
