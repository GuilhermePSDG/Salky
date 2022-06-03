export enum MessageStatus {
  Success = 10,
  Error = -10,
  NotAutorized = -20,
  InvalidState = -30,
}
export interface MessageServer {
  path: string;
  method:
    | 'get'
    | 'get_response'
    | 'post'
    | 'put'
    | 'delete'
    | 'redirect'
    | 'listener'
    | 'confirm'
    | '*';
  status?: MessageStatus;
  data?: any;
}
