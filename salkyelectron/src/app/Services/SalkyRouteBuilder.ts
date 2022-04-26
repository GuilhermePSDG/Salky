
import { MessageServer } from '../Models/MessageWsServer';
import { RouteFunction } from '../Models/RouteFunction';
import { SalykWsClientService } from './SalykWsClient.service';

interface RouteBuilderThen {
  ThenKill(expiresDate: Date): void;
  ThenDoAgain(): void;
  DoToo(todo: (message: MessageServer) => void): RouteBuilderThen;
}
interface RouteBuilderDo {
  Do(todo: (message: MessageServer) => void): RouteBuilderThen;
}
interface RouteBuilderOn {
  On(routeName: string, method:string): RouteBuilderDo;
}
export class SalkyRouteBuilder
  implements RouteBuilderOn, RouteBuilderDo, RouteBuilderThen
{
  private routeName?: string;
  private method: string ='GET';
  private todos: ((message: MessageServer) => void)[] = [];
  context: SalykWsClientService;
  private constructor(context : SalykWsClientService) {
    this.context = context;
  }

  public static Create(context: SalykWsClientService) : RouteBuilderOn{
    return new SalkyRouteBuilder(context);
  }

  On(routeName: string, method:string): RouteBuilderDo {
    this.routeName = routeName;
    this.method = method;
    return this;
  }
  Do(todo: (message: MessageServer) => void): RouteBuilderThen {
    this.todos.push(todo);
    return this;
  }
  DoToo(todo: (message: MessageServer) => void): RouteBuilderThen {
    this.todos.push(todo);
    return this;
  }

  ThenKill(expiresDate?: Date): void {
    this.then(true, expiresDate);
  }
  ThenDoAgain(): void {
    this.then(false);
  }

  private then(isTemp: boolean, expiresDate?: Date) {
    if (!this.routeName || this.routeName.length < 1) {
      throw new Error('RouteName cannot be null or empty');
    }
    var funcs: RouteFunction[] = [];
    this.todos.forEach((f) => {
      funcs.push({
        guidId: this.generateUUID(),
        handler: f,
        isTemporary: isTemp,
        expiresDate: expiresDate,
      });
    });

    var index = this.context.events.findIndex(
      (x) => x.routePath === this.routeName
    );

    if (index != -1) {
      funcs.forEach((x) => this.context.events[index].Functions.push(x));
    } else {
      this.context.events.push({
        Functions: funcs,
        method : this.method,
        routePath: this.routeName,
      });
    }
  }

  generateUUID() {
    // Public Domain/MIT
    var d = new Date().getTime(); //Timestamp
    var d2 =
      (typeof performance !== 'undefined' &&
        performance.now &&
        performance.now() * 1000) ||
      0; //Time in microseconds since page-load or 0 if unsupported
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(
      /[xy]/g,
      function (c) {
        var r = Math.random() * 16; //random number between 0 and 16
        if (d > 0) {
          //Use timestamp until depleted
          r = (d + r) % 16 | 0;
          d = Math.floor(d / 16);
        } else {
          //Use microseconds since page-load if supported
          r = (d2 + r) % 16 | 0;
          d2 = Math.floor(d2 / 16);
        }
        return (c === 'x' ? r : (r & 0x3) | 0x8).toString(16);
      }
    );
  }
}
