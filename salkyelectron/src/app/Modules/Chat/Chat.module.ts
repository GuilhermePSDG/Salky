import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainComponent } from './main/main.component';
import { ChatBoxComponent } from './Components/chat-box/chat-box.component';
import { ContactsListComponent } from './Components/left-bar/left-bar.component';
import { AddGroupFormComponent } from './Components/add-group-form/add-group-form.component';
import { AddUserInGroupComponent } from './Components/add-user-in-group/add-user-in-group.component';

import { HttpClientModule } from '@angular/common/http';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSliderModule } from '@angular/material/slider';
import { MatGridListModule } from '@angular/material/grid-list';
import { LogoutComponent } from './Components/logout/logout.component';
import { ToggleComponent } from 'src/app/Components/toggle/toggle.component';
import { LeaveComponent } from 'src/app/Modules/Chat/Components/buttons/User/leave/leave.component';
import { ChatRoutingModule } from './Chat-routing.module';
import { CallComponent } from './Components/call/call.component';
import { EmojisComponent } from './Components/emojis/emojis.component';
import { FriendListComponent } from './Components/friend-list/friend-list.component';
import { GifComponent } from './Components/gif/gif.component';
import { GroupMemberListComponent } from './Components/group-members-list/group-members-list.component';
import { MessageRenderComponent } from './Components/message-render/message-render.component';
import { ModalComponent } from '../../Components/modal/modal.component';
import { RoundedPictureComponent } from './Components/Picture/rounded-picture.component';
import { ScrenCaptureComponent } from './Components/scren-capture/scren-capture.component';
import { SearchUserComponent } from './Components/search-user/search-user.component';
import { SendMessageBoxComponent } from './Components/send-message-box/send-message-box.component';
import { AddGroupComponent } from './Components/add-friend-or-group/add-friend-or-group.component';
import { FriendsComponent } from './Components/friends/friends.component';
import { EntrarComponent } from './Components/buttons/Call/entrar/entrar.component';
import { HeadSetComponent } from './Components/buttons/Call/head-set/head-set.component';
import { MicrofoneComponent } from './Components/buttons/Call/microfone/microfone.component';
import { SairComponent } from './Components/buttons/Call/sair/sair.component';
import { ConfigButtonComponent } from './Components/buttons/Config/config-button/config-button.component';
import { AcceptComponent } from './Components/buttons/Friends/accept/accept.component';
import { CancelComponent } from './Components/buttons/Friends/cancel/cancel.component';
import { DeleteComponent } from './Components/buttons/Friends/delete/delete.component';
import { DeniedComponent } from './Components/buttons/Friends/denied/denied.component';
import { ContextMenuComponent } from 'src/app/Components/context-menu/context-menu.component';
import { ShowInfoComponent } from 'src/app/Components/show-info/show-info.component';

@NgModule({
  imports: [
    ChatRoutingModule,
    CommonModule,
    HttpClientModule,
    MatTooltipModule,
    FormsModule,
    ReactiveFormsModule,
    MatCheckboxModule,
    MatSliderModule,
    MatGridListModule,
  ],

  declarations: [
    ChatBoxComponent,
    SendMessageBoxComponent,
    ContactsListComponent,
    RoundedPictureComponent,
    ScrenCaptureComponent,
    LogoutComponent,
    ModalComponent,
    GifComponent,
    EmojisComponent,
    MicrofoneComponent,
    HeadSetComponent,
    ConfigButtonComponent,
    EntrarComponent,
    SairComponent,
    CallComponent,
    MainComponent,
    MessageRenderComponent,
    ShowInfoComponent,
    AddGroupComponent,
    AddGroupFormComponent,
    SearchUserComponent,
    GroupMemberListComponent,
    AddUserInGroupComponent,
    FriendListComponent,
    DeleteComponent,
    AcceptComponent,
    CancelComponent,
    DeniedComponent,
    ToggleComponent,
    ContextMenuComponent,
    LeaveComponent,
    FriendsComponent
  ],
})
export class ChatModule {}
