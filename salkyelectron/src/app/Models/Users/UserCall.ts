import { AudioState } from '../AudioState';
import { User } from './User';

export interface UserCall {
  isInCall: boolean;
  groupId: string;
  memberId: string;
  audioState: AudioState;
}
