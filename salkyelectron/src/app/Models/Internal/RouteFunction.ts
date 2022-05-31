import { MessageServer } from "src/app/Models/MessageWsServer";

export interface RouteFunction {
  id: string;
  handler: (x: MessageServer) => void;
}
