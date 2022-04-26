export class AudioService {
  private mediaRecorder?: MediaRecorder;
  private audioChunks?: any[];
  private audioChunksBufferTime = 300;
  public ouMicrofoneOutput?: (data: string) => {};

  constructor() {}
  private async initObjs() {
    const stream = await navigator.mediaDevices.getUserMedia({
      audio: true,
    });
    this.mediaRecorder = new MediaRecorder(stream);
    this.audioChunks = [];
    this.ouMicrofoneOutput = this.ReproduceAudio;
  }

  private _microfoneMuted: boolean = false;
  private _headPhoneMuted: boolean = false;
  public get microfoneMuted(): boolean {
    return this._microfoneMuted;
  }
  public get headPhoneMuted(): boolean {
    return this._headPhoneMuted;
  }
  ChangeMicrofoneState() {
    this._microfoneMuted = !this._microfoneMuted;
  }
  ChangeHeadPhoneState() {
    this._headPhoneMuted = !this._headPhoneMuted;
  }

  public async Start(): Promise<void> {
    await this.initObjs();
    if (!this.mediaRecorder) return;
    this.mediaRecorder.ondataavailable = (x: BlobEvent) =>
      this.OnMicrofoneDataAvailable(x);
    this.mediaRecorder.onstop = () => this.OnMicrofoneListenerStop();

    while (true) {
      this.mediaRecorder.start();
      await this.sleep(this.audioChunksBufferTime);
      this.mediaRecorder.stop();
    }
  }

  private async OnMicrofoneDataAvailable(f: BlobEvent) {
    if (this.audioChunks) this.audioChunks.push(f.data);
  }
  private async OnMicrofoneListenerStop() {
    if (!this.microfoneMuted && this.ouMicrofoneOutput) {
      const audioBlob = new Blob(this.audioChunks, { type: 'audio/mpeg' });
      const audioUrl = URL.createObjectURL(audioBlob);
      this.ouMicrofoneOutput(audioUrl);
    }
    this.audioChunks = [];
  }

  private async ReproduceAudio(audioUrl: string): Promise<void> {
    if (!this.headPhoneMuted) {
      const audio = new Audio(audioUrl);
      await audio.play();
    }
  }

  private sleep(timeMs: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, timeMs));
  }
}
