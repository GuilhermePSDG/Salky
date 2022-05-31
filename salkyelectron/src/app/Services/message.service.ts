import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { first, map, Observable, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Message } from '../Models/Message';
import { MessageAdd } from '../Models/MessageAdd';
import { PaginationResult } from '../Models/PaginationResult';
import { SalkyWebSocket } from './SalykWsClient.service';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  sendBase64Image(base64: string): Observable<string> {
    return this.http
      .post<string>(`${this.apiBaseUrl}/message/img`, base64)
      .pipe(take(1));
  }
  _cached: { [groupId: string]: Message[] } = {};

  private isCached(groupId: string): boolean {
    return this._cached[groupId] !== undefined;
  }

  constructor(private http: HttpClient, private ws: SalkyWebSocket) {}

  private apiBaseUrl = `${environment.apiUrl}/group`;

  private setImageUrl(relativePath: string) {
    if (!relativePath.includes('http')) {
      return `${environment.apiImageurl}/${relativePath}`;
    } else {
      return relativePath;
    }
  }

  public getMessagesOfGroup(
    groupId: string,
    nextPage: number,
    pageSize: number
  ): Observable<PaginationResult<Message>> {
    return this.http
      .get<PaginationResult<Message>>(
        `${this.apiBaseUrl}/message?groupId=${groupId}&currentPage=${nextPage}&pageSize=${pageSize}`
      )
      .pipe(take(1),map(data =>{
        data.results.forEach(n => n.author.pictureSource =this.setImageUrl(n.author.pictureSource));
        return data;
      }));
  }

  public onMessageReceived(handler: (msg: Message) => void) {
    this.ws.On('group/message', 'post').Do((f) => {
      if (f.data) {
        var data = f.data as Message;
        data.author.pictureSource =this.setImageUrl(data.author.pictureSource);
        handler(data);
      }
    });
  }

  public onMessageDeleted(
    handler: (removedMessage: { groupId: string; messageId: string }) => void
  ) {
    this.ws.On('group/message', 'delete').Do((f) => {
      if (f.data) {
        handler(f.data);
      }
    });
  }

  public sendMessage(msg: MessageAdd) {
    this.ws.send('group/message', 'redirect', msg as MessageAdd);
  }

  public deleteMessage(messageId: string): void {
    this.ws.sendMessageServer({
      data: messageId,
      path: 'group/message',
      method: 'delete',
    });
  }
}
