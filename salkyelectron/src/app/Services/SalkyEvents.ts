import { MessageServer } from "../Models/MessageWsServer";
import { Method } from "../Models/Method";

interface Handler {
    id: any;
    sucess?: ((data: any) => void);
    error?: ((data: any) => void);
}



interface Route {
    path: string;
    method: string
    handlers: Handler[];
}
export interface Destroyable {
    destroy: () => void;
}



export class SalkyEvents {
    public events: Map<string, Route>;
    constructor(private EnablePreventDuplicated: boolean) {
        this.events = new Map<string, Route>();
    }
    private createRouteKey(path: string, method: string): string {
        return `${path}${method}`.toLowerCase().trim();
    }
    private generateHandlerKey(): string {

        if (this.EnablePreventDuplicated)
        {
            // Error.stackTraceLimit = 1000;
            return new Error().stack?.toString() ?? Math.random().toString();
        }
      
        else
            return Math.random().toString();
    }

    private preventDuplicatedIfActive(route: Route) {
        if (!this.EnablePreventDuplicated) return;
        var stack = new Error().stack?.toString();
        route.handlers.forEach(x => {
            if (x.id === stack) {
                console.warn(`ALERT - Preveting removing route.. P:${route.path} M:${route.method}`);
                this.removeHandlerByIdWithRoute(x.id,route);
            }
        });
    }

    private createRouteIfNot(routeKey: string, method: string, path: string) {
        if (!this.events.has(routeKey)) {
            this.events.set(routeKey, {
                handlers: [],
                method: method,
                path: path
            });
        }
    }

    public sucess(msg: MessageServer) {
        this.shoot(msg, true);
    }
    public error(msg: MessageServer) {
        this.shoot(msg, false);
    }
    private shoot(msg: MessageServer, sucess: boolean) {
        var key = this.createRouteKey(msg.path, msg.method);
        var route = this.events.get(key);
        if (!route) return;
        route.handlers.forEach(handler => {
            if (sucess && handler.sucess)
                handler.sucess(msg.data);
            else if (handler.error)
                handler.error(msg.data);
        });
    }

    public on<T>
        (
            path: string,
            method: Method,
            sucess?: (data: T) => void,
            error?: (data: T) => void,
    ): Destroyable {
        var routeKey = this.createRouteKey(path, method);
        var singleKey = this.generateHandlerKey();
        this.createRouteIfNot(routeKey, method, path);
        var route = this.events.get(routeKey);
        if (!route) throw new Error("");
        this.preventDuplicatedIfActive(route);
        route.handlers.push({
            sucess: sucess,
            error: error,
            id: singleKey,
        });
        return {
            destroy: () => this.removeHandlerByIdWithKey(singleKey,routeKey)
        };
    }

    public clear() {
        console.log("cleared");
        this.events.clear();
    }

    private removeHandlerByIdWithRoute(handlerId: string, route : Route) {
        var i = route.handlers.findIndex(x => x.id === handlerId);
        if (i === -1) return;
        route.handlers.splice(i, 1);
    }

    private removeHandlerByIdWithKey(handlerId: string, routeKey: string) {
        var route = this.events.get(routeKey);
        if(!route)return;
        this.removeHandlerByIdWithRoute(handlerId,route);
    }


}
