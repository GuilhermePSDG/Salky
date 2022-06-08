import { animate, style, transition, trigger } from '@angular/animations';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Group } from 'src/app/Models/GroupModels/Group';
import { GroupMember } from 'src/app/Models/Users/UserGroup';
import { GroupService } from 'src/app/Services/Group.service';
import { StorageService } from 'src/app/Services/storage.service';
import { UserService } from 'src/app/Services/UserService.service';

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