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

  public show(Title :string ,message: string,buttons?: ShowInfoButton[],CanClickOutToClose = true) {
    this.control?.Show(message, Title,buttons ?? [],CanClickOutToClose);
  }

  public showMsg(Title :string,message: string,CanClickOutToClose = true) {
    this.show(Title,message, this.Styles['default'].btns,CanClickOutToClose);
  }

  public showStyle(Title :string,styleName:string,CanClickOutToClose = true) {
    var style = this.Styles[styleName];
    this.show(Title,style.message, style.btns,CanClickOutToClose);
  }
  public showStyleWithOtherMsg(Title :string,message: string,styleName: string,CanClickOutToClose = true) {
    var style = this.Styles[styleName];
    this.show(Title,message,style.btns,CanClickOutToClose);
  }

  public hide() {
    this.control?.Hide();
  }
}
