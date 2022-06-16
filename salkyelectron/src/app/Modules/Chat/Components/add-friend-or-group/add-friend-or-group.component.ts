import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-add-friend-or-group',
  templateUrl: './add-friend-or-group.component.html',
  styleUrls: ['./add-friend-or-group.component.scss'],
  animations: [
  ],
})
export class AddGroupComponent implements OnInit {
  constructor() {}
  public mode : 'CreateGroup' | 'SearchUsers' = 'SearchUsers';
  ngOnInit(): void {}

}
