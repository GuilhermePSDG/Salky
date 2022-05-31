import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, ReplaySubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ListHelper } from '../Helpers/ListHelper';
import { Group } from '../Models/GroupModels/Group';
import { GroupMember } from '../Models/Users/UserGroup';
import { SalkyWebSocket } from './SalykWsClient.service';
import { UserService } from './UserService.service';

@Injectable({
  providedIn: 'root',
})
export class GroupMemberService {
  private CurrentMembers = new ReplaySubject<GroupMember[]>(1);
  private CurrentMember = new ReplaySubject<GroupMember>(1);
  //
  public $CurrentMember = this.CurrentMember.asObservable();
  public $CurrentMembers = this.CurrentMembers.asObservable();
  //
  Members: GroupMember[] = [];
  //
  private apiBaseUrl = `${environment.apiUrl}/group/member`;

  constructor(private http: HttpClient, private ws: SalkyWebSocket) {

  }
  public setEvents() {
    this.onUserEntryInGroup((member) =>
      this.next(ListHelper.AddOrUpdate(this.Members,member , 'id'))
    );
    this.onMemberRemoved((member) =>
      this.next(ListHelper.TryRemove(this.Members, 'id', member.memberId))
    );
    this.onMemberChangePicture(value =>{
      ListHelper.UpdateField(this.Members,'id',value.memberId,'pictureSource',this.setImageUrl(value.pictureSource))
    });
  }
  private onMemberRemoved(
    handler: (data: { memberId: string; groupId: string }) => void
  ) {
    this.ws.On('group/member', 'delete').Do((x) => handler(x.data));
  }
  private onUserEntryInGroup(handler: (GroupMember: GroupMember) => void) {
    this.ws.On('group/member', 'post').Do((x) => handler(x.data));
  }


  public addFriendInGroup(friendId: string, groupId: string) {
    this.ws.sendMessageServer({
      data: {
        groupId: groupId,
        friendId: friendId,
      },
      method: 'post',
      path: 'group/member',
    });
  }

  public removeMember(memberId: string) {
    this.ws.sendMessageServer({
      data: memberId,
      method: 'delete',
      path: 'group/member',
    });
  }

  public ChangeGroup(groupId: string) {
    this.nextAllMembers(groupId).subscribe({});
    this.nextCurrentMember(groupId).subscribe();
  }

  private nextAllMembers(groupId: string): Observable<void> {
    return this.http.get<GroupMember[]>(`${this.apiBaseUrl}/${groupId}`).pipe(
      take(1),
      map((x) => {
        x.forEach((n) => (n.pictureSource = this.setImageUrl(n.pictureSource)));
        this.next(x);
      })
    );
  }

  private next(GroupMembers: GroupMember[]) {
    this.Members = GroupMembers;
    GroupMembers.forEach(
      (x) => (x.pictureSource = this.setImageUrl(x.pictureSource))
    );
    this.CurrentMembers.next(GroupMembers);
  }

  public onMemberChangePicture(
    handler: (data: {
      groupId: string;
      memberId: string;
      pictureSource: string;
    }) => void
  ): void {
    this.ws
      .On('group/member/change/picture', 'put')
      .Do((x) => {
        console.log(`Data received : ${x.data}`);
        handler(x.data);
      });
  }

  private nextCurrentMember(groupId: string): Observable<void> {
    return this.http
      .get<GroupMember>(`${this.apiBaseUrl}/self/${groupId}`)
      .pipe(
        take(1),
        map((x) => {
          x.pictureSource = this.setImageUrl(x.pictureSource);
          this.CurrentMember.next(x);
        })
      );
  }

  public GetLoggedMemberOfGroup(groupId: string): Observable<GroupMember> {
    return this.http
      .get<GroupMember>(`${this.apiBaseUrl}/self/${groupId}`)
      .pipe(
        take(1),
        map((member) => {
          member.pictureSource = this.setImageUrl(member.pictureSource);
          return member;
        })
      );
  }
  private setImageUrl(relativePath: string) {
    if (!relativePath.includes('http')) {
      return `${environment.apiImageurl}/${relativePath}`.replace('\\', '/');
    } else {
      return relativePath;
    }
  }
}
