import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MessageServer } from '../Models/MessageWsServer';
import { UserLogged } from '../Models/Users/UserLogged';
import { Subject } from 'rxjs';
import { SalkyRouteBuilder } from '../RoutingBuildingModel/SalkyRouteBuilder';
import { SalkyEvents } from './SalkyEvents';

@Injectable({
  providedIn: 'root',
})
export class SalkyWebSocket {
  public events: SalkyEvents = new SalkyEvents();

  protected ws: WebSocket = {} as WebSocket;

  constructor() {
  }

  public get isConnected(): boolean {
    return this.ws.readyState === 1;
  }

  public connectIfnot(user: UserLogged): Promise<void> {
    if (!this.ws || (this.ws.readyState != 1 && this.ws.readyState != 0))
      return this.connect(user);
    return new Promise(() => { });
  }

  public async connect(user: UserLogged): Promise<void> {
    // console.info('Conectando..');
    // this.ws = new WebSocket(environment.webSocketUrl, ["Authorization", user.token])
    this.ws = new WebSocket(`${environment.webSocketUrl}?token=${user.token}`);
    this.ws.onopen = (event: any) => this.onOpen(event);
    this.ws.onerror = (event: any) => this.onError(event);
    this.ws.onclose = (event: any) => this.onClose(event);
    this.ws.onmessage = (event: any) => this.onMessage(event);
  }

  public close() {
    // console.info('WebSocket Close Method Called');
    this.ws.close();
  }

  public On(routeName: string, method: string) {
    return SalkyRouteBuilder.Create(this).On(routeName, method);
  }

  public sendMessageServer(message: MessageServer) {
    // console.log('Messaged sended : ' + JSON.stringify(message));
    if (this.ws.readyState === 1) this.ws.send(JSON.stringify(message));
  }

  protected onMessage(event: MessageEvent<any>): void {
    var msgServer = JSON.parse(event.data) as MessageServer;
    //  console.log('Messaged Received : ' + JSON.stringify(msgServer));
    this.ExecuteEvents(msgServer);
  }

  public async onOpen(event: any): Promise<void> {
    // console.info('Connection Open');
    // console.log(event);
    this.ExecuteEvents({
      path: 'open',
      method: '*',
      data: event,
    });
  }


  public onError(event: any): void {
    this.ExecuteEvents({
      method: '*',
      path: 'error',
      data: event,
    });
    // console.error('Socket error');
    // console.error(event);
  }

  public async onClose(event: any): Promise<void> {
    // console.info('WebSocket Connection Closed');
    // console.info(event);
    this.ExecuteEvents({
      method: '*',
      path: 'close',
      data: event,
    });
  }
  private ExecuteEvents(message: MessageServer) {
    if (message.status && message.status < 0) {
      this.events.error(message);
    } else {
      this.events.sucess(message);
    }
  }
  protected sleep(timeMs: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, timeMs));
  }
}
