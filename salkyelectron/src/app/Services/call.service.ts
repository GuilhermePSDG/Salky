import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { UserCall as CallMember } from '../Models/Users/UserCall';
import { AudioState } from '../Models/AudioState';
import { AudioService } from './AudioService';
import { SalkyWebSocket } from './SalykWsClient.service';
import { UserService } from './UserService.service';
import { UserLogged } from '../Models/Users/UserLogged';
import { Converter } from '../Helpers/Converter';
import { Destroyable } from './SalkyEvents';
import { WebSocketBaseService } from './WebSocketBaseService';

@Injectable({
  providedIn: 'root',
})
export class CallService extends WebSocketBaseService {
  public UserCall: CallMember;
  public UserLogged: UserLogged = {} as any;

  private BasePath = '';

  constructor(
    private audioService: AudioService,
    usrService: UserService,
    ws: SalkyWebSocket,
  ) {
    var basepath = 'group/call';
    super(ws);
    this.BasePath = basepath;
    this.UserCall = {
      audioState: this.AudioState,
      isInCall: false,
      callId: '',
      userId: '',
    } as CallMember;

    var sub1 = usrService.currentUser$.subscribe({
      next: (usr) => {
        this.UserLogged = usr;
      },
    });
    var sub2 = this.audioState$.subscribe({
      next: (x) => (this.UserCall.audioState = x),
    });
    this.audioService.onMicrofoneOutPut = (data) => this.sendAudioToCall(data);
    var sub4 = this.onAudioReceived((base64) => {
      if (this.canReproduceAudio) {
        this.audioService.ReproduceAudio(base64);
      }
    });
    this.AppendToDestroy(sub1).AppendToDestroy(sub2).AppendToDestroy(sub4);
    this.syncAudioState();
  }


  public entryInCall(groupId: string): void {
    var data = {
      groupId: groupId,
      // audioState: this.UserCall.audioState,
    };
    this.ws.sendMessageServer({
      data: data,
      method: 'post',
      path: this.BasePath,
    });
    this.UserCall.isInCall = true;
  }

  public quitFromCall(): void {
    this.ws.sendMessageServer({
      data: '',
      method: 'delete',
      path: this.BasePath,
    });
    this.UserCall.isInCall = false;
  }

  public getUsersInCallOfGroup(groupId: string) {
    this.ws.sendMessageServer({
      path: `${this.BasePath}/all`,
      method: 'get',
      data: groupId,
    })
  }

  private get canSendAudio(): boolean {
    return (
      this.UserCall.isInCall &&
      !this.UserCall.audioState.microFoneMuted &&
      !this.UserCall.audioState.headPhoneMuted &&
      this.ws.isConnected
    );
  }

  private get canReproduceAudio(): boolean {
    return this.UserCall.isInCall && !this.UserCall.audioState.headPhoneMuted;
  }

  private sendAudioToCall(data: any) {
    if (this.canSendAudio) {
      this.ws.sendMessageServer({
        method: 'redirect',
        data: data,
        path: this.BasePath,
      });
    }
  }

  private audioStateReplaySubjet = new ReplaySubject<AudioState>(1);
  public audioState$ = this.audioStateReplaySubjet.asObservable();
  private AudioState: AudioState = {
    headPhoneMuted: false,
    microFoneMuted: false,
  };

  public get getAudioState(): AudioState {
    return this.AudioState;
  }

  ChangeMicrofoneState() {
    this.AudioState.microFoneMuted = !this.AudioState.microFoneMuted;
    if (!this.AudioState.microFoneMuted && this.AudioState.headPhoneMuted) {
      this.AudioState.headPhoneMuted = false;
    }
    this.syncAudioState();
  }

  ChangeHeadPhoneState() {
    this.AudioState.headPhoneMuted = !this.AudioState.headPhoneMuted;
    if (this.AudioState.headPhoneMuted) {
      this.AudioState.microFoneMuted = true;
    }
    this.syncAudioState();
  }

  private syncAudioState() {
    this.ws.sendMessageServer({
      data: this.AudioState,
      method: 'put',
      path: this.BasePath,
    });
    this.audioStateReplaySubjet.next(this.AudioState);
  }

  //#region Events
  private onAudioReceived(
    handler: (Base64: string) => void,
    error?: (err: any) => void
  ): Destroyable {
    return this.ws.On(this.BasePath, 'redirect').Build((x: any) => handler(x), error);
  }
  public onUserQuitCall(handler: (UserCall: CallMember) => void): Destroyable {
    return this.ws.On(this.BasePath, 'delete').Build(handler);
  }
  public onUserEntryCall(
    handler: (UserCall: CallMember) => void
  ): Destroyable {
    return this.ws.On(this.BasePath, 'post').Build(handler);
  }
  public onPutUserCall(handler: (UserCall: CallMember) => void): Destroyable {
    return this.ws.On(this.BasePath, 'put').Build(handler);
  }

  public onAllUsersInCallReceived(
    handler: (users: CallMember[]) => void
  ): Destroyable {
    return this.ws.On(`${this.BasePath}/all`, 'get').Build(handler);
  }
  //#endregion
}
