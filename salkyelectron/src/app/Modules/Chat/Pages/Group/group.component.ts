import {
  AfterContentChecked, AfterContentInit, AfterViewInit,
  Component, ElementRef, OnInit, QueryList, ViewChild, ViewChildren
} from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Converter } from "src/app/Helpers/Converter";
import { ScrollEventListener } from "src/app/Helpers/ScrollListener";
import { Group } from "src/app/Models/GroupModels/Group";
import { Message } from "src/app/Models/Message";
import { PaginationResult } from "src/app/Models/PaginationResult";
import { Friend } from "src/app/Models/Users/Friend";
import { GroupMember } from "src/app/Models/Users/UserGroup";
import { UserLogged } from "src/app/Models/Users/UserLogged";
import { GroupService } from "src/app/Services/Group.service";
import { GroupMemberService } from "src/app/Services/GroupMember.service";
import { MessageService } from "src/app/Services/message.service";
import { StorageService } from "src/app/Services/storage.service";
import { EventsDestroyables } from "src/app/Services/WebSocketBaseService";

const defaultPaginationResult: PaginationResult<Message> = {
  pageIndex: -1,
  totalPages: null as any,
  totalCount: null as any,
  pageSize: 100,
  lastPage: null as any,
  results: [],
};
@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss'],
})
export class GroupComponent
  extends EventsDestroyables
  implements OnInit, AfterViewInit, AfterContentChecked, AfterContentInit, AfterViewInit {
  @ViewChildren('chatBoxMessage') chatBoxMessage?: QueryList<any>;

  public groupMembers: GroupMember[] = [];
  public currentGroupMember?: GroupMember;

  public MessageResults = defaultPaginationResult;

  private isGettingMessage = false;
  public LoggedUser: UserLogged = {} as UserLogged;

  scroll: ScrollEventListener = {} as any;
  group?: Group;
  friend?: Friend;
  mode: 'group' | 'friends' = 'group';

  constructor(
    private messageService: MessageService,
    private localStorageService: StorageService,
    private activeRouter: ActivatedRoute,
    private router: Router,
    private groupService: GroupService,
    private memberService: GroupMemberService
  ) {
    super();
  }

  ngAfterContentInit(): void {
    this.scrollToBottom();
    this.scroll = new ScrollEventListener(
      () => this.chatBoxMessage?.first?.nativeElement
    );
    setTimeout(() => {
      this.scroll.onUserScroll = (distanceFromTop, TotalHeigh) => {
        if (distanceFromTop < TotalHeigh * 0.2) {
          this.getNextMessages('unset');
        }
      };
    }, 1000)
  }

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
      const groupId = value['id'];
      if (groupId) {
        var group = this.groupService.findGroup(groupId);
        if (group) {
          this.groupService.startListenerGroup(groupId);
          this.memberService.ChangeGroup(groupId);
          this.group = group;
          this.MessageResults = defaultPaginationResult;
          this.getNextMessages('goDown');
        } else {
          this.router.navigateByUrl("");
        }
      }
    });
    this.AppendManyToDestroy(sub1, sub2, sub3, sub4, sub5, () => {
      this.groupService.startListenerGroup(undefined);
    });
    this.LoggedUser = this.localStorageService.CurrentUser;
  }

  ngOnDestroy(): void {
    this.Destroy();
  }

  ngAfterViewChecked() { }
  ngAfterContentChecked(): void {
  }

  ngAfterViewInit(): void {
    this.scrollToBottom();
  }

  scrollToBottom(delay = 100): void {
    setTimeout(() => {
      try {
        if (this.chatBoxMessage)
          this.chatBoxMessage.first.nativeElement.scroll(0,
            this.chatBoxMessage.first.nativeElement.scrollHeight * 10);
      } catch (err) { }
    }, delay);
  }

  groupPictureChangedRequested(event: any) {
    Converter.BlobToBase64(event.files[0], (base64) => {
      if (!this.group?.id) return;
      this.groupService.ChangeGroupPicture(this.group.id, base64).subscribe({
        next: (picture) => {
          console.log('Nova foto, path : ' + picture);
        },
      });
    });
  }

  delete(index: number) {
    this.messageService.deleteMessage(this.MessageResults.results[index].id);
  }

  private getNextMessages(scrollMode: 'unset' | 'goDown') {
    if (!this.group?.id) return;
    if (this.isGettingMessage) return;
    console.log(this.MessageResults);
    if((this.MessageResults.pageIndex+1) === this.MessageResults.totalPages) return;
    this.isGettingMessage = true;

    this.messageService
      .getMessagesOfGroup(
        this.group.id,
        (this.MessageResults.pageIndex + 1),
        this.MessageResults.pageSize
      )
      .subscribe({
        next: (msgs) => {
          if (msgs && msgs.results.length > 0) {

            msgs.results = msgs.results.concat(this.MessageResults.results);
            this.MessageResults = msgs;
            if (scrollMode === 'goDown') {
              this.scrollToBottom();
            }
          }
        },
      })
      .add(() => {
        this.isGettingMessage = false;
      });
  }
  // private setMessages() {
  //   if (!this.group?.id) return;
  //   this.messageService
  //     .getMessagesOfGroup(this.group.id, -1, this.MessageResults.pageSize)
  //     .subscribe({
  //       next: (msgs) => {
  //         this.MessageResults = msgs;
  //         this.scrollToBottom(250);
  //       },
  //     });
  // }

  public deleteMessage(groupId: string, messageId: string) {
    if (!this.group?.id) return;
    var i = this.MessageResults.results.findIndex(
      (x) => x.id === messageId && x.groupId === groupId
    );
    if (i !== -1) this.MessageResults.results.splice(i, 1);
  }

  public receiveMessage(msg: Message) {
    if (!this.group?.id) return;
    if (msg.groupId === this.group?.id) {
      this.MessageResults.results.push(msg);
      this.scrollToBottom(50);
      this.scrollToBottom(250);
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
    return true;
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
