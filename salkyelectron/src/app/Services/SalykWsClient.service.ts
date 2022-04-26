import { Injectable } from '@angular/core';
import { delay } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CadastredRoute } from '../Models/CadastredRoute';
import { MessageServer } from '../Models/MessageWsServer';
import { UserLogged } from '../Models/UserLogged';
import { User } from '../Models/UserWsServer';

import { RsaService } from './rsa.service';
import { SalkyRouteBuilder } from './SalkyRouteBuilder';

@Injectable({
  providedIn: 'root',
})
export class SalykWsClientService {
  // private API_URL = 'ws://salky-websocket.herokuapp.com';
  protected currentUser: UserLogged = {} as UserLogged;
  public events: CadastredRoute[] = [];
  protected ws: WebSocket = {} as WebSocket;

  public IsConnected: boolean = false;

  constructor() {}

  //Initiate WebSocket connection
  public async connect(
    user: UserLogged
  ): Promise<void> {
    this.currentUser = user;
    this.ws = new WebSocket(`${environment.webSocketUrl}?Authorization=${user.token}`);
    this.ws.onopen = (event: any) => this.onOpen(event);
    this.ws.onerror = (event: any) => this.onError(event);
    this.ws.onclose = (event: any) => this.onClose(event);
    this.ws.onmessage = (event: any) => this.onMessage(event);
  }
  protected CreateRouteBuilder() {
    return SalkyRouteBuilder.Create(this);
  }
  public close() {
    this.ws.close();
  }

  public  On(routeName: string, method : string) {
    return this.CreateRouteBuilder().On(routeName,method);
  }

  protected async waitInRoute(
    routeName: string,
    method: string,
    maxTimeMs: number
  ): Promise<MessageServer | undefined> {
    let timeWait = 200;
    let result: MessageServer | undefined;

    var expireDate = new Date();
    expireDate.setTime(new Date().getTime() + maxTimeMs * 2);

    this.CreateRouteBuilder()
      .On(routeName,method)
      .Do((x) => (result = x))
      .ThenKill(expireDate);

    for (var i = 0; i < maxTimeMs; i += timeWait) {
      if (result !== undefined) {
        break;
      }
      await this.sleep(timeWait);
    }

    return result;
  }

  protected sendMessageServer(message: MessageServer) {
    this.ws.send(JSON.stringify(message));
  }

  //indicates that the connection is ready to send and receive data
  public async onOpen(event: any): Promise<void> {

    this.IsConnected = true;
  }
  //An event listener to be called when a message is received from the server
  protected onMessage(event: MessageEvent<any>): void {
    console.log("message received");
    var msgServer = JSON.parse(event.data) as MessageServer;
    console.log(msgServer);
    this.ExecuteEvents(msgServer);
  }

  protected ExecuteEvents(message: MessageServer): void {
    var foundIndex = this.events.findIndex(
      (x) => x.routePath === message.PathString
    );
    if (foundIndex !== -1) {
      var routeFunc = this.events[foundIndex].Functions;

      //Faz a execução dos eventos
      routeFunc.forEach((x) => {
        var canExec = true;
        //Se x for temporario e tiver data de expiração e estiver vencido, não pode executar
        if (x.isTemporary) {
          if (x.expiresDate) {
            if (x.expiresDate.getTime() <= new Date().getTime()) {
              canExec = false;
            }
          }
        }
        if (canExec) {
          x.handler(message);
        }
      });
      //Faz a remoção dos eventos temporarios.
      this.events[foundIndex].Functions = routeFunc.filter(
        (x) => !x.isTemporary
      );
    }
  }

  //An event listener to be called when an error occurs. This is a simple event named "error".
  public onError(event: any): void {
    console.error(event);
  }
  //An event listener to be called when the WebSocket connection's readyState changes to CLOSED.
  public onClose(event: any): void {
    console.warn(event);
  }

  protected sleep(timeMs: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, timeMs));
  }

}
