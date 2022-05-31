import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
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

  private BasePath = "group/call";

  constructor(
    private audioService: AudioService,
    usrService: UserService,
    private ws: SalkyWebSocket,
    private storageService: StorageService
  ) {
    usrService.currentUser$.subscribe({
      next: (usr) => {
        this.UserLogged = usr;
      },
    });
    this.UserCall = {
      audioState: this.AudioState,
      isInCall: false,
      groupId: '',
      memberId : '',
    } as CallMember;
    this.audioState$.subscribe({
      next: (x) => (this.UserCall.audioState = x),
    });
    this.audioService.onMicrofoneOutPut = (blob) => {
      blob.arrayBuffer().then((buff) => this.sendAudioToCall(buff));
    };
    this.onAudioReceived((sender, blob) =>
      this.audioService.ReproduceAudio(blob)
    );
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

  public onUserQuitCall(handler: (UserCall: CallMember) => void) {
    this.onUserAny(handler,'delete');
  }
  public onUserEntryCall(handler: (UserCall: CallMember) => void) {
    this.onUserAny(handler, 'post');
  }
  public onPutUserCall(handler: (UserCall: CallMember) => void) {
    this.onUserAny(handler, 'put');
  }
  private onUserAny(handler: (UserCall: CallMember) => void, method: string) {
    this.ws.On(this.BasePath, method).Do((x) => {
      var data = x.data as CallMember;
      handler(data);
    });
  }

  public onAllUsersInCallReceived(handler: (users: CallMember[]) => void) {
    this.ws.On(`${this.BasePath}/all`, 'get_response').Do((f) => {
      var users = f.data as CallMember[];
      handler(users);
    });
  }

  public getUsersInCallOfGroup(groupId: string) {
    this.ws.send(`${this.BasePath}/all`, 'get', groupId);
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

  private onAudioReceived(handler: (senderName: string, blob: Blob) => void) {
    this.ws.On(this.BasePath, 'redirect').Do((x) => {
      if (this.canReproduceAudio) {
        var buffer = Converter.stringToArrayBuffer(atob(x.data));
        var blob = new Blob([new Uint8Array(buffer, 0, buffer.byteLength)]);
        handler(x.data.senderIndentifier, blob);
      }
    });
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

  private setImageUrl(relativePath: string) {
    if (!relativePath.includes('http')) {
      return `${environment.apiImageurl}/${relativePath}`;
    } else {
      return relativePath;
    }
  }
}
