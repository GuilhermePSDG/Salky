import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-InputBase',
  templateUrl: './InputBase.component.html',
  styleUrls: ['./InputBase.component.scss']
})
export class InputBaseComponent implements OnInit {

  constructor() { }
  @Input() hint = ""
  @Input() text = ""

  ngOnInit() {



  }

}
