import { ThisReceiver } from '@angular/compiler';
import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { Router } from '@angular/router';
import { Converter } from 'src/app/Helpers/Converter';
import { Group } from 'src/app/Models/GroupModels/Group';
import { GroupMember } from 'src/app/Models/Users/UserGroup';
import { UserLogged } from 'src/app/Models/Users/UserLogged';
import { AudioService } from 'src/app/Services/AudioService';
import { GroupService } from 'src/app/Services/Group.service';
import { GroupMemberService } from 'src/app/Services/GroupMember.service';
import { LoaddingService } from 'src/app/Services/loadding.service';
import { SalkyWebSocket } from 'src/app/Services/SalykWsClient.service';
import { ShowInfoService } from 'src/app/Services/show-info.service';
import { StorageService } from 'src/app/Services/storage.service';
import { UserService } from 'src/app/Services/UserService.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-user-list',
  templateUrl: './contacts-list.component.html',
  styleUrls: ['./contacts-list.component.scss'],
})
export class ContactsListComponent implements OnInit {
  public selectedGroup?: Group;
  public _ContextMenuSelectedGroup?: Group;
  public currentUser: UserLogged = {} as UserLogged;
  constructor(
    public audioService: AudioService,
    public userService: UserService,
    public groupService: GroupService,
    public groupMemberService: GroupMemberService,
    public router: Router,
    public storageService: StorageService,
    public chatService: SalkyWebSocket,
    public loaddingService: LoaddingService,
    private show: ShowInfoService
  ) {}
  async ngOnInit(): Promise<void> {
    this.userService.currentUser$.subscribe({
      next:(usr) =>{
        this.currentUser = usr;
      }
    })
    this.groupService.setEvents();
    this.groupMemberService.setEvents();
    this.audioService.START();
  }

  public isHide = false;
  public change() {
    this.isHide = !this.isHide;
  }

  private _contextMenuSelectedMember?: GroupMember;

  public set ContextMenuSelectedGroup(Group: Group | undefined) {
    this._ContextMenuSelectedGroup = Group;
    if (Group) {
      this.groupMemberService.GetLoggedMemberOfGroup(Group.id).subscribe({
        next: (member) => {
          this._contextMenuSelectedMember = member;
        },
      });
    }
  }
  changeUserPicture(event : any){
    Converter.BlobToBase64(event.files[0], (base64) => {
      this.userService.ChangeUserPicture(base64).subscribe();
    });
  }

  public get ContextMenuSelectedGroup(): Group | undefined {
    return this._ContextMenuSelectedGroup;
  }

  public get CanDeleteGroup(): boolean {
    return (
      this._ContextMenuSelectedGroup?.id ===
        this._contextMenuSelectedMember?.groupId &&
      this._contextMenuSelectedMember?.userId ===
        this._ContextMenuSelectedGroup?.ownerId
    );
  }

  public DeleteGroup() {
    this.show.show(
      'Tem certeza que deseja remover este groupo ?',
      'A remoção do grupo é irreversível, todas mensagens serão perdidas.',
      [
        {
          ClickHandle: () => {
            if (this.ContextMenuSelectedGroup)
              this.groupService.deleteGroup(this.ContextMenuSelectedGroup.id);
            this.show.hide();
          },
          Style: 'max-width:30%;',
          Text: 'Remover',
          Class: 'btn btn-danger',
        },
        {
          ClickHandle: () => {
            this.show.hide();
          },
          Style: 'max-width:60%;',
          Text: 'Manter',
          Class: 'btn btn-success',
        },
      ]
    );
  }
  public LeaveGroup() {
    if (this.ContextMenuSelectedGroup)
      this.groupMemberService
        .GetLoggedMemberOfGroup(this.ContextMenuSelectedGroup.id)
        .subscribe({
          next: (member) => {
            this.groupMemberService.removeMember(member.id);
          },
        });
  }
}
