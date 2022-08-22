import { Component, Input, OnInit } from '@angular/core';
import { CallService } from 'src/app/Services/call.service';

@Component({
  selector: 'app-buttons-call-entrar',
  templateUrl: './entrar.component.html',
  styleUrls: ['./entrar.component.scss']
})
export class EntrarComponent implements OnInit {

  @Input() GroupId? : string;
  constructor(public callService : CallService) { }

  ngOnInit(): void {
  }

}
