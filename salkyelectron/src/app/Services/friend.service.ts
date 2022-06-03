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
import { MessageServer } from '../Models/MessageWsServer';
import { SalkyWebSocket } from './SalykWsClient.service';
import { User } from '../Models/Users/User';
import { UserService } from './UserService.service';

@Injectable({
  providedIn: 'root',
})
export class FriendService {
  // private FriendsArr: Friend[] = [];
  // private CurrentFriendsSource = new ReplaySubject<Friend[]>(1);
  // public CurrentFriends = this.CurrentFriendsSource.asObservable();
  private url = `${environment.apiUrl}/friend`;
  private loggedUser: User = {} as any;
  constructor(
    private usrService: UserService,
    private http: HttpClient,
    private ws: SalkyWebSocket
  ) {
    this.usrService.currentUser$.subscribe({
      next: (usr) => (this.loggedUser = usr),
    });
    // this.GetAllFriends().subscribe();
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
      path: 'friend',
    });
  }

  public SendFriendRequest(userId: String) {
    this.ws.sendMessageServer({
      method : 'post',
      path : 'friend/add',
      data : userId,
    })
  }

  public AcceptFriendRequest(friendId: string) {
    this.ws.sendMessageServer({
      path:'friend/accept',
      method : 'post',
      data : friendId,
    })
  }

  public sendMessageToFriend(friendId: string) {
    this.ws.sendMessageServer({
      data: friendId,
      method: 'redirect',
      path: 'friend/message/send',
    });
  }

  public RejectFriendRequest(friendId: string) {
    this.ws.sendMessageServer({
      data: friendId,
      method: 'post',
      path: 'friend/reject',
    });
  }

  public CancelFriendRequest(friendId: string) {
    this.ws.sendMessageServer({
      data: friendId,
      method: 'post',
      path: 'friend/cancel',
    });
  }

  //#region EVENTS
  public onFriendAddComfirmReceived(
    handler: (friend: Friend) => void,
    error?: (error: any) => void
  ): Subscription {
    return this.ws.On('friend/add', 'confirm').Build(handler, error);
  }

  public onFriendAddReceived(
    handler: (friend: Friend) => void,
    error?: (error: any) => void
  ): Subscription {
    return this.ws.On('friend/add', 'post').Build(handler, error);
  }

  onFriendDelete(
    handler: (friendId: string) => void,
    error?: (error: any) => void
  ): Subscription {
    return this.ws.On('friend', 'delete').Build(handler, error);
  }
  public onFriendReject(
    handler: (friendId: string) => void,
    error?: (error: any) => void
  ): Subscription {
    return this.ws.On('friend/reject', 'delete').Build(handler, error);
  }

  public onFriendCancel(
    handler: (friendId: string) => void,
    error?: (error: any) => void
  ): Subscription {
    return this.ws.On('friend/cancel', 'delete').Build<string>(handler, error);
  }

  public onFriendChangePicture(
    handler: (data: { friendId: string; pictureSource: string }) => void
  ): Subscription {
    throw new Error('Not Implemented');
  }

  public onFriendAccept(
    handler: (friend: Friend) => void,
    error?: (error: any) => void
  ): Subscription {
    return this.ws.On('friend/accept', 'put').Build<Friend>(handler, error);
  }

  //#endregion
}
