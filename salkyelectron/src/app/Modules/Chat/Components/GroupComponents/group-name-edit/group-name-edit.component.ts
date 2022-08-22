import { Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { GroupPermissions } from 'src/app/Models/Users/UserGroup';
import { GroupService } from 'src/app/Services/Group.service';
import { EventsDestroyables } from 'src/app/Services/WebSocketBaseService';

@Component({
  selector: 'app-group-name-edit',
  templateUrl: './group-name-edit.component.html',
  styleUrls: ['./group-name-edit.component.scss']
})
export class GrupoEditDisplayComponent extends EventsDestroyables implements OnInit, OnDestroy {

  groupId: string = '';
  @Input() set GroupId(gpId: string) {
    this.groupId = gpId;
    this.viewGroupname = this.groupService.findGroup(this.groupId)?.name ?? '';
  }
  @Input('GroupPermissions') groupPermissions?: GroupPermissions = undefined;

  constructor(private groupService: GroupService) {
    super();
    this.AppendToDestroy(
      this.groupService.onGroupNameChanged((objt) => {
        if (objt.groupId === this.groupId) {
          this.viewGroupname = objt.newGroupName;
        }
      })
    );
  }

  ngOnDestroy() {
    this.Destroy();
  }
  ngOnInit(): void {
  }

  @ViewChild('spanGroupName') spanGroupName?: ElementRef;

  public activeEdit: boolean = false;

  startEdit() {
    this.activeEdit = true;
    this.spanGroupName?.nativeElement.focus();
  }
  viewGroupname: string | null = null;

  cancelEdit() {
    if (this.groupId)
      this.viewGroupname = this.groupService.findGroup(this.groupId)?.name ?? '';
    this.finishEdit();
  }
  saveEdit() {
    if (
      this.groupId &&
      this.viewGroupname &&
      this.groupNameIsValid &&
      this.viewGroupname !== this.groupService.findGroup(this.groupId)!.name
    ) {
      this.groupService.changeGroupName(this.groupId, this.viewGroupname);
      this.finishEdit();
    } else {
      this.cancelEdit();
    }
  }
  private finishEdit() {
    this.activeEdit = false;
    setTimeout(() => {
      if (this.groupId)
        this.viewGroupname =
          this.groupService.findGroup(this.groupId)?.name ?? '';
    }, 1000);
  }

  get groupNameIsValid(): boolean {
    return !!this.viewGroupname && this.viewGroupname.length > 0;
  }


}
