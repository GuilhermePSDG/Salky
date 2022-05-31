import { Embed } from "./Embed";
import { User } from "./Users/User";

export interface Message {
  id: string;
  groupId: string;
  content: string;
  author : User
  sendedAt: Date;
  embeds: Embed[];
}
