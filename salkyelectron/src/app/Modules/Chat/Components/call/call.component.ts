import { Component, Input, OnInit } from '@angular/core';
import { Group } from 'src/app/Models/GroupModels/Group';
import { GroupMember } from 'src/app/Models/Users/UserGroup';
import { UserCall } from 'src/app/Models/Users/UserCall';
import { UserLogged } from 'src/app/Models/Users/UserLogged';
import { CallService } from 'src/app/Services/call.service';
import { SalkyWebSocket } from 'src/app/Services/SalykWsClient.service';
import { StorageService } from 'src/app/Services/storage.service';
import { UserService } from 'src/app/Services/UserService.service';
import { EventsDestroyables } from 'src/app/Services/WebSocketBaseService';

@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss'],
})
export class CallComponent extends EventsDestroyables implements OnInit {
  public callMembers: UserCall[] = [];
  @Input() GroupMembers: GroupMember[] = [];
  @Input() CurrentMember?: GroupMember;

  public get hasMembersInCall(): boolean {
    return this.callMembers.length > 0;
  }
  public get loggedUserIsInCall(): boolean {
    return (
      this.callMembers.findIndex(
        (x) => x.isInCall && x.userId === this.CurrentMember?.userId
      ) !== -1
    );
  }
  groupId?: string;
  @Input() set GroupId(value: string) {
    this.groupId = value;
    this.callService.getUsersInCallOfGroup(value);
  }


  ngOnDestroy(): void {
    this.Destroy();
  }
  constructor(private callService: CallService) {
    super();
  }

  ngOnInit(): void {
    this.AppendToDestroy(this.callService.onAllUsersInCallReceived((x) => {
      (this.callMembers = x)
    }));
    this.AppendToDestroy(this.callService.onUserQuitCall((x) => this.removeUser(x)));
    this.AppendToDestroy(this.callService.onUserEntryCall((x) => this.addUserCall(x)));
    this.AppendToDestroy(this.callService.onPutUserCall((x) => this.updateUserCall(x)));
  }

  public getMember(UserCall: UserCall): GroupMember | undefined {
    return this.GroupMembers.find(x => x.userId === UserCall.userId);
  }

  private addUserCall(userCall: UserCall) {
    if (this.groupId) {
      if (this.groupId === userCall.callId) {
        this.callMembers.push(userCall);
      }
    }
  }

  private updateUserCall(userCall: UserCall) {
    if (this.groupId) {
      if (this.groupId === userCall.callId) {
        var index = this.callMembers.findIndex(
          (usr) => usr.userId === userCall.userId
        );
        if (index !== -1) {
          this.callMembers[index] = userCall;
        }
      }
    }
  }

  private removeUser(userCall: UserCall) {
    if (this.groupId) {
      if (this.groupId === userCall.callId) {
        var index = this.callMembers.findIndex(
          (usr) => usr.userId === userCall.userId
        );
        if (index !== -1) {
          this.callMembers.splice(index, 1);
        }
      }
    }
  }

}
