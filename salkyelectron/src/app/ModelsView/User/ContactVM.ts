import { MessageVM } from "../MessageVM";
import { UserMinimalVM } from "./UserMinimalVM";

export interface ContactVM extends UserMinimalVM{
  messages : MessageVM[];
}
