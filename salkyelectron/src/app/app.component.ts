import {
  Component,
  HostListener,
  OnInit,
} from '@angular/core';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  @HostListener('contextmenu', ['$event'])
  onRightClick(event: any) {
    if (event) {
      event.preventDefault();
      this.e = event;
    }
  }
  e: any;
  constructor() {
  }
 
  async ngOnInit(): Promise<void> { }
  sleep(ms: number) {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }

}
