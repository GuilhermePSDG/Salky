import { PartialContent } from "./PartialContent";
import { User } from "./Users/User";

export interface Message {
  id: string;
  groupId: string;
  content: string;
  author : User
  sendedAt: Date;
  partialContents: PartialContent[];
}
