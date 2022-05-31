import { HttpClient } from '@angular/common/http';
import { ThisReceiver } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { first, map, Observable, ReplaySubject, take } from 'rxjs';
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

  public onFriendAddComfirmReceived(handler: (friend: Friend) => void) {
    this.ws.On('friend/add', 'confirm').Do((res) => {
      res.data.pictureSource = this.setImageUrl(res.data.pictureSource);
      handler(res.data);
    });
  }

  public onFriendAddReceived(handler: (friend: Friend) => void) {
    this.ws.On('friend/add', 'post').Do((res) => {
      res.data.pictureSource = this.setImageUrl(res.data.pictureSource);
      handler(res.data);
    });
  }

  onFriendDelete(handler: (friendId: string) => void) {
    this.ws.On('friend', 'delete').Do((res) => {
      handler(res.data);
    });
  }

  public removeFriend(friendId: string) {
    this.ws.sendMessageServer({
      data: friendId,
      method: 'delete',
      path: 'friend',
    });
  }

  public onFriendReject(handler: (friendId: string) => void) {
    this.ws.On('friend/reject', 'delete').Do((res) => {
      const friendId = res.data as string;
      handler(friendId);
    });
  }

  public onFriendCancel(handler: (friendId: string) => void) {
    this.ws.On('friend/cancel', 'delete').Do((res) => {
      const friendId = res.data as string;
      handler(friendId);
    });
  }

  public onFriendChangePicture(
    handler: (data: { friendId: string; pictureSource: string }) => void
  ) {
    throw new Error("Not Implemented");
    this.ws.On('friend/change/picture', 'put').Do((x) => {
      x.data.pictureSource = this.setImageUrl(x.data.pictureSource);
      handler(x.data);
    });
  }

  public onFriendAccept(handler: (friend: Friend) => void) {
    this.ws.On('friend/accept', 'put').Do((res) => {
      const friend = res.data as Friend;
      friend.pictureSource = this.setImageUrl(friend.pictureSource);
      handler(friend);
    });
  }

  public SendFriendRequest(userId: String) {
    this.ws.send('friend/add', 'post', userId);
  }

  public AcceptFriendRequest(friendId: string) {
    this.ws.send('friend/accept', 'post', friendId);
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
}
