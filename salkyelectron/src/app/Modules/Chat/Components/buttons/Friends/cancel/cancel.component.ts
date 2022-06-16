import { Component, Input, OnInit } from '@angular/core';
import { Friend } from 'src/app/Models/Users/Friend';
import { FriendFlag } from 'src/app/Models/Users/FriendFlag';
import { FriendService } from 'src/app/Services/friend.service';
import { StorageService } from 'src/app/Services/storage.service';

@Component({
  selector: 'app-buttons-friends-cancel',
  templateUrl: './cancel.component.html',
  styleUrls: ['./cancel.component.scss']
})
export class CancelComponent implements OnInit {


  @Input() ButtonStyle = '';
  @Input() Friend? : Friend;

  constructor(
    private friendService: FriendService,
    private storage : StorageService
  ) { }

  ngOnInit(): void {
  }


  canCancelFriendRequest(friend: Friend) {
    return (
      friend.friendFlag === FriendFlag.Pending && friend.requestByCurrentUser
    );
  }


  CancelFriendRequest(friend: Friend) {
    this.friendService.CancelFriendRequest(friend.id);
  }

}
