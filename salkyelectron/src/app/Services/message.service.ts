import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {  map, Observable,take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Message } from '../Models/Message';
import { MessageAdd } from '../Models/MessageAdd';
import { PaginationResult } from '../Models/PaginationResult';
import { Destroyable } from './SalkyEvents';
import { SalkyWebSocket } from './SalykWsClient.service';
import { WebSocketBaseService } from './WebSocketBaseService';

@Injectable({
  providedIn: 'root',
})
export class MessageService extends WebSocketBaseService {
  private wsBasePath = "";
  sendBase64Image(base64: string): Observable<string> {
    return this.http
      .post<string>(`${this.apiBaseUrl}/message/img`, base64)
      .pipe(take(1));
  }
  _cached: { [groupId: string]: Message[] } = {};

  constructor(private http: HttpClient,  ws: SalkyWebSocket) {
    var basePath = "group/message";
    super(ws,basePath);
    this.wsBasePath = basePath;
  }

  private apiBaseUrl = `${environment.apiUrl}/group`;

  

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
        return data;
      }));
  }

  public sendMessage(msg: MessageAdd) {
    this.ws.sendMessageServer({
      data : msg,
      method : 'redirect',
      path : this.wsBasePath,
    })
  }

  public deleteMessage(messageId: string): void {
    this.ws.sendMessageServer({
      data: messageId,
      path: this.wsBasePath,
      method: 'delete',
    });
  }


  public onMessageReceived(handler: (msg: Message) => void) : Destroyable {
    return this.ws.On(this.wsBasePath, 'post').Build<Message>(msg =>{
      console.log('executando msg received')
      handler(msg);
    });
  }

  public onMessageDeleted(
    handler: (removedMessage: { groupId: string; messageId: string }) => void
  ) :  Destroyable{
    return this.ws.On(this.wsBasePath, 'delete').Build(handler);
  }

}
