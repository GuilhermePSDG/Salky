import { HttpClient } from '@angular/common/http';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Injectable, OnInit } from '@angular/core';
import { map, Observable, ReplaySubject, Subscription, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Group } from '../Models/GroupModels/Group';
import { GroupMember } from '../Models/Users/UserGroup';
import { MessageAdd } from '../Models/MessageAdd';
import { UserCall } from '../Models/Users/UserCall';
import { SalkyWebSocket } from './SalykWsClient.service';
import { GroupMemberService } from './GroupMember.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class GroupService {
  private groupsEventEmiter = new ReplaySubject<Group[]>(1);
  public groupsObservable = this.groupsEventEmiter.asObservable();
  private groups: Group[] = [];
  private apiBaseUrl = `${environment.apiUrl}/group`;

  constructor(
    private router: Router,
    private http: HttpClient,
    private ws: SalkyWebSocket
  ) {
    this.setGroups().subscribe();
  }

  eventsStarted = false;
  SubscribeEvents() {
    if(this.eventsStarted) return;
    this.eventsStarted = true;

    var sub1 = this.onGroupNameChanged((x) =>
      this.ChangeFieldOfGroup(x.groupId, 'name', x.newGroupName)
    );
    var sub2 = this.onGroupDeleted((groupId) => {
      this.removeGroup(groupId);
      if (this.router.url.includes(groupId)) {
        this.router.navigateByUrl('');
      }
    });

    var sub3 = this.onGroupCreated((x) => this.addOrUpdateGroup(x));
    var sub4 = this.onUserAddedInGroup((x) => this.addOrUpdateGroup(x));
    var sub5 = this.onPictureChanged((q) =>
      this.ChangeFieldOfGroup(q.id, 'pictureSource', this.setImageUrl(q.value))
    );
    this.subs.push(sub1,sub2,sub3,sub4,sub5);
  }
  subs : Subscription[] = [];
  UnsubscribeEvents(){
    this.eventsStarted = false;
    this.subs.forEach(x => x.unsubscribe());
  }

  deleteGroup(GroupId: string) {
    this.ws.sendMessageServer({
      path: 'group',
      method: 'delete',
      data: GroupId,
    });
  }

  public findGroup(Id: string): Group | null {
    var i = this.groups.findIndex((x) => x.id === Id);
    return i === -1 ? null : this.groups[i];
  }

  public createGroup(name: string): void {
    this.ws.sendMessageServer({
      data: name,
      method: 'post',
      path: 'group/create',
    });
  }

  public ChangeGroupPicture(GroupId: string, base64: string): Observable<void> {
    return this.http
      .put<string>(`${this.apiBaseUrl}/picture/${GroupId}`, {
        value: base64,
      })
      .pipe(
        take(1),
        map((x) => {
          var nPicture = `${environment.apiImageurl}/${x}`;
          this.ChangeFieldOfGroup(GroupId, 'pictureSource', nPicture);
        })
      );
  }

  public changeGroupName(groupId: string, name: string) {
    this.ws.sendMessageServer({
      data: { groupId: groupId, newGroupName: name },
      method: 'post',
      path: 'group/change/name',
    });
  }

  private setImageUrlOfGroup(Group: Group) {
    if (!Group.pictureSource.includes('http'))
      Group.pictureSource = this.setImageUrl(Group.pictureSource);
  }
  private setImageUrl(relativePath: string) {
    return `${environment.apiImageurl}/${relativePath}`.replace('\\', '/');
  }

  private ChangeFieldOfGroup(
    GroupId: string,
    Field: keyof Group,
    NewValue: any
  ) {
    var i = this.groups.findIndex((x) => x.id === GroupId);
    if (i !== -1) {
      if (this.groups[i][Field] !== NewValue) {
        this.groups[i][Field] = NewValue;
      }
    }
  }

  private addOrUpdateGroup(Group: Group): void {
    var i = this.groups.findIndex((x) => x.id === Group.id);
    if (i === -1) {
      this.setImageUrlOfGroup(Group);
      this.groups.push(Group);
      this.groupsEventEmiter.next(this.groups);
    } else {
      this.groups[i] = Group;
      this.groupsEventEmiter.next(this.groups);
    }
  }

  private removeGroup(id: string) {
    var i = this.groups.findIndex((x) => x.id === id);
    if (i !== -1) {
      this.groups.splice(i, 1);
    }
  }

  private setGroups(): Observable<void> {
    return this.http.get<Group[]>(`${this.apiBaseUrl}`).pipe(
      take(1),
      map((f) => {
        f.forEach((g) => this.setImageUrlOfGroup(g));
        this.groups = f;
        this.groupsEventEmiter.next(this.groups);
      })
    );
  }

  //#region Events

  private onUserAddedInGroup(handler: (group: Group) => void) : Subscription {
    return this.ws.On('group/member/added', 'post').Build<any>((x) => {
      handler(x);
    });
  }

  private onGroupNameChanged(
    handler: (newName: { groupId: string; newGroupName: string }) => void
  ): Subscription {
    return this.ws
      .On('group/change/name', 'put')
      .Build<any>((msg) => handler(msg));
  }

  private onPictureChanged(
    handler: (Data: { id: string; value: string }) => void
  ): Subscription {
    return this.ws.On('group/change/picture', 'put').Build<any>((x) => {
      x.data.imageSource = this.setImageUrl(x.data.imageSource);
      handler(x);
    });
  }

  public onGroupCreated(handler: (group: Group) => void): Subscription {
    return this.ws.On('group/create', 'post').Build<Group>((x) => handler(x));
  }
  private onGroupDeleted(handler: (groupId: string) => void): Subscription {
    return this.ws.On('group', 'delete').Build<string>((x) => handler(x));
  }
  //#endregion
}
