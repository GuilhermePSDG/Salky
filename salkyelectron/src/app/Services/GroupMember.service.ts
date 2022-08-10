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
    super(ws)
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
        (value.pictureSource)
      );
    });
    this.AppendManyToDestroy(sub1, sub2, sub3);
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
        this.next(x);
      })
    );
  }

  private next(GroupMembers: GroupMember[]) {
    this.Members = GroupMembers;
    this.CurrentMembers.next(GroupMembers);
  }

  private nextCurrentMember(groupId: string): Observable<void> {
    return this.http
      .get<GroupMember>(`${this.apiBaseUrl}/self/${groupId}`)
      .pipe(
        take(1),
        map((x) => {
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
          return member;
        })
      );
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
    return this.ws.On(this.wsBasePath + '/change/picture', 'put').Build<any>((x) => {
      handler(x);
    });
  }
}
