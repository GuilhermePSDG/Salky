import { AudioState } from '../AudioState';
import { User } from './User';

export interface UserCall {
  isInCall: boolean;
  callId: string;
  userId: string;
  audioState: AudioState;
}
