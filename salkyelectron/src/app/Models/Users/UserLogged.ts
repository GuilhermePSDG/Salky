import { User } from './User';

export interface UserLogged extends User {
  token: string;
  TokenExpire: Date;
}
