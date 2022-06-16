import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { User } from 'src/app/Models/Users/User';
import { FriendService } from 'src/app/Services/friend.service';
import { UserService } from 'src/app/Services/UserService.service';
import { Friend } from 'src/app/Models/Users/Friend';
import { UserLogged } from 'src/app/Models/Users/UserLogged';

@Component({
  selector: 'app-search-user',
  templateUrl: './search-user.component.html',
  styleUrls: ['./search-user.component.scss'],
})
export class SearchUserComponent implements OnInit {
  friends: User[] = [];
  loggedUser: UserLogged = {} as any;
  constructor(
    private userService: UserService,
    private friendService: FriendService,
  ) {
    this.userService.currentUser$.subscribe({
      next: (usr) => (this.loggedUser = usr),
    });
  }

  ngOnInit(): void {
    this.friendService.GetAllFriends().subscribe({
      next: (friends) => {
        this.friends = friends;
      },
    });
    this.friendService.onFriendAddComfirmReceived((friend) =>
      this.OnComplete.emit(friend)
    );
  }
  public searchText: string = '';
  public SearchUserResult?: User[];
  @Output() OnComplete = new EventEmitter<Friend>();

  locked = false;
  lockedReturn = false;
  lastSearch = '';
  onInputUserKeyChanged() {
    if (this.searchText === '') {
      this.SearchUserResult = [];
      return;
    }
    if (this.searchText === this.lastSearch) {
      return;
    }

    if (this.locked) {
      this.lockedReturn = true;
      return;
    }
    this.locked = true;
    const usersFounds = this.userService.searchUser(this.searchText);
    this.lastSearch = this.searchText;
    usersFounds
      .subscribe({
        next: (contatcs) => {
          this.SearchUserResult = contatcs.filter(
            (x) =>
              x.userName !== this.loggedUser.userName &&
              this.friends.findIndex((q) => q.id === x.id) === -1
          );
        },
      })
      .add(() => {
        this.locked = false;
        if (this.lockedReturn) {
          this.lockedReturn = false;
          this.onInputUserKeyChanged();
        }
      });
  }

  public addContact(member: User) {
    this.friendService.SendFriendRequest(member.id);
  }
}
