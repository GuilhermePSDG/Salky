import { Component, Input, OnInit } from '@angular/core';
import { Friend } from 'src/app/Models/Users/Friend';
import { FriendFlag } from 'src/app/Models/Users/FriendFlag';
import { FriendService } from 'src/app/Services/friend.service';
import { StorageService } from 'src/app/Services/storage.service';

@Component({
  selector: 'app-buttons-friends-accept',
  templateUrl: './accept.component.html',
  styleUrls: ['./accept.component.scss']
})
export class AcceptComponent implements OnInit {

  @Input() Friend? : Friend;
  @Input() ButtonStyle = '';
  constructor(
    private friendService: FriendService,
    private storage : StorageService
  ) { }

  ngOnInit(

  ): void {
  }

  canAcceptFriendRequest(friend: Friend) {
    return (
      friend.friendFlag === FriendFlag.Pending && !friend.requestByCurrentUser
    );
  }

  AcceptFriendRequest(friend: Friend) {
    this.friendService.AcceptFriendRequest(friend.id);
  }

}
