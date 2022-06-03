import { Component, Input, OnInit } from '@angular/core';
import { Group } from 'src/app/Models/GroupModels/Group';
import { GroupMember } from 'src/app/Models/Users/UserGroup';
import { UserCall } from 'src/app/Models/Users/UserCall';
import { UserLogged } from 'src/app/Models/Users/UserLogged';
import { CallService } from 'src/app/Services/call.service';
import { SalkyWebSocket } from 'src/app/Services/SalykWsClient.service';
import { StorageService } from 'src/app/Services/storage.service';
import { UserService } from 'src/app/Services/UserService.service';

@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss'],
})
export class CallComponent implements OnInit {
  public callMembers: UserCall[] = [];
  @Input() GroupMembers: GroupMember[] = [];
  @Input() CurrentMember?: GroupMember;

  public get hasMembersInCall(): boolean {
    return this.callMembers.length > 0;
  }
  public get loggedUserIsInCall(): boolean {
    return (
      this.callMembers.findIndex(
        (x) => x.isInCall && x.memberId === this.CurrentMember?.id
      ) !== -1
    );
  }
  groupId?: string;
  @Input() set GroupId(value: string) {
    this.groupId = value;
    this.callService.getUsersInCallOfGroup(value);
  }


  constructor(private callService: CallService) {}

  ngOnInit(): void {
    this.callService.onAllUsersInCallReceived((x) => (this.callMembers = x));
    this.callService.onUserQuitCall((x) => this.removeUser(x));
    this.callService.onUserEntryCall((x) => this.addUserCall(x));
    this.callService.onPutUserCall((x) => this.updateUserCall(x));
  }

  public getMember(UserCall : UserCall) : GroupMember | undefined{
    return this.GroupMembers.find(x => x.id === UserCall.memberId);
  }

  private addUserCall(userCall: UserCall) {
    if (this.groupId) {
      if (this.groupId === userCall.groupId) {
        this.callMembers.push(userCall);
      }
    }
  }
  private addOrUpdate(userCall: UserCall) {
    if (this.groupId) {
      if (this.groupId === userCall.groupId) {
        var index = this.callMembers.findIndex(
          (f) => f.memberId === userCall.memberId
        );
        if (index === -1) this.callMembers.push(userCall);
        else this.callMembers[index] = userCall;
      }
    }
  }

  private updateUserCall(userCall: UserCall) {
    if (this.groupId) {
      if (this.groupId === userCall.groupId) {
        var index = this.callMembers.findIndex(
          (usr) => usr.memberId === userCall.memberId
        );
        if (index !== -1) {
          this.callMembers[index] = userCall;
        }
      }
    }
  }

  private removeUser(userCall: UserCall) {
    if (this.groupId) {
      if (this.groupId === userCall.groupId) {
        var index = this.callMembers.findIndex(
          (usr) => usr.memberId === userCall.memberId
        );
        if (index !== -1) {
          this.callMembers.splice(index, 1);
        }
      }
    }
  }
}
