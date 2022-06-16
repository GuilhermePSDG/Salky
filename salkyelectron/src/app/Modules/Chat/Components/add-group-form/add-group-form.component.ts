import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { GroupService } from 'src/app/Services/Group.service';
import { StorageService } from 'src/app/Services/storage.service';

@Component({
  selector: 'app-add-group-form',
  templateUrl: './add-group-form.component.html',
  styleUrls: ['./add-group-form.component.scss'],
})
export class AddGroupFormComponent implements OnInit {
  @Output() OnComplete = new EventEmitter<boolean>();
  value: string = '';
  constructor(
    private groupService: GroupService,
    private router: Router
  ) {}

  public get isValid(): boolean {
    return this.value.length > 0 && this.value.length < 20;
  }

  ngOnInit(): void {
    var sub = this.groupService.onGroupCreated((x) => {
      this.OnComplete.emit(true);
      this.router.navigateByUrl('/main/group/' + x.id);
      sub.unsubscribe();
    });
  }
  public CreateGroup() {
    this.groupService.createGroup(this.value);
  }
}
