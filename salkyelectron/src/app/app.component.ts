import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ComponentFactoryResolver,
  HostListener,
  OnInit,
} from '@angular/core';
import { ShowInfoComponent } from './Components/Shared/show-info/show-info.component';
import { ShowInfoService } from './Services/show-info.service';
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
  e : any;
  constructor() {

  }
  async ngOnInit(): Promise<void> {}
  sleep(ms: number) {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }

}
