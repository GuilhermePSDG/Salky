import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { MessageService } from 'src/app/Services/message.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-send-message-input',
  templateUrl: './send-message-input.component.html',
  styleUrls: ['./send-message-input.component.scss'],
})
export class SendMessageBoxComponent implements OnInit {
  public inputText: string = '';
  @Input() Validator: (x: string) => boolean = (x) =>
    x != null && x != undefined && x.length > 0;

  constructor(
    public sanitizer: DomSanitizer,
    private messageService: MessageService
  ) { }
  public gifIsOpen = false;
  public emojiIsOpen = false;
  public imagesToSend: string[] = [];

  ngOnInit(): void {
    document.addEventListener('click', (event: any) => {
      let ignoreClickOnMeElement = document.querySelector('.gifContainer');
      if (!ignoreClickOnMeElement) return;
      var isClickInsideElement = ignoreClickOnMeElement.contains(event.target);
      if (!isClickInsideElement) {
        this.gifIsOpen = false;
      }
    });

    window.addEventListener('paste', (e: any) => {
      this.onFilePaste(e.clipboardData.files);
    });
  }
  @Output() onSendMessageRequested: EventEmitter<string> =
    new EventEmitter<string>();

  public onKeyPres(x: KeyboardEvent): void {
    if (x.key === 'Enter') {
      this.sendMessage();
    }
  }

  public sendEmoji(emoji: string) {
    document.querySelectorAll('input').forEach((q) => {
      if (q.classList.contains('inputMessage')) {
        q.focus();
        return;
      }
    });
    this.inputText += emoji;
  }

  removeImage(index: number) {
    this.imagesToSend.splice(index, 1);
  }
  sendGif(ev: any) {
    if (this.Validator(ev)) {
      this.onSendMessageRequested.emit(ev);
    }
    this.gifIsOpen = false;
  }

  pushBlobImage(blob: Blob) {
    console.log('file are not implemented');
    return;
    const reader = new FileReader();
    reader.onload = (event: any) => {
      this.imagesToSend.push(event.target.result);
    };
    reader.readAsDataURL(blob);
  }

  onFileChange(ev: any): void {
    try {
      this.pushBlobImage(ev[0]);
    } catch { }
  }

  async appendClipBoardContent(): Promise<void> {
    var value = await navigator.clipboard.readText();
    (await navigator.clipboard.read()).forEach((clipItem) => {
      var typeIndex = clipItem.types.findIndex((x) => x.includes('image/'));
      if (typeIndex !== -1) {
        clipItem.getType(clipItem.types[typeIndex]).then((blob) => {
          this.pushBlobImage(blob);
        });
      }
    });

    this.inputText += value;
  }

  onFilePaste(ev: any) {
    this.onFileChange(ev);
  }

  public sendMessage(): void {
    if (this.Validator(this.inputText)) {
      this.onSendMessageRequested.emit(this.inputText);
      this.inputText = '';
    }

    if (this.imagesToSend.length > 0) {
      this.imagesToSend.forEach((img) => {
        this.messageService.sendBase64Image(img).subscribe({
          next: (imgRelativePath: string) => {
            this.onSendMessageRequested.emit(imgRelativePath);
          },
        });
      });
    }
  }
}
