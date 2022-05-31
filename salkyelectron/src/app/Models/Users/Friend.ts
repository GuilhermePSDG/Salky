import { FriendFlag } from "./FriendFlag";
import { User } from "./User";

export interface Friend extends User{
  id : string;
  requestByCurrentUser : boolean;
  friendFlag : FriendFlag;
  userId:string;
}
