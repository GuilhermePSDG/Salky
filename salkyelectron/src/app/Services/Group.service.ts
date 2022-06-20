import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, ReplaySubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Group } from '../Models/GroupModels/Group';
import { SalkyWebSocket } from './SalykWsClient.service';
import { Router } from '@angular/router';
import { Destroyable } from './SalkyEvents';
import { WebSocketBaseService } from './WebSocketBaseService';

@Injectable({
  providedIn: 'root',
})
export class GroupService extends WebSocketBaseService {
  private groupsEventEmiter = new ReplaySubject<Group[]>(1);
  public groupsObservable = this.groupsEventEmiter.asObservable();
  private groups: Group[] = [];
  private apiBaseUrl = `${environment.apiUrl}/group`;
  private wsBasePath = '';
  constructor(
    private router: Router,
    private http: HttpClient,
    ws: SalkyWebSocket
  ) {
    var wsBasePath = 'group';
    super(ws,wsBasePath);
    this.wsBasePath = wsBasePath;
    this.setGroups().subscribe();

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
      this.ChangeFieldOfGroup(q.id, 'pictureSource', (q.value))
    );
    this.AppendToDestroy(sub1).AppendToDestroy(sub2).AppendToDestroy(sub3).AppendToDestroy(sub4).AppendToDestroy(sub5);
  }

  deleteGroup(GroupId: string) {
    this.ws.sendMessageServer({
      path: this.wsBasePath,
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
      path: this.wsBasePath + '/create',
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
          this.ChangeFieldOfGroup(GroupId, 'pictureSource', x);
        })
      );
  }

  public changeGroupName(groupId: string, name: string) {
    this.ws.sendMessageServer({
      data: { groupId: groupId, newGroupName: name },
      method: 'post',
      path: this.wsBasePath + '/change/name',
    });
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
        this.groups = f;
        this.groupsEventEmiter.next(this.groups);
      })
    );
  }

  //#region Events

  private onUserAddedInGroup(handler: (group: Group) => void): Destroyable {
    return this.ws.On(this.wsBasePath + '/member/added', 'post').Build<any>((x) => {
      handler(x);
    });
  }

  private onGroupNameChanged(
    handler: (newName: { groupId: string; newGroupName: string }) => void
  ): Destroyable {
    return this.ws
      .On(this.wsBasePath + '/change/name', 'put')
      .Build<any>((msg) => handler(msg));
  }

  private onPictureChanged(
    handler: (Data: { id: string; value: string }) => void
  ): Destroyable {
    return this.ws.On(this.wsBasePath + '/change/picture', 'put').Build<any>((x) => {
      handler(x);
    });
  }

  public onGroupCreated(handler: (group: Group) => void): Destroyable {
    return this.ws.On(this.wsBasePath + '/create', 'post').Build<Group>((x) => handler(x));
  }
  private onGroupDeleted(handler: (groupId: string) => void): Destroyable {
    return this.ws.On(this.wsBasePath, 'delete').Build<any>((x) => handler(x));
  }
  //#endregion
}
