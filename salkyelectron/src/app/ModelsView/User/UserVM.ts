import { ContactVM } from "./ContactVM";
import { UserMinimalVM } from "./UserMinimalVM";

export interface UserVM extends UserMinimalVM {
  Contacts : ContactVM[];
}
