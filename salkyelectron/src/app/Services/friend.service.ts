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
import { FriendFlag } from '../Models/Users/FriendFlag';

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
    super(ws);
    this.wsBasePath = wsBasePath;
  }
  
  public GetAllFriends(): Observable<Friend[]> {
    return this.http.get<Friend[]>(this.url).pipe(
      take(1),
      map((x) => {
        return x;
      })
    );
  }


  public SendFriendRequest(userId: String) {
    this.ws.sendMessageServer({
      method: 'post',
      path: this.wsBasePath + '/add',
      data: userId,
    })
  }
  public sendMessageToFriend(friendId: string) {
    this.ws.sendMessageServer({
      data: friendId,
      method: 'redirect',
      path: this.wsBasePath + '/message/send',
    });
  }

  public AcceptFriendRequest(friendId: string) {
    this.updateFriend(friendId,FriendFlag.Approved);
  }
  public RejectFriendRequest(friendId: string) {
    this.updateFriend(friendId,FriendFlag.Rejected);
  }
  public CancelFriendRequest(friendId: string) {
    this.updateFriend(friendId,FriendFlag.Canceled);
  }
  public removeFriend(friendId: string) {
    this.updateFriend(friendId,FriendFlag.Removed);
  }
  public updateFriend(friendId : string,status : FriendFlag){
    this.ws.sendMessageServer({
      data:{
        friendId:friendId,
        status:status,
      },
      method: 'put',
      path: `${this.wsBasePath}/status`,
    });
  }

  public onFriendPut(
    handler:(friend:Friend) => void ,
    error? : (error:any) => void
  ) : Destroyable{
    return this.ws
    .On(`${this.wsBasePath}`,"put")
    .Build(handler,error);
  }

  public onFriendDeleted(
    handler:(friend:Friend) => void ,
    error? : (error:any) => void
  ){
    return this.ws
    .On(`${this.wsBasePath}`,"delete")
    .Build(handler,error);
  }

}
