import { Component, Input, OnInit } from '@angular/core';
import { Friend } from 'src/app/Models/Users/Friend';
import { FriendFlag } from 'src/app/Models/Users/FriendFlag';
import { FriendService } from 'src/app/Services/friend.service';
import { StorageService } from 'src/app/Services/storage.service';

@Component({
  selector: 'app-buttons-friends-denie',
  templateUrl: './denied.component.html',
  styleUrls: ['./denied.component.scss']
})
export class DeniedComponent implements OnInit {

  @Input() Friend? : Friend;
  @Input() ButtonStyle = '';

  constructor(
    private friendService: FriendService,
    private storage : StorageService
  ) { }


  ngOnInit(): void {
  }
  canRejectFriendRequest(friend: Friend) {
    return (
      friend.friendFlag === FriendFlag.Pending && !friend.requestByCurrentUser
    );
  }
  RejectFriendRequest(friend: Friend) {
    this.friendService.RejectFriendRequest(friend.id);
  }

}
