import { Method } from "./Method";

export enum MessageStatus {
  Success = 10,
  Error = -10,
  NotAutorized = -20,
  InvalidState = -30,
}
export interface MessageServer {
  path: string;
  method: Method,
  status?: MessageStatus;
  data?: any;
}
