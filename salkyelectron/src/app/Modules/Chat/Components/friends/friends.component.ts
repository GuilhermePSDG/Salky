import { Component, OnDestroy, OnInit } from '@angular/core';
import { Friend } from 'src/app/Models/Users/Friend';
import { FriendFlag } from 'src/app/Models/Users/FriendFlag';
import { User } from 'src/app/Models/Users/User';
import { UserLogged } from 'src/app/Models/Users/UserLogged';
import { FriendService } from 'src/app/Services/friend.service';
import { StorageService } from 'src/app/Services/storage.service';
import { EventsDestroyables } from 'src/app/Services/WebSocketBaseService';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.scss'],
})
export class FriendsComponent
extends EventsDestroyables
 implements OnInit,OnDestroy {
  staticFriends: Friend[] = [];
  friends: Friend[] = [];
  loggedUser: UserLogged;

  constructor(
    private storage: StorageService,
    private friendService: FriendService
  ) {
    super();
    this.loggedUser = this.storage.CurrentUser;
  }
  ngOnDestroy(): void {
    this.Destroy();
  }

  ngOnInit(): void {
    var sub1 = this.friendService.GetAllFriends().subscribe({
      next: (friends) => {
        this.staticFriends = friends;
        this.changeTo({ flag: null })
      },
    });

    var sub2 =this.friendService.onFriendAccept((friend) =>
      this.addOrUpdateFriend(friend)
    );
    var sub3 = this.friendService.onFriendChangePicture(friendIn => {
      console.log("processando mensagem")
      console.log(friendIn)
      console.log(this.friends)

      var i = this.friends.findIndex(x => x.id === friendIn.friendId);
      console.log(i)
      if (i !== -1) {
        this.friends[i].pictureSource = friendIn.pictureSource;
      }
    });
    var sub4 = this.friendService.onFriendAddComfirmReceived((friend) => {
      this.addOrUpdateFriend(friend);
    });
    var sub5 = this.friendService.onFriendAddReceived((friend) =>
      this.addOrUpdateFriend(friend)
    );
    var sub6 = this.friendService.onFriendReject((friendId) =>
      this.removeFriend(friendId)
    );
    var sub7 = this.friendService.onFriendCancel((friendId) =>
      this.removeFriend(friendId)
    );
    var sub8 = this.friendService.onFriendDelete((friendId) =>
      this.removeFriend(friendId)
    );
    this.AppendManyToDestroy(sub1,sub2,sub3,sub4,sub5,sub6,sub7,sub8);
  }

  removeFriend(friendId: string) {
    this.removeFriendByIdInArr(friendId, this.staticFriends);
    this.removeFriendByIdInArr(friendId, this.friends);
  }
  addOrUpdateFriend(friend: Friend) {
    this.addOrUpdateFriendInArr(friend, this.staticFriends);
    this.addOrUpdateFriendInArr(friend, this.friends);
  }
  addOrUpdateFriendInArr(friend: Friend, arr: Friend[]) {
    var index = arr.findIndex((x) => x.id === friend.id);
    if (index === -1) arr.push(friend);
    else arr[index] = friend;
  }
  removeFriendByIdInArr(friendId: string, arr: Friend[]) {
    var index = arr.findIndex((x) => x.id === friendId);
    if (index !== -1) arr.splice(index, 1);
  }

  flags: any[] = [
    {
      flag: null,
      text: 'Todos',
      active: true,
    },
    {
      flag: FriendFlag.Pending,
      text: 'Pendente',
      active: false,
    },
  ];


  private desactiveAllFlags() {
    this.flags.forEach((x) => (x.active = false));
  }
  public changeTo(flag: any) {
    this.desactiveAllFlags();
    flag.active = true;
    if (flag.flag)
      this.friends = this.staticFriends.filter((x) => x.friendFlag === flag.flag);
    else
      this.friends = this.staticFriends;
  }
}
