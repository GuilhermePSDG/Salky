import { Injectable } from '@angular/core';
import { ReplaySubject, takeLast } from 'rxjs';
import { Group } from '../Models/GroupModels/Group';
import { AudioState } from '../Models/AudioState';

export class AudioService {
  public onMicrofoneOutPut: (base64: string) => void = (_) => { };
  mediaRecorder?: MediaRecorder;
  audioChunks: any[] = [];
  time: number = 300;

  constructor() {
    this.mainFunction(this.time);
  }

  public ReproduceAudio(data: any) {
    var audio = new Audio(data);
    audio.play();
  }

  emiter: any;

  mainFunction(time: number) {
    navigator.mediaDevices.getUserMedia({ audio: true }).then((stream) => {
      var mediaRecorder = new MediaRecorder(stream);
      mediaRecorder.start();

      var audioChunks: any[] = [];

      mediaRecorder.addEventListener("dataavailable", function (event) {
        audioChunks.push(event.data);
      });

      const callBack = this.onMicrofoneOutPut;
      mediaRecorder.addEventListener("stop", function () {
        var audioBlob = new Blob(audioChunks);

        audioChunks = [];

        var fileReader = new FileReader();
        fileReader.readAsDataURL(audioBlob);

        const handler = callBack;
        fileReader.onloadend = function () {
          var base64String = fileReader.result as any;
          var newData = base64String.split(";");
          newData[0] = "data:audio/ogg;";
          var result = newData[0] + newData[1];
          handler(result);
        };

        mediaRecorder.start();

        setTimeout(function () {
          mediaRecorder.stop();
        }, time);
      });

      setTimeout(function () {
        mediaRecorder.stop();
      }, time);
    });
  }

}
