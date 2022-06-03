// interface RouteBuilderDo {
//   Do(todo: (message: MessageServer) => void): void;
//   Do<T>(todo: (value: T) => void): void;
// }
// interface RouteBuilderOn {
//   On(routeName: string, method: string): RouteBuilderDo;
// }
// export class SalkyRouteBuilder implements RouteBuilderOn, RouteBuilderDo {
//   private routeName: string = 'UNKNOW';
//   private method: string = 'UNKNOW';
//   private todo: (message: MessageServer) => void = (x) => {};
//   context: SalkyWebSocket;
//   private constructor(context: SalkyWebSocket) {
//     this.context = context;
//   }

import { Observable, Subject, Subscription } from 'rxjs';
import { MessageServer } from '../Models/MessageWsServer';
import { SalkyWebSocket } from '../Services/SalykWsClient.service';
import { RouteFunction } from './RouteFunction';

//   public static Create(context: SalkyWebSocket): RouteBuilderOn {
//     return new SalkyRouteBuilder(context);
//   }

//   On(routeName: string, method: string): RouteBuilderDo {
//     this.routeName = routeName;
//     this.method = method;
//     return this;
//   }

//   Do(todo: (message: MessageServer) => void): void {
//     this.todo = todo;
//     this.then();
//   }

//   getId(): string {
//     return new Error().stack?.toString() ?? Math.random.toString();
//   }

//   private then() {
//     if (!this.routeName || this.routeName.length < 1) {
//       throw new Error('RouteName cannot be null or empty');
//     }
//     var uniqueId = this.getId();

//     var funcToAdd = {
//       id: uniqueId,
//       handler: this.todo,
//     } as RouteFunction;

//     //Verifica se já existe uma rota-method
//     var index = this.context.events.findIndex(
//       (x) =>
//         x.routePath.toLocaleLowerCase() ===
//           this.routeName.toLocaleLowerCase() &&
//         x.method.toLocaleLowerCase() === this.method.toLocaleLowerCase()
//     );
//     //Se não existir adiciona a rota e a funçãox
//     if (index === -1) {
//       this.context.events.push({
//         Functions: [funcToAdd],
//         method: this.method,
//         routePath: this.routeName,
//       });
//     }
//     //Se existir
//     else {
//       var funcIndex = this.context.events[index].Functions.findIndex((x) => x.id === funcToAdd.id);
//       //Se não tem uma função com o mesmo id adiciona, se não substitui.
//       if (funcIndex === -1)this.context.events[index].Functions.push(funcToAdd);
//       else this.context.events[index].Functions[funcIndex] = funcToAdd;
//     }
//   }

// }

interface RouteBuilderDo {
  // Do(todo: (message: MessageServer) => void): void;
  // Observe<T>() : Observable<T>
  Build<T>(next: (data: T) => void, error?: (err: any) => void): Subscription;
}
interface RouteBuilderOn {
  On(routeName: string, method: string): RouteBuilderDo;
}
export class SalkyRouteBuilder implements RouteBuilderOn, RouteBuilderDo {
  private routeName: string = 'UNKNOW';
  private method: string = 'UNKNOW';

  context: SalkyWebSocket;
  private constructor(context: SalkyWebSocket) {
    this.context = context;
  }

  Build<T>(next: (data: T) => void, error?: (err: any) => void): Subscription {
    return this.context.OnMessageReceived.subscribe({
      next: (x) => {
        if (
          x.path.toLocaleLowerCase() === this.routeName &&
          this.method === x.method.toLowerCase()
        ){
          next(x.data);
        }
      },
      error: (err) => {
        if (
          error &&
          err.path.toLocaleLowerCase() === this.routeName &&
          this.method === err.method.toLowerCase()
        ){
          error(err.data);
        }
      },
    });
  }

  public static Create(context: SalkyWebSocket): RouteBuilderOn {
    return new SalkyRouteBuilder(context);
  }

  On(routeName: string, method: string): RouteBuilderDo {
    this.routeName = routeName.toLowerCase();
    this.method = method.toLowerCase();
    return this;
  }
}
