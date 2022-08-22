import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  Input,
  NgZone,
  OnInit,
  Renderer2,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { GroupMember } from 'src/app/Models/Users/UserGroup';
import { GroupMemberService } from 'src/app/Services/GroupMember.service';

@Component({
  selector: 'app-group-members-list',
  templateUrl: './group-members-list.component.html',
  styleUrls: ['./group-members-list.component.scss'],
})
export class GroupMemberListComponent implements OnInit {
  @Input() groupMembers: GroupMember[] = [];
  @Input() currentMember?: GroupMember;
  @Input() GroupId?: string;
  open = false;

  @ViewChild('contextMenu') contextMenu?: ElementRef;
  lastEvent?: MouseEvent;
  SelectedContextMember?: GroupMember;

  constructor(
    private groupMemberS: GroupMemberService,
  ) {

  }
  ngOnInit(): void { }

  ngAfterViewInit(): void { }

  public get UserCanRemoveSelectedContextMember(): boolean {
    if (!this.currentMember) return false;
    if (!this.currentMember.groupRole) return false;
    if (!this.SelectedContextMember) return false;

    return (
      this.currentMember.groupRole.groupPermissions.canRemoveOtherUsers &&
      this.currentMember.id !== this.SelectedContextMember.id
    );
  }

  public get CanShowContextMenu(): boolean {
    return this.UserCanRemoveSelectedContextMember;
  }

  public RemoveSelectedContextMember() {
    if (this.SelectedContextMember && this.UserCanRemoveSelectedContextMember)
      this.groupMemberS.removeMember(this.SelectedContextMember.id);
  }

}
