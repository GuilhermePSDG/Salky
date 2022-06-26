import { Component, OnInit } from '@angular/core';
// const { desktopCapturer, remote } = require('electron');
// const { dialog, Menu } = remote;


@Component({
  selector: 'app-scren-capture',
  templateUrl: './scren-capture.component.html',
  styleUrls: ['./scren-capture.component.scss'],
})
export class ScrenCaptureComponent implements OnInit {
  videoSelectBtn = document.getElementById('videoSelectBtn');
  mediaRecorder?: MediaRecorder;
  public videoElement : HTMLVideoElement = {} as HTMLVideoElement;
  public recordedChunks: Blob[] = [];
  startBtn = document.getElementById('startBtn');
  stopBtn = document.getElementById('stopBtn');

  public stream? : MediaStream;

  constructor() {
    var element =document.getElementById('video');
    if(element)
    console.log("video setted")
      this.videoElement = element as HTMLVideoElement;
    }

  ngOnInit(): void {

    this.selectSource({id : 1 , name : "desktop"})

    console.log("ok");
    console.log(this.mediaRecorder);

    if (
      this.videoSelectBtn &&
      this.startBtn &&
      this.stopBtn &&
      this.mediaRecorder
    ) {
      this.videoSelectBtn.onclick = this.getVideoSources;
      this.startBtn.onclick = (e) => {
        console.log(this.mediaRecorder);
        this.mediaRecorder?.start();
        if (this.startBtn) {
          this.startBtn.classList.add('is-danger');
          this.startBtn.innerText = 'Recording';
        }
      };
      this.stopBtn.onclick = (e) => {
        this.mediaRecorder?.stop();
        if (this.startBtn) {
          this.startBtn.classList.remove('is-danger');
          this.startBtn.innerText = 'Start';
        }
      };
    }
  }



  async startCapture() {}

  async selectSource(source: any) {
    if (!this.videoElement) {
      throw new Error('videoElement cannot be null');
    }
    if (!source) {
      throw new Error('source cannot be null');
    }
    if (this.videoSelectBtn) this.videoSelectBtn.innerText = source.name;

    const constraints = {
      audio: false,
      video: {
        mandatory: {
          chromeMediaSource: 'desktop',
          chromeMediaSourceId: 1,
        },
      },
    } as any;

    // Create a Stream
    const stream = await navigator.mediaDevices.getUserMedia(constraints);
    this.stream = stream;
    // Preview the source in a video element
    this.videoElement.srcObject = stream;
    this.videoElement.play();

    // Create the Media Recorder
    const options = { mimeType: 'video/webm; codecs=vp9' };
    this.mediaRecorder = new MediaRecorder(stream, options);

    // Register Event Handlers
    this.mediaRecorder.ondataavailable = this.handleDataAvailable;
    this.mediaRecorder.onstop = this.handleStop;

    // Updates the UI
  }

  async getVideoSources() {
    // const inputSources = await desktopCapturer.getSources({
    //   types: ['window', 'screen'],
    // });

    // const videoOptionsMenu = Menu.buildFromTemplate(
    //   inputSources.map((source: any) => {
    //     return {
    //       label: source.name,
    //       click: () => this.selectSource(source),
    //     };
    //   })
    // );
  }
  handleDataAvailable(e: BlobEvent) {
    console.log('video data available');
    this.recordedChunks.push(e.data);
  }

  // Saves the video file on stop
  async handleStop(e: Event) {
    const blob = new Blob(this.recordedChunks, {
      type: 'video/webm; codecs=vp9',
    });
  }
}
