import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserLogged } from 'src/app/Models/Users/UserLogged';
import { AudioService } from 'src/app/Services/AudioService';
import { CallService } from 'src/app/Services/call.service';
import { GroupService } from 'src/app/Services/Group.service';
import { LoaddingService } from 'src/app/Services/loadding.service';
import { Destroyable } from 'src/app/Services/SalkyEvents';
import { SalkyWebSocket } from 'src/app/Services/SalykWsClient.service';
import { ShowInfoService } from 'src/app/Services/show-info.service';
import { UserService } from 'src/app/Services/UserService.service';
import { EventsDestroyables } from 'src/app/Services/WebSocketBaseService';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent extends EventsDestroyables implements OnInit {
  constructor(
    public audioService: AudioService,
    public salkyWsClient: SalkyWebSocket,
    public userService: UserService,
    public contactService: GroupService,
    public router: Router,
    public loaddingService: LoaddingService,
    private show: ShowInfoService,
    private groupService: GroupService
  ) {
    super();
    this.userService.currentUser$.subscribe({
      next: (usr) => (this.loggedUser = usr),
    });
  }

  position = {} as any;
  loggedUser: UserLogged = {} as any;

  public canShowUserList() {
    return !(
      this.router.url.includes('user/login') ||
      this.router.url.includes('user/register')
    );
  }

  subs: Destroyable[] = [];
  async ngOnInit(): Promise<void> {
    var sub1 = this.userService.currentUser$.subscribe({
      next: async (usr) => await this.salkyWsClient.connectIfnot(usr),
    });
    var sub2 = this.salkyWsClient.On('close', '*').Build<any>(async (err) => {
      console.log('closed');
      console.log(err);
      try {
        switch (err.reason) {
          case 'DuplicatedConnection':
            this.show.show(
              'Hummm.. Algo não está certo..',
              'Parece que você se logou em outro lugar.',
              [
                {
                  ClickHandle: () => {
                    window.location.href = 'https://google.com';
                  },
                  Style: 'background-color:#212326; width:30%;color:white;',
                  Text: 'Sair',
                },
                {
                  ClickHandle: () => {
                    this.salkyWsClient.connectIfnot(this.loggedUser);
                    this.show.hide();
                  },
                  Style: 'background-color:#47586D; width:60%;color:white;',
                  Text: 'Continuar aqui.',
                },
              ],
              false
            );
            break;
          case 'Normal' || 'HandShakeProblems':
            this.router.navigateByUrl('/user/logout');
            break;
          default:
            this.router.navigateByUrl('/user/logout');
            // this.salkyWsClient.connectIfnot(usr);
            break;
        }
      } catch { }
    });
    this.AppendManyToDestroy(sub1, sub2);
  }
  ngOnDestroy(): void {
    this.Destroy();
  }

  protected sleep(timeMs: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, timeMs));
  }
}
