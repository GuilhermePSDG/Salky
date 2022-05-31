import { Component, OnInit } from '@angular/core';
import { AudioService } from 'src/app/Services/AudioService';
import { CallService } from 'src/app/Services/call.service';

@Component({
  selector: 'app-buttons-call-headset',
  templateUrl: './head-set.component.html',
  styleUrls: ['./head-set.component.scss']
})
export class HeadSetComponent implements OnInit {

  constructor(public callService: CallService) {}


  ngOnInit(): void {

  }

}
