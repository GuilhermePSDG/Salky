import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';
import { delay } from 'rxjs';
import { ContactVM } from './ModelsView/User/ContactVM';
import { MessageVM } from './ModelsView/MessageVM';
import { UserVM } from './ModelsView/User/UserVM';
import { AudioService } from './Services/AudioService';
import { User } from './Models/UserWsServer';
import { SalykWsClientService } from './Services/SalykWsClient.service';
import { UserService } from './Services/UserService.service';
import { ContactService } from './Services/ContactService.service';
import { Router } from '@angular/router';
import { SalkyWebSocketChat } from './Services/SalkyWebSocketChat';
import { UserLogged } from './Models/UserLogged';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'Salky';

  private user?: UserLogged | null;

  constructor(
    public audioService: AudioService,
    public salkyWsClient: SalkyWebSocketChat,
    public userService: UserService,
    public contactService: ContactService,
    public router: Router
  ) {
    audioService.Start();
  }

  public canShowUserList() {
    return !(
      this.router.url.includes('user/login') ||
      this.router.url.includes('user/register')
    );
  }
  public ngOnInit(): void {
    this.user = this.userService.getUserFromStorage();
    this.userService.currentUser$.subscribe({
      next: async (usr) => {
        if (usr) {
          if (usr && this.user?.id !== usr.id) {
            this.user = usr;
            if(this.salkyWsClient.IsConnected) await this.salkyWsClient.close()
            console.log("connectiong  ")
            await this.salkyWsClient.connect(usr);
          }
        }
      },
    });
    console.log(this.user)
    if (this.user) this.salkyWsClient.connect(this.user);
  }

}
