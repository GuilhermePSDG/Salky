import { User } from './User';

export interface GroupMember extends User {
  roleName: string;
  groupId:string;
  groupRole?: GroupRoleDto;
  userId : string;
}

export interface GroupRoleDto {
  roleName: string;
  groupPermissions: GroupPermissions;
  chatPermissions: ChatPermissions;
  callPermisions: CallPermisions;
}

export interface GroupPermissions {
  canInviteOtherUsers: boolean;
  canRemoveOtherUsers: boolean;
  canEditGroupName: boolean;
  canEditGroupPicture: boolean;
  canChangeOtherUserRoles: boolean;
}

export interface ChatPermissions extends User {
  canDeleteOtherUserMessages: boolean;
  canSendMessage: boolean;
  canReadMessage: boolean;
}
export interface CallPermisions extends User {
  canMuteMicrofoneOfOtherUser: boolean;
  canUnMuteMicrofoneOfOtherUser: boolean;
  canMuteHeadPhoneOfOtherUser: boolean;
  canUnMuteHeadPhoneOfOtherUser: boolean;
  canEntryInCall: boolean;
  canSeeCall: boolean;
}
