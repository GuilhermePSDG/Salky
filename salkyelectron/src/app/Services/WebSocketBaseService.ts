import { Subscription } from "rxjs";
import { Destroyable } from "./SalkyEvents";
import { SalkyWebSocket } from "./SalykWsClient.service";

export class EventsDestroyables {
    private subscriptions: Subscription[] = [];
    private destroyables: Destroyable[] = [];
    private funcs: (() => void)[] = [];

    public Destroy() {
        this.subscriptions.forEach(x => x.unsubscribe());
        this.destroyables.forEach(x => x.destroy());
        this.funcs.forEach(x => x());
    }
    public AppendToDestroy(toDestroy: Destroyable | Subscription | (() => void)): EventsDestroyables {
        if (toDestroy instanceof Subscription) {
            this.subscriptions.push(toDestroy);
        } else if (typeof toDestroy === 'function') {
            this.funcs.push(toDestroy)
        }
        else {
            this.destroyables.push(toDestroy);
        }
        return this;
    }
    public AppendManyToDestroy(...toDestroy: (Destroyable | Subscription| (() => void))[]): EventsDestroyables {
        toDestroy.forEach(x => this.AppendToDestroy(x));
        return this;
    }
}

export class WebSocketBaseService extends EventsDestroyables {

    constructor(public ws: SalkyWebSocket) {
        super();
    }
    public override Destroy(): void {
        super.Destroy();
    }
  
}

