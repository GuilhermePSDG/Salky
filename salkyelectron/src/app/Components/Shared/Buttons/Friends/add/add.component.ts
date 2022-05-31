import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-buttons-friends-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent implements OnInit {
  @Input() ButtonStyle = '';
  constructor() { }

  ngOnInit(): void {
  }

}
