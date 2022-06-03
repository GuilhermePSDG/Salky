import { Injectable } from '@angular/core';
import { ReplaySubject, Subscription } from 'rxjs';
import { Group } from '../Models/GroupModels/Group';
import { UserCall as CallMember } from '../Models/Users/UserCall';
import { AudioState } from '../Models/AudioState';
import { AudioService } from './AudioService';
import { SalkyWebSocket } from './SalykWsClient.service';
import { StorageService } from './storage.service';
import { UserService } from './UserService.service';
import { UserLogged } from '../Models/Users/UserLogged';
import { environment } from 'src/environments/environment';
import { Converter } from '../Helpers/Converter';

@Injectable({
  providedIn: 'root',
})
export class CallService {
  public UserCall: CallMember;
  public UserLogged: UserLogged = {} as any;

  private BasePath = 'group/call';

  constructor(
    private audioService: AudioService,
    usrService: UserService,
    private ws: SalkyWebSocket,
    private storageService: StorageService
  ) {
    var sub1 = usrService.currentUser$.subscribe({
      next: (usr) => {
        this.UserLogged = usr;
      },
    });
    this.UserCall = {
      audioState: this.AudioState,
      isInCall: false,
      groupId: '',
      memberId: '',
    } as CallMember;
    var sub2 = this.audioState$.subscribe({
      next: (x) => (this.UserCall.audioState = x),
    });
    var sub3 = this.audioService.onMicrofoneOutPut = (blob) => {
      blob.arrayBuffer().then((buff) => this.sendAudioToCall(buff));
    };
    var sub4 = this.onAudioReceived((base64) => {
      if (this.canReproduceAudio) {
        var buffer = Converter.stringToArrayBuffer(atob(base64));
        var blob = new Blob([new Uint8Array(buffer, 0, buffer.byteLength)]);
        this.audioService.ReproduceAudio(blob);
      }
    });
    console.warn("A lot of subscriptions without finish");
  }

  public entryInCall(groupId: string): void {
    var data = {
      groupId: groupId,
      audioState: this.UserCall.audioState,
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
      path:`${this.BasePath}/all`,
      method:'get',
      data : groupId,
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

  private sendAudioToCall(data: ArrayBuffer) {
    if (this.canSendAudio) {
      var base64 = btoa(Converter.arrayBufferToString(data));
      this.ws.sendMessageServer({
        method: 'redirect',
        data: base64,
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
    this.changeAudioState();
  }

  ChangeHeadPhoneState() {
    this.AudioState.headPhoneMuted = !this.AudioState.headPhoneMuted;
    if (this.AudioState.headPhoneMuted) {
      this.AudioState.microFoneMuted = true;
    }
    this.changeAudioState();
  }

  private changeAudioState() {
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
  ) {
    this.ws.On(this.BasePath, 'redirect').Build(handler, error);
  }
  public onUserQuitCall(handler: (UserCall: CallMember) => void): Subscription {
    return this.ws.On(this.BasePath, 'delete').Build(handler);
  }
  public onUserEntryCall(
    handler: (UserCall: CallMember) => void
  ): Subscription {
    return this.ws.On(this.BasePath, 'post').Build(handler);
  }
  public onPutUserCall(handler: (UserCall: CallMember) => void): Subscription {
    return this.ws.On(this.BasePath, 'put').Build(handler);
  }

  public onAllUsersInCallReceived(
    handler: (users: CallMember[]) => void
  ): Subscription {
    return this.ws.On(`${this.BasePath}/all`, 'get_response').Build(handler);
  }
  //#endregion
}
