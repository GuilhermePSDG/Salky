import { MessageServer } from "src/app/Models/MessageWsServer";

export interface RouteFunction {
  guidId: string;
  handler: (x: MessageServer) => void;
  isTemporary: boolean;
  expiresDate?: Date;
}
