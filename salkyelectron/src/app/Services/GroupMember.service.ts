import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, ReplaySubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ListHelper } from '../Helpers/ListHelper';
import { Group } from '../Models/GroupModels/Group';
import { GroupMember } from '../Models/Users/UserGroup';
import { Destroyable } from './SalkyEvents';
import { SalkyWebSocket } from './SalykWsClient.service';
import { UserService } from './UserService.service';
import { WebSocketBaseService } from './WebSocketBaseService';

@Injectable({
  providedIn: 'root',
})
export class GroupMemberService extends WebSocketBaseService {
  private CurrentMembers = new ReplaySubject<GroupMember[]>(1);
  private CurrentMember = new ReplaySubject<GroupMember>(1);
  //
  public $CurrentMember = this.CurrentMember.asObservable();
  public $CurrentMembers = this.CurrentMembers.asObservable();
  //
  Members: GroupMember[] = [];
  //
  private apiBaseUrl = `${environment.apiUrl}/group/member`;
  wsBasePath: string;

  constructor(private http: HttpClient, ws: SalkyWebSocket) {
    var wsBasePath = 'group/member'
    super(ws, wsBasePath)
    this.wsBasePath = wsBasePath;
  }
  public setEvents() {
    var sub1 = this.onUserEntryInGroup((member) =>
      this.next(ListHelper.AddOrUpdate(this.Members, member, 'id'))
    );
    var sub2 = this.onMemberRemoved((member) =>
      this.next(ListHelper.TryRemove(this.Members, 'id', member.memberId))
    );
    var sub3 = this.onMemberChangePicture((value) => {
      ListHelper.UpdateField(
        this.Members,
        'id',
        value.memberId,
        'pictureSource',
        this.setImageUrl(value.pictureSource)
      );
    });
    this.AppendManyToDestroy(sub1,sub2,sub3);
  }

  public addFriendInGroup(friendId: string, groupId: string) {
    this.ws.sendMessageServer({
      data: {
        groupId: groupId,
        friendId: friendId,
      },
      method: 'post',
      path: this.wsBasePath,
    });
  }

  public removeMember(memberId: string) {
    this.ws.sendMessageServer({
      data: memberId,
      method: 'delete',
      path: this.wsBasePath,
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

  private onMemberRemoved(
    handler: (data: { memberId: string; groupId: string }) => void
  ): Destroyable {
    return this.ws.On(this.wsBasePath, 'delete').Build<any>((x) => handler(x));
  }
  private onUserEntryInGroup(
    handler: (GroupMember: GroupMember) => void
  ): Destroyable {
    return this.ws
      .On(this.wsBasePath, 'post')
      .Build<GroupMember>((x) => handler(x));
  }

  public onMemberChangePicture(
    handler: (data: {
      groupId: string;
      memberId: string;
      pictureSource: string;
    }) => void
  ): Destroyable {
    return this.ws.On(this.wsBasePath+'/change/picture', 'put').Build<any>((x) => {
      handler(x);
    });
  }
}
