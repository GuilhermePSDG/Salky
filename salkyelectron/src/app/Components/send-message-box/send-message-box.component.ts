import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ContactVM } from 'src/app/ModelsView/User/ContactVM';
import { MessageVM } from 'src/app/ModelsView/MessageVM';

@Component({
  selector: 'app-send-message-box',
  templateUrl: './send-message-box.component.html',
  styleUrls: ['./send-message-box.component.scss'],
})
export class SendMessageBoxComponent implements OnInit {
  public inputText: string = '';
  @Input() Validator: (x: string) => boolean = (x) =>
    x != null && x != undefined && x.length > 0;

  constructor() {}
  ngOnInit(): void {}
  @Output() onSendMessageRequested: EventEmitter<string> = new EventEmitter<string>();

  public onKeyPres(x: KeyboardEvent): void {
    if (x.key === 'Enter') {
      this.sendMessage();
    }
  }

  public sendMessage(): void {
    if (this.Validator(this.inputText)) {
      this.onSendMessageRequested.emit(this.inputText);
      this.inputText = '';
    }
  }
}
