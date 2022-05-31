import { Component, Input, OnInit } from '@angular/core';
import { Friend } from 'src/app/Models/Users/Friend';
import { FriendFlag } from 'src/app/Models/Users/FriendFlag';
import { FriendService } from 'src/app/Services/friend.service';
import { StorageService } from 'src/app/Services/storage.service';


@Component({
  selector: 'app-buttons-friends-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.scss']
})
export class DeleteComponent implements OnInit {

  @Input() ButtonStyle = '';
  @Input() Friend? : Friend;

  constructor(
    private friendService: FriendService,
    private storage : StorageService
  ) { }

  ngOnInit(): void {
  }

  canDeleteFriend(friend: Friend) {
    return (
      friend.friendFlag === FriendFlag.Approved
    );
  }

  deleteFriend(friend: Friend) {
    this.friendService.removeFriend(friend.id);
  }

}
