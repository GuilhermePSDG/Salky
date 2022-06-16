import { animate, style, transition, trigger } from '@angular/animations';
import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { ShowInfoService } from 'src/app/Services/show-info.service';
export interface ShowInfoControl{
  Hide : () => void,
  ChangeState : () => void,
  Show : (message:string,Title : string, buttons : ShowInfoButton[],CanClickOutToClose : boolean) => void,
}
export interface ShowInfoButton{
  Text : string;
  Style : string,
  Class? : string,
  ClickHandle : (event : any) => void;
}

@Component({
  selector: 'app-show-info',
  templateUrl: './show-info.component.html',
  styleUrls: ['./show-info.component.scss'],
  animations: [
    trigger('overlay', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('200ms', style({ opacity: 0.6 })),
      ]),
      transition(':leave', [animate('200ms', style({ opacity: 0 }))]),
    ]),

    trigger('container', [
      transition(':enter', [
        style({
          transform: 'translate(-50%,-50%) scale(0)',
        } ),
        animate('20ms', style({ transform: 'translate(-50%, -50%)' })),
      ]),

      transition(':leave', [
        animate('20ms', style({ transform: 'translate(-50%,-50%) scale(0.0001)' })),
      ]),
    ]),
  ],
})

export class ShowInfoComponent implements OnInit,AfterViewInit {

  @Input() Message? : string;
  @Input() Buttons : ShowInfoButton[] = [];
  @Input() isHide : boolean = true;
  @Input() Title : string ='';
  @Input() CanClickOutToClose: boolean = true;
  constructor(private showInfoService : ShowInfoService) {
    this.showInfoService.createBind({
      ChangeState : this.ChangeState.bind(this),
      Hide : this.Hide.bind(this),
      Show : this.Show.bind(this)
    });

   }
  ngAfterViewInit(): void {
  }

  public Show(message:string, Title:string, buttons : ShowInfoButton[],CanClickOutToClose : boolean){
    console.log("show")
    if(buttons)this.Buttons = buttons
    this.Message = message;
    this.isHide = false;
    this.Title = Title;
    this.CanClickOutToClose = CanClickOutToClose;
  }

  public Hide(){
    this.isHide = true;
  }
  public ChangeState(){
    this.isHide = !this.isHide;
  }

  ngOnInit(): void {

  }

}
