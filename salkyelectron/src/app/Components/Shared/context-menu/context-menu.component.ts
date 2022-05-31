import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  HostListener,
  Input,
  NgZone,
  OnInit,
  Renderer2,
  ViewChild,
} from '@angular/core';
import { ForceSingle } from 'src/app/Helpers/ContextMenuForceSingleInstance';
import { ContextMenuItem } from 'src/app/Models/ContextMenuItem';
import { GroupMember } from 'src/app/Models/Users/UserGroup';

@Component({
  selector: 'app-context-menu',
  templateUrl: './context-menu.component.html',
  styleUrls: ['./context-menu.component.scss'],
})
export class ContextMenuComponent implements OnInit {
  @ViewChild('contextMenu') contextMenu?: ElementRef;
  @Input() lastEvent?: MouseEvent;
  @Input() set IsOpen(value: boolean) {
    if (value) {
      this.singleService.InformOpen(() => this.hide());
    }
    this.isOpen = value;
    this.cdref.detectChanges();
  }
  isOpen: boolean = false;
  constructor(
    private singleService: ForceSingle,
    private cdref: ChangeDetectorRef
  ) {}

  get IsOpen(): boolean {
    return this.isOpen;
  }
  ngOnInit(): void {
    this.onClickOutElement('.context-menu', () => {
      this.hide();
    });
  }

  contextMenuItems: ContextMenuItem[] = [];

  public get getX(): number {
    if (!this.lastEvent) return 0;
    if (!this.contextMenu) return 0;
    var x = 0;

    var elWidth = this.contextMenu.nativeElement.clientWidth;
    var clickX = this.lastEvent.clientX;
    var wWidth = window.innerWidth;

    if (clickX + elWidth > wWidth) x = clickX - elWidth;
    else x = clickX;
    this.cdref.markForCheck();
    return x;
  }
  public get getY(): number {
    if (!this.lastEvent) return 0;
    if (!this.contextMenu) return 0;

    var elHeight = this.contextMenu.nativeElement.clientHeight;
    var wHeight = window.innerHeight;
    var clickHeight = this.lastEvent.clientY;

    var y = 0;
    if (clickHeight + elHeight + 20 < wHeight) y = clickHeight;
    else y = clickHeight - elHeight;
    this.cdref.markForCheck();
    return y;
  }

  public show(MouseEvent: MouseEvent) {
    this.lastEvent = MouseEvent;
    this.IsOpen = true;
    this.cdref.detectChanges();
  }

  public hide() {
    this.singleService.InformClose();
    this.IsOpen = false;
    this.cdref.detectChanges();
  }
  onClickOutElement(selector: string, onClickedOut: () => void) {
    document.addEventListener('click', function (event: any) {
      var element = document.querySelector(selector) as any;
      if (element)
        if (!element.contains(event.target)) {
          onClickedOut();
        }
    });
  }
}
