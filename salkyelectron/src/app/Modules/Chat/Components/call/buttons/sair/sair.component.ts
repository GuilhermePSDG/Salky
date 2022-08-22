import { Component, OnInit } from '@angular/core';
import { CallService } from 'src/app/Services/call.service';

@Component({
  selector: 'app-buttons-call-leave',
  templateUrl: './sair.component.html',
  styleUrls: ['./sair.component.scss']
})
export class SairComponent implements OnInit {

  constructor(public callService : CallService) { }

  ngOnInit(): void {
  }

}
