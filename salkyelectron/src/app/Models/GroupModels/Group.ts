import { GroupMember } from '../Users/UserGroup';

export interface Group {
  id: string;
  name: string;
  ownerId:string;
  pictureSource: string;
  config:{
    isPrivate : boolean,
    maxUsers : number,
  }
}
