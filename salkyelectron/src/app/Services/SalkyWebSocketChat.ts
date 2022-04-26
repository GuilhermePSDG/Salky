import { Injectable, OnInit } from '@angular/core';
import { Contact } from '../Models/Contact';
import { MessageAdd } from '../Models/MessageAdd';
import { MessageServer } from '../Models/MessageWsServer';
import { UserLogged } from '../Models/UserLogged';
import { ContactVM } from '../ModelsView/User/ContactVM';
import { SalykWsClientService } from './SalykWsClient.service';

@Injectable({
  providedIn: 'root',
})
export class SalkyWebSocketChat extends SalykWsClientService {
  public SendMessage(msg: string, contact: ContactVM) {}
  private routesStarted = false;
  public override connect(user: UserLogged): Promise<void> {
    return super.connect(user).finally(() => {
    });
  }


  public sendMessage(
   msg : MessageAdd
  ) {

    this.sendMessageServer({
      DataJson: JSON.stringify(msg),
      PathString: 'message',
      Method: 'REDIRECT',
      SenderIntentifier: this.currentUser.id,
    });
  }
}
