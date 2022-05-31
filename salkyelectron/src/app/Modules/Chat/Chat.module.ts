import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainComponent } from './Components/main/main.component';
import { GifComponent } from './Components/Shared/gif/gif.component';
import { CallComponent } from './Components/Shared/call/call.component';
import { ModalComponent } from './Components/Shared/modal/modal.component';
import { ChatBoxComponent } from './Components/chat-box/chat-box.component';
import { EmojisComponent } from './Components/Shared/emojis/emojis.component';
import { AddGroupComponent } from './Components/Shared/AddGroup/add-group.component';
import { RoundedPictureComponent } from './Components/Shared/Picture/rounded-picture.component';
import { ContactsListComponent } from './Components/Shared/contacts-list/contacts-list.component';
import { ScrenCaptureComponent } from './Components/Shared/scren-capture/scren-capture.component';
import { MessageRenderComponent } from './Components/Shared/message-render/message-render.component';
import { SendMessageBoxComponent } from './Components/Shared/send-message-box/send-message-box.component';
import { AddGroupFormComponent } from './Components/Shared/add-group-form/add-group-form.component';
import { SearchUserComponent } from './Components/Shared/search-user/search-user.component';
import { GroupMemberListComponent } from './Components/Shared/group-members-list/group-members-list.component';
import { AddUserInGroupComponent } from './Components/Shared/add-user-in-group/add-user-in-group.component';
import { FriendsComponent } from './Components/friends/friends.component';
import { FriendListComponent } from './Components/Shared/friend-list/friend-list.component';

import { HttpClientModule } from '@angular/common/http';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSliderModule } from '@angular/material/slider';
import { MatGridListModule } from '@angular/material/grid-list';
import { LogoutComponent } from './Components/Shared/user/logout/logout.component';
import { MicrofoneComponent } from 'src/app/Components/Shared/Buttons/Call/microfone/microfone.component';
import { HeadSetComponent } from 'src/app/Components/Shared/Buttons/Call/head-set/head-set.component';
import { ConfigButtonComponent } from 'src/app/Components/Shared/Buttons/Config/config-button/config-button.component';
import { EntrarComponent } from 'src/app/Components/Shared/Buttons/Call/entrar/entrar.component';
import { SairComponent } from 'src/app/Components/Shared/Buttons/Call/sair/sair.component';
import { ShowInfoComponent } from 'src/app/Components/Shared/show-info/show-info.component';
import { DeleteComponent } from 'src/app/Components/Shared/Buttons/Friends/delete/delete.component';
import { AcceptComponent } from 'src/app/Components/Shared/Buttons/Friends/accept/accept.component';
import { CancelComponent } from 'src/app/Components/Shared/Buttons/Friends/cancel/cancel.component';
import { DeniedComponent } from 'src/app/Components/Shared/Buttons/Friends/denied/denied.component';
import { ToggleComponent } from 'src/app/Components/Shared/Buttons/toggle/toggle.component';
import { ContextMenuComponent } from 'src/app/Components/Shared/context-menu/context-menu.component';
import { LeaveComponent } from 'src/app/Components/Shared/Buttons/User/leave/leave.component';
import { ChatRoutingModule } from './Chat-routing.module';

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
    FriendsComponent,
    FriendListComponent,
    DeleteComponent,
    AcceptComponent,
    CancelComponent,
    DeniedComponent,
    ToggleComponent,
    ContextMenuComponent,
    LeaveComponent,
  ],
})
export class ChatModule {}
