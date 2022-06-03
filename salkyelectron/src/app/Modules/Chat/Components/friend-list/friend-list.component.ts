import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Friend } from 'src/app/Models/Users/Friend';
import { FriendFlag } from 'src/app/Models/Users/FriendFlag';
import { User } from 'src/app/Models/Users/User';
import { UserLogged } from 'src/app/Models/Users/UserLogged';
import { FriendService } from 'src/app/Services/friend.service';
import { StorageService } from 'src/app/Services/storage.service';

@Component({
  selector: 'app-friend-list',
  templateUrl: './friend-list.component.html',
  styleUrls: ['./friend-list.component.scss'],
})
export class FriendListComponent implements OnInit {
  @Input() ContainerStyle = '';
  @Input() friends: Friend[] = [];
  @Input() CanShowCrudButtons: boolean = true;
  @Output() onFriendClicked = new EventEmitter<Friend>();
  loggedUser: UserLogged;

  constructor(
    private storage: StorageService,
    private friendService: FriendService
  ) {
    this.loggedUser = this.storage.CurrentUser;
  }

  ngOnInit(): void {}


}
