import { Message } from 'src/app/Models/Message';
import {
  AfterContentChecked,
  AfterViewChecked,
  AfterViewInit,
  Component,
  ElementRef,
  OnChanges,
  OnInit,
  QueryList,
  SimpleChanges,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { UserLogged } from 'src/app/Models/Users/UserLogged';
import { Group } from 'src/app/Models/GroupModels/Group';
import { PaginationResult } from 'src/app/Models/PaginationResult';
import { MessageService } from 'src/app/Services/Message.service';
import { SalkyWebSocket } from 'src/app/Services/SalykWsClient.service';
import { StorageService as LocalStorageService } from 'src/app/Services/storage.service';
import { GroupMember } from 'src/app/Models/Users/UserGroup';
import { ActivatedRoute, Router } from '@angular/router';
import { GroupService } from 'src/app/Services/Group.service';
import { GroupMemberService } from 'src/app/Services/GroupMember.service';
import { ScrollEventListener } from 'src/app/Helpers/ScrollListener';
import { Friend } from 'src/app/Models/Users/Friend';
import { Converter } from 'src/app/Helpers/Converter';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-chat-box',
  templateUrl: './chat-box.component.html',
  styleUrls: ['./chat-box.component.scss'],
})
export class ChatBoxComponent
  implements OnInit, AfterViewInit, AfterContentChecked
{
  @ViewChild('spanGroupName') spanGroupName?: ElementRef;
  @ViewChildren('chatBoxMessage') chatBoxMessage?: QueryList<any>;

  public groupMembers: GroupMember[] = [];
  public currentGroupMember?: GroupMember;

  public MessageResults: PaginationResult<Message> = {
    currentPage: 0,
    lastPage: -1,
    totalCount: -1,
    pageSize: 100,
    results: [],
  };
  private isGettingMessage = false;
  public LoggedUser: UserLogged = {} as UserLogged;
  public activeEdit: boolean = false;
  viewGroupname: string | null = null;

  scroll: ScrollEventListener = {} as any;
  group?: Group;
  friend?: Friend;
  mode: 'group' | 'friends' = 'group';

  constructor(
    private messageService: MessageService,
    private localStorageService: LocalStorageService,
    private activeRouter: ActivatedRoute,
    private router:Router,
    private groupService: GroupService,
    private memberService: GroupMemberService
  ) {}

  ngAfterContentInit(): void {
    this.scroll = new ScrollEventListener(
      () => this.chatBoxMessage?.first?.nativeElement
    );
    this.scroll.onUserScroll = (distanceFromTop, TotalHeigh) => {
      if (distanceFromTop < TotalHeigh * 0.2) {
        this.getNextMessages('unset');
      }
    };
    this.scroll.onIncress = () => this.scrollToBottom();
  }

  subscribes : Subscription[] = [];
  async ngOnInit() {
    var sub1 = this.messageService.onMessageReceived((msg) => this.receiveMessage(msg));
    var sub2 = this.messageService.onMessageDeleted((r) =>
      this.deleteMessage(r.groupId, r.messageId)
    );
    var sub3 = this.memberService.$CurrentMember.subscribe({
      next: (member) => (this.currentGroupMember = member),
    });
    var sub4 = this.memberService.$CurrentMembers.subscribe({
      next: (members) => (this.groupMembers = members),
    });

    var sub5 = this.activeRouter.params.subscribe((value) => {
      const id = value['id'];
      if (id) {
        var group = this.groupService.findGroup(id);
        if (group) {
          this.memberService.ChangeGroup(id);
          this.group = group;
          this.viewGroupname = group.name;
          this.setMessages();
        }else{
          this.router.navigateByUrl("");
        }
      }
    });
    this.LoggedUser = this.localStorageService.CurrentUser;
    this.subscribes.push(sub1,sub2,sub3,sub4,sub5);
  }

  ngOnDestroy(): void {
    this.scroll.Dispose();
    this.subscribes.forEach(sub => sub.unsubscribe());
  }

  ngAfterViewChecked() {}
  ngAfterContentChecked(): void {}

  ngAfterViewInit(): void {
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
      if (this.chatBoxMessage)
        this.chatBoxMessage.first.nativeElement.scrollTop =
          this.chatBoxMessage.first.nativeElement.scrollHeight;
    } catch (err) {}
  }

  groupPictureChangedRequested(event: any) {
    console.log('event');
    console.log(event);
    console.log(event.files[0]);
    Converter.BlobToBase64(event.files[0], (base64) => {
      if (!this.group?.id) return;
      this.groupService.ChangeGroupPicture(this.group.id, base64).subscribe({
        next: (picture) => {
          console.log('Nova foto, path : ' + picture);
        },
      });
    });
  }

  startEdit() {
    this.activeEdit = true;
    this.spanGroupName?.nativeElement.focus();
  }
  cancelEdit() {
    if (this.group?.id)
      this.viewGroupname =
        this.groupService.findGroup(this.group?.id)?.name ?? '';
    this.finishEdit();
  }
  saveEdit() {
    if (
      this.group?.id &&
      this.viewGroupname &&
      this.groupNameIsValid &&
      this.viewGroupname !== this.groupService.findGroup(this.group?.id)?.name
    ) {
      this.groupService.changeGroupName(this.group?.id, this.viewGroupname);
      this.finishEdit();
    } else {
      console.log('not valid');
      console.log(this.viewGroupname);
      this.cancelEdit();
    }
  }
  private finishEdit() {
    this.activeEdit = false;
    setTimeout(() => {
      if (this.group?.id)
        this.viewGroupname =
          this.groupService.findGroup(this.group?.id)?.name ?? '';
    }, 1000);
  }

  get groupNameIsValid(): boolean {
    return !!this.viewGroupname && this.viewGroupname.length > 0;
  }

  delete(index: number) {
    this.messageService.deleteMessage(this.MessageResults.results[index].id);
  }

  private getNextMessages(scrollMode: 'unset' | 'goDown') {
    if (!this.group?.id) return;
    if (this.MessageResults.currentPage === 1) return;
    if (this.isGettingMessage) {
      return;
    }
    this.isGettingMessage = true;

    this.messageService
      .getMessagesOfGroup(
        this.group.id,
        this.MessageResults.currentPage - 1,
        this.MessageResults.pageSize
      )
      .subscribe({
        next: (msgs) => {
          if (msgs && msgs.results.length > 0) {
            if (scrollMode === 'unset') {
              this.scroll.onIncress = () => {};
            } else {
              this.scroll.DoOnNextHeightIncress(() => this.scrollToBottom());
            }
            msgs.results = msgs.results.concat(this.MessageResults.results);
            this.MessageResults = msgs;
          }
        },
      })
      .add(() => {
        this.isGettingMessage = false;
      });
  }
  private setMessages() {
    if (!this.group?.id) return;
    this.messageService
      .getMessagesOfGroup(this.group?.id, -1, this.MessageResults.pageSize)
      .subscribe({
        next: (msgs) => {
          this.scroll.DoOnNextHeightIncress(() => this.scrollToBottom());
          this.MessageResults = msgs;
          this.getNextMessages('goDown');
          this.scroll.DoOnNextHeightIncress(() => this.scrollToBottom());
        },
      });
  }

  public deleteMessage(groupId: string, messageId: string) {
    this.scroll.onIncress = () => {};
    if (!this.group?.id) return;
    var i = this.MessageResults.results.findIndex(
      (x) => x.id === messageId && x.groupId === groupId
    );
    if (i !== -1) this.MessageResults.results.splice(i, 1);
  }

  public receiveMessage(msg: Message) {
    if (!this.group?.id) return;
    if (msg.groupId === this.group?.id) {
      this.scroll.DoOnNextHeightIncress(() => this.scrollToBottom());
      this.MessageResults.results.push(msg);
      this.scroll.DoOnNextHeightIncress(() => this.scrollToBottom());
    }
  }

  public sendMessage(content: string) {
    if (!this.group?.id) return;
    this.messageService.sendMessage({
      content: content,
      groupId: this.group?.id,
    });
  }

  //Refazer e ponderar o horario da mensagem
  public canShowUserInfo(index: number): boolean {
    if (index === 0) {
      return true;
    } else if (
      this.MessageResults.results[index - 1].author.id ===
      this.MessageResults.results[index].author.id
    ) {
      return false;
    } else {
      return true;
    }
  }

  public goToUrl(url: string) {
    window.open(url, '_blank')?.focus();
  }
}
