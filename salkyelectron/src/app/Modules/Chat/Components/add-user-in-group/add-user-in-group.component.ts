import { Component, Input, OnInit } from '@angular/core';
import { Friend } from 'src/app/Models/Users/Friend';
import { FriendFlag } from 'src/app/Models/Users/FriendFlag';
import { User } from 'src/app/Models/Users/User';
import { GroupMember } from 'src/app/Models/Users/UserGroup';
import { UserLogged } from 'src/app/Models/Users/UserLogged';
import { FriendService } from 'src/app/Services/friend.service';
import { GroupService } from 'src/app/Services/Group.service';
import { GroupMemberService } from 'src/app/Services/GroupMember.service';
import { StorageService } from 'src/app/Services/storage.service';
import { UserService } from 'src/app/Services/UserService.service';

@Component({
  selector: 'app-add-user-in-group',
  templateUrl: './add-user-in-group.component.html',
  styleUrls: ['./add-user-in-group.component.scss'],
})
export class AddUserInGroupComponent implements OnInit {
  friends: Friend[] = [];
  @Input() GroupId?: string;
  @Input() GroupMembers: GroupMember[] = [];

  currentUser: UserLogged = {} as any;
  constructor(
    public friendService: FriendService,
    public groupService: GroupService,
    private storage: StorageService,
    userService: UserService,
    private groupMemberSer: GroupMemberService
  ) {
    userService.currentUser$.subscribe({
      next: (usr) => {
        if (usr) {
          this.currentUser = usr;
        }
      },
    });
  }

  ngOnInit(): void {
    this.onClickOutElement('.mainAddUserInGroup', () => this.hide());
    this.initFriends();
  }

  ngAfterViewInit(): void {}

  
  public addFriendInGroup(friend: Friend) {
    console.log('addFriendInGroup(friend : Friend)');
    if (!this.GroupId) return;
    this.groupMemberSer.addFriendInGroup(friend.id, this.GroupId);
    this.hide();
  }

  onClickOutElement(selector: string, onClickedOut: () => void) {
    var element = document.querySelector(selector) as any;
    document.addEventListener('click', function (event: any) {
      if (!element.contains(event.target)) {
        onClickedOut();
      }
    });
  }

  public enabled: boolean = false;

  public hide() {
    this.enabled = false;
  }

  public changeState() {
    this.enabled = !this.enabled;
    if (this.enabled) {
      this.initFriends();
    }
  }
  private initFriends() {
    this.friends = [];
    this.friendService.GetAllFriends().subscribe({
      next: (friends) => {
        this.friends = friends.filter(
          (x) => x.friendFlag === FriendFlag.Approved &&
          this.GroupMembers.findIndex(q => q.userId === x.userId) === -1
        );
      },
    });
  }
}
