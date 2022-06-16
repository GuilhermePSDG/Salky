import { HttpClient } from '@angular/common/http';
import { ThisReceiver } from '@angular/compiler';
import { Injectable } from '@angular/core';
import {
  first,
  map,
  Observable,
  ReplaySubject,
  Subscription,
  take,
} from 'rxjs';
import { environment } from 'src/environments/environment';
import { Friend } from '../Models/Users/Friend';
import { SalkyWebSocket } from './SalykWsClient.service';
import { Destroyable } from './SalkyEvents';
import { WebSocketBaseService } from './WebSocketBaseService';

@Injectable({
  providedIn: 'root',
})



export class FriendService extends WebSocketBaseService {

  private url = `${environment.apiUrl}/friend`;
  private wsBasePath = "";
  constructor(
    private http: HttpClient,
    ws: SalkyWebSocket
  ) {
    var wsBasePath = "friend";
    super(ws, wsBasePath);
    this.wsBasePath = wsBasePath;
  }
  private setImageUrl(relativePath: string) {
    if (!relativePath.includes('http'))
      return `${environment.apiImageurl}/${relativePath}`.replace('\\', '/');
    else return relativePath;
  }
  public GetAllFriends(): Observable<Friend[]> {
    return this.http.get<Friend[]>(this.url).pipe(
      take(1),
      map((x) => {
        x.forEach((n) => (n.pictureSource = this.setImageUrl(n.pictureSource)));
        return x;
      })
    );
  }

  public removeFriend(friendId: string) {
    this.ws.sendMessageServer({
      data: friendId,
      method: 'delete',
      path: this.wsBasePath,
    });
  }

  public SendFriendRequest(userId: String) {
    this.ws.sendMessageServer({
      method: 'post',
      path: this.wsBasePath + '/add',
      data: userId,
    })
  }

  public AcceptFriendRequest(friendId: string) {
    this.ws.sendMessageServer({
      path: this.wsBasePath + '/accept',
      method: 'post',
      data: friendId,
    })
  }

  public sendMessageToFriend(friendId: string) {
    this.ws.sendMessageServer({
      data: friendId,
      method: 'redirect',
      path: this.wsBasePath + '/message/send',
    });
  }

  public RejectFriendRequest(friendId: string) {
    this.ws.sendMessageServer({
      data: friendId,
      method: 'post',
      path: this.wsBasePath + '/reject',
    });
  }

  public CancelFriendRequest(friendId: string) {
    this.ws.sendMessageServer({
      data: friendId,
      method: 'post',
      path: this.wsBasePath + '/cancel',
    });
  }

  //#region EVENTS
  public onFriendAddComfirmReceived(
    handler: (friend: Friend) => void,
    error?: (error: any) => void
  ): Destroyable {
    return this.ws.On(this.wsBasePath + '/add', 'confirm').Build(handler, error);
  }

  public onFriendAddReceived(
    handler: (friend: Friend) => void,
    error?: (error: any) => void
  ): Destroyable {
    return this.ws.On(this.wsBasePath + '/add', 'post').Build(handler, error);
  }

  onFriendDelete(
    handler: (friendId: string) => void,
    error?: (error: any) => void
  ): Destroyable {
    return this.ws.On(this.wsBasePath, 'delete').Build(handler, error);
  }
  public onFriendReject(
    handler: (friendId: string) => void,
    error?: (error: any) => void
  ): Destroyable {
    return this.ws.On(this.wsBasePath + '/reject', 'delete').Build(handler, error);
  }

  public onFriendCancel(
    handler: (friendId: string) => void,
    error?: (error: any) => void
  ): Destroyable {
    return this.ws.On(this.wsBasePath + '/cancel', 'delete').Build<string>(handler, error);
  }

  public onFriendChangePicture(
    handler: (data: { friendId: string; pictureSource: string }) => void
  ): Destroyable {
    throw new Error('Not Implemented');
  }

  public onFriendAccept(
    handler: (friend: Friend) => void,
    error?: (error: any) => void
  ): Destroyable {
    return this.ws.On(this.wsBasePath + '/accept', 'put').Build<Friend>(handler, error);
  }

  //#endregion
}
