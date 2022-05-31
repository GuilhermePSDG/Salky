import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CadastredRoute } from '../Models/Internal/CadastredRoute';
import { MessageServer } from '../Models/MessageWsServer';
import { UserLogged } from '../Models/Users/UserLogged';
import { RsaService } from './rsa.service';
import { SalkyRouteBuilder } from '../Models/Internal/SalkyRouteBuilder';

@Injectable({
  providedIn: 'root',
})
export class SalkyWebSocket {
  public events: CadastredRoute[] = [];
  protected ws: WebSocket = {} as WebSocket;

  constructor() {}

  public get isConnected(): boolean {
    return this.ws.readyState === 1;
  }

  public connectIfnot(user: UserLogged): Promise<void> {
    if (!this.ws || (this.ws.readyState != 1 && this.ws.readyState != 0))
      return this.connect(user);
    return new Promise(() => {});
  }

  public clearEvents() {
    this.events = [];
  }

  //Initiate WebSocket connection
  private async connect(user: UserLogged): Promise<void> {
    console.info('Conectando..');
    console.info('Events : ');
    this.ws = new WebSocket(`${environment.webSocketUrl}?token=${user.token}`);
    this.ws.onopen = (event: any) => this.onOpen(event);
    this.ws.onerror = (event: any) => this.onError(event);
    this.ws.onclose = (event: any) => this.onClose(event);
    this.ws.onmessage = (event: any) => this.onMessage(event);
  }
  protected CreateRouteBuilder() {
    return SalkyRouteBuilder.Create(this);
  }

  public close() {
    console.info('WebSocket Close Method Called');
    this.ws.close();
  }

  public send(path: string, method: string, data: any = null) {
    this.sendMessageServer({
      method: method,
      path: path,
      data: data,
    });
  }

  public On(routeName: string, method: string) {
    return this.CreateRouteBuilder().On(routeName, method);
  }

  public sendMessageServer(message: MessageServer) {
    console.log('Messaged sended : ' + JSON.stringify(message));
    if (this.ws.readyState === 1) this.ws.send(JSON.stringify(message));
  }

  public async onOpen(event: any): Promise<void> {
    console.info('Connection Open');
    console.log(event);
    this.ExecuteEvents({
      path: 'open',
      method: '*',
      data: event,
    });
  }

  protected onMessage(event: MessageEvent<any>): void {
    var msgServer = JSON.parse(event.data) as MessageServer;
    console.log('Messaged Received : ' + JSON.stringify(msgServer));
    this.ExecuteEvents(msgServer);
  }

  public onError(event: any): void {
    this.ExecuteEvents({
      method: '*',
      path: 'error',
      data: event,
    });
    console.error('Socket error');
    console.error(event);
  }

  public async onClose(event: any): Promise<void> {
    console.info('WebSocket Connection Closed');
    console.info(event);
    this.ExecuteEvents({
      method: '*',
      path: 'close',
      data: event,
    });
  }

  private isSameRoute(origin: CadastredRoute, other: MessageServer): boolean {
    var result =
      (origin.routePath.toLocaleLowerCase() ===
        other.path.toLocaleLowerCase() ||
        origin.routePath === '*' ||
        other.path === '*') &&
      (origin.method.toLocaleLowerCase() === other.method.toLocaleLowerCase() ||
        origin.method === '*' ||
        other.method === '*');
    return result;
  }
  private ExecuteEvents(message: MessageServer): Promise<void> {
    return new Promise(() => {
      var foundIndex = this.events.findIndex((x) =>
        this.isSameRoute(x, message)
      );
      if (foundIndex !== -1) {
        var routeFunc = this.events[foundIndex].Functions;
        //Faz a execução dos eventos
        routeFunc.forEach((x) => {
          x.handler(message);
        });
      }
    });
  }

  private async startPing(): Promise<void> {
    while (this.ws.readyState === 1) {
      // console.log('pinged');
      this.sendMessageServer({
        path: 'ping',
        method: 'post',
        data: '',
      });
      await this.sleep(20000);
    }
  }

  protected sleep(timeMs: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, timeMs));
  }
}
