import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Contact } from 'src/app/Models/Contact';
import { Message } from 'src/app/Models/Message';
import { MessageAdd } from 'src/app/Models/MessageAdd';
import { UserLogged } from 'src/app/Models/UserLogged';
import { User } from 'src/app/Models/UserWsServer';
import { MessageVM } from 'src/app/ModelsView/MessageVM';
import { ContactVM } from 'src/app/ModelsView/User/ContactVM';
import { UserVM } from 'src/app/ModelsView/User/UserVM';
import { ContactService } from 'src/app/Services/ContactService.service';
import { MessageService } from 'src/app/Services/message.service';
import { SalkyWebSocketChat } from 'src/app/Services/SalkyWebSocketChat';
import { UserService } from 'src/app/Services/UserService.service';

@Component({
  selector: 'app-chat-box',
  templateUrl: './chat-box.component.html',
  styleUrls: ['./chat-box.component.scss'],
})
export class ChatBoxComponent implements OnInit {
  public CurrentContact: Contact = {} as Contact;
  public Messages: Message[] = [];
  public LoggedUser: UserLogged = {} as UserLogged;

  public messageIsOwner(msg: Message): boolean {
    return msg.userSenderId === this.LoggedUser.id;
  }

  constructor(
    private activatedRouter: ActivatedRoute,
    private userService: UserService,
    private contactService: ContactService,
    private chatService: SalkyWebSocketChat,
    private messageService: MessageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.activatedRouter.paramMap.subscribe((params: ParamMap) => {
      const usr = this.userService.getUserFromStorage();
      if (usr) {
        this.LoggedUser = usr;
      } else {
        this.router.navigateByUrl('user/login');
      }
      var contactId = params.get('contactId');
      if (contactId) {
        var contact = this.contactService.findInLocalStorage(contactId);
        if (contact) this.CurrentContact = contact;
        this.messageService.getMessages(contactId).subscribe({
          next: (msgs: Message[]) => {
            this.Messages = msgs;
            this.forceScrolDown();
          },
          error: (err: any) => {
            console.error('Não foi possivel obter as mensagens\n' + err);
          },
          complete: () => {},
        });
      } else {
        console.error('Não foi possivel obter o id da rota');
      }
    });
    this.startRotues();
    console.log(this.chatService);
    this.onElementScrolHeightChanged(
      document.querySelector('.chatBoxMessage'),
      (h: any) => {
        this.forceScrolDown();
      }
    );
  }

  public receiveMessage(msg: Message) {
    console.log("message received");
    console.log(msg);
    console.log("all messages")
    console.log(this.Messages);
    this.Messages.push(msg);
    // this.forceScrolDown();
  }
  public sendMessage(content: string) {
    if(this.CurrentContact){
      var msg = {
        ContactId: this.CurrentContact.contactId,
        Content: content,
        UserContactId: this.CurrentContact.userContactId,
      } as MessageAdd;
      this.chatService.sendMessage(msg)
    }

  }

  private startRotues() {
    this.chatService.On('message', 'post').Do((f) => {
      if (f.DataJson) {
        var msg = JSON.parse(f.DataJson);
        var parsedres =  {
          id : msg.Id,
          content : msg.Content,
          userSenderId : msg.UserSenderId,
          userReceiverId : msg.UserReceiverId,
        } as Message;
        this.receiveMessage(parsedres);
      }
    }).ThenDoAgain();
  }

  //Refatorar e ponderar o horario da mensagem
  public canShowUserInfo(index: number): boolean {
    if (index === 0) {
      return true;
    } else if (
      this.Messages[index - 1].userSenderId ===
      this.Messages[index].userSenderId
    ) {
      return false;
    } else {
      return true;
    }
  }

  public forceScrolDown() {
    const chatBOx = document.querySelector('.chatBoxMessage');
    chatBOx?.scroll(0, chatBOx.scrollHeight * 2);
  }

  onElementScrolHeightChanged(elm: any, callback: any) {
    var lastHeight = elm.scrollHeight,
      newHeight;

    (function run() {
      newHeight = elm.scrollHeight;
      if (lastHeight != newHeight) callback(newHeight);
      lastHeight = newHeight;

      if (elm.onElementHeightChangeTimer)
        clearTimeout(elm.onElementHeightChangeTimer);

      elm.onElementHeightChangeTimer = setTimeout(run, 200);
    })();
  }
}
