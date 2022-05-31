import { HttpClient } from '@angular/common/http';
import { ThisReceiver } from '@angular/compiler';
import { Component, Input, OnInit } from '@angular/core';
import {
  DomSanitizer,
  SafeResourceUrl,
  SafeUrl,
} from '@angular/platform-browser';
import { take } from 'rxjs';
import { Message } from 'src/app/Models/Message';
import { ShowInfoService } from 'src/app/Services/show-info.service';

@Component({
  selector: 'app-message-render',
  templateUrl: './message-render.component.html',
  styleUrls: ['./message-render.component.scss'],
})
export class MessageRenderComponent implements OnInit {
  public youtubeEmbedUrl: SafeUrl | null = null;
  public imgSource: SafeUrl | null = null;
  public isHighLight = false;

  @Input() msg: Message = {} as Message;

  constructor(
    private domSaniter: DomSanitizer,
    private http: HttpClient,
    private show: ShowInfoService
  ) {}

  ngOnInit(): void {
    if (this.msg.content.startsWith('##') && this.msg.content.endsWith('##')) {
      this.isHighLight = true;
      this.msg.embeds.forEach(
        (x) => (x.content = x.content.replace(/##/g, ''))
      );
      console.log(this.msg.embeds);
    }

    this.msg.embeds.forEach((x) => {
      if (x.renderType === 'gif') {
        this.imgSource = this.sanitizeURL(x.content);
      } else if (x.renderType === 'youtubeEmbed') {
        this.youtubeEmbedUrl = this.sanitizeURL(x.content);
      } else if (x.renderType === 'url') {
      } else {
      }
    });
  }

  public NavigateUrl(url: string) {
    this.show.show(
      'Pagina extena..',
      'Tem certeza que deseja que deseja acessar este site ?',
      [
        {
          ClickHandle: () => {
            window.open(url, '_blank')?.focus();
            this.show.hide();
          },
          Style: 'width:50px;max-height:40px; padding:0 5px;max-height: 28px;',
          Class: 'btn btn-danger',
          Text: 'Ir',
        },
        {
          ClickHandle: () => this.show.hide(),
          Style: 'width:120px;max-height:40px; padding:0 5px;max-height: 28px;',
          Class: 'btn btn-success',
          Text: 'Não',
        },
      ]
    );
  }
  public sanitizeURL(value: string): SafeUrl {
    return this.domSaniter.bypassSecurityTrustResourceUrl(value);
  }
}
interface MetaTag {
  property: string;
  content: string;
}
