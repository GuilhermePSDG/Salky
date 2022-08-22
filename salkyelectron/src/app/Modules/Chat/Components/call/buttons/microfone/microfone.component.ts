import { Component, OnInit } from '@angular/core';
import { AudioService } from 'src/app/Services/AudioService';
import { CallService } from 'src/app/Services/call.service';

@Component({
  selector: 'app-buttons-call-microfone',
  templateUrl: './microfone.component.html',
  styleUrls: ['./microfone.component.scss']
})
export class MicrofoneComponent implements OnInit {

  constructor(public callService: CallService) {}

  ngOnInit(): void {

  }

}
