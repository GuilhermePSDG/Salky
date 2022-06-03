import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { Group } from '../Models/GroupModels/Group';
import { AudioState } from '../Models/AudioState';

export class AudioService {
  private mediaRecorder?: MediaRecorder;
  private audioChunks?: any[];
  private audioChunksBufferTime = 300;
  private audio = new Audio();
  public onMicrofoneOutPut: (data: Blob) => void = (_) => {};

  constructor() {}

  public async START(): Promise<void> {
    var started = await navigator.mediaDevices
      .getUserMedia({
        audio: true,
      })
      .then((stream) => {
        this.mediaRecorder = new MediaRecorder(stream);
        this.audioChunks = [];
        this.mediaRecorder.ondataavailable = (x: BlobEvent) => {
          if (this.audioChunks) this.audioChunks.push(x.data);
        };
        this.mediaRecorder.onstop = () => {
          if (!this.isStoped) {
            const audioBlob = new Blob(this.audioChunks);
            this.onMicrofoneOutPut(audioBlob);
          }
          this.audioChunks = [];
        };
        return true;
      })
      .catch((e) => {
        console.error(e);
        return false;
      });
    if (!started || !this.mediaRecorder)
      throw new Error('Cannot start audio service');
    this.isStoped = false;
    while (true) {
      if (!this.isStoped) {
        this.mediaRecorder.start();
        await this.sleep(this.audioChunksBufferTime);
        this.mediaRecorder.stop();
      }
    }
  }

  private isStoped = false;

  public STOP() {
    this.isStoped = true;
  }

  public async ReproduceAudio(data: Blob): Promise<void> {
    this.audio.src = URL.createObjectURL(data);
    await this.audio.play();
  }

  private sleep(timeMs: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, timeMs));
  }
}
