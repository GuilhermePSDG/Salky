import { Injectable } from '@angular/core';
import {
  ShowInfoButton,
  ShowInfoComponent,
  ShowInfoControl,
} from '../Components/Shared/show-info/show-info.component';

@Injectable({
  providedIn: 'root',
})
export class ShowInfoService {
  private control?: ShowInfoControl;

  private Styles: {
    [key: string]: {
      message: string;
      btns: ShowInfoButton[];
      title? : string,
    };
  };

  constructor() {
    this.Styles = {
      ['default']: {
        btns: [
          {
            ClickHandle: () => this.hide(),
            Style: 'background-color:red;',
            Text: 'Fechar',
          },
        ],
        message: 'Mensagem padrão.',
      },
      ['danger'] : {
        btns: [
          {
            ClickHandle: () => this.hide(),
            Style: 'background-color:red;',
            Text: 'Fechar',
          },
        ],
        message: 'Mensagem padrão.',
      }
      ,
      ['sucess'] : {
        btns: [
          {
            ClickHandle: () => this.hide(),
            Style: 'background-color:green;',
            Text: 'Fechar',
          },
        ],
        message: 'Mensagem padrão.',
      }
      ,
      ['alert'] : {
        btns: [
          {
            ClickHandle: () => this.hide(),
            Style: 'background-color:yellow; color:black;',
            Text: 'Fechar',
          },
        ],
        message: 'Mensagem padrão.',
      }




    };
  }

  public createBind(control: ShowInfoControl) {
    this.control = control;
  }

  public createStyle(
    styleName: string,
    Message: string,
    Title:string,
    buttons: ShowInfoButton[]
  ) {
    this.Styles[styleName] = {
      btns: buttons,
      message: Message,
      title :Title
    };
  }

  public show(Title :string ,message: string,buttons?: ShowInfoButton[]) {
    this.control?.Show(message, Title,buttons);
  }

  public showMsg(Title :string,message: string) {
    this.show(Title,message, this.Styles['default'].btns);
  }

  public showStyle(Title :string,styleName:string) {
    var style = this.Styles[styleName];
    this.show(Title,style.message, style.btns);
  }
  public showStyleWithOtherMsg(Title :string,message: string,styleName: string,) {
    var style = this.Styles[styleName];
    this.show(Title,message,style.btns);
  }

  public hide() {
    this.control?.Hide();
  }
}
