import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainComponent } from './main/main.component';
import { ContactsListComponent } from './Components/left-bar/left-bar.component';
import { AddGroupFormComponent } from './Components/GroupComponents/add-group-form/add-group-form.component';
import { AddUserInGroupComponent } from './Components/GroupComponents/add-user-in-group/add-user-in-group.component';

import { HttpClientModule } from '@angular/common/http';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSliderModule } from '@angular/material/slider';
import { MatGridListModule } from '@angular/material/grid-list';
import { ToggleComponent } from 'src/app/Components/toggle/toggle.component';
import { LeaveComponent } from 'src/app/Modules/Chat/Components/UserComponents/leave/leave.component';
import { ChatRoutingModule } from './Chat-routing.module';
import { CallComponent } from './Components/call/call.component';
import { FriendListComponent } from './Components/FriendsComponents/friend-list/friend-list.component';
import { GifComponent } from './Components/GroupComponents/gif/gif.component';
import { GroupMemberListComponent } from './Components/GroupComponents/group-members-list/group-members-list.component';
import { MessageRenderComponent } from './Components/GroupComponents/message-render/message-render.component';
import { ModalComponent } from '../../Components/modal/modal.component';
import { RoundedPictureComponent } from './Components/Picture/rounded-picture.component';
import { ScrenCaptureComponent } from './Components/scren-capture/scren-capture.component';
import { SearchUserComponent } from './Components/UserComponents/search-user/search-user.component';
import { SendMessageBoxComponent } from './Components/GroupComponents/send-message-input/send-message-input.component';
import { AddGroupComponent } from './Components/add-friend-or-group/add-friend-or-group.component';

import { AcceptComponent } from './Components/FriendsComponents/Buttons/accept/accept.component';
import { CancelComponent } from './Components/FriendsComponents/Buttons/cancel/cancel.component';
import { ContextMenuComponent } from 'src/app/Components/context-menu/context-menu.component';
import { ShowInfoComponent } from 'src/app/Components/show-info/show-info.component';
import { GrupoEditDisplayComponent } from './Components/GroupComponents/group-name-edit/group-name-edit.component';
import { EntrarComponent } from './Components/call/buttons/entrar/entrar.component';
import { HeadSetComponent } from './Components/call/buttons/head-set/head-set.component';
import { MicrofoneComponent } from './Components/call/buttons/microfone/microfone.component';
import { SairComponent } from './Components/call/buttons/sair/sair.component';
import { DeniedComponent } from './Components/FriendsComponents/Buttons/denied/denied.component'
import { DeleteComponent } from './Components/FriendsComponents/Buttons/delete/delete.component'
import { EmojisComponent } from './Components/GroupComponents/emojis/emojis.component';
import { ConfigButtonComponent } from './Components/ConfigComponents/config-button/config-button.component';
import { FriendsComponent } from './Pages/friends/friends.component';
import { GroupComponent } from './Pages/Group/group.component';
import { LogoutComponent } from './Pages/logout/logout.component';



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
    DeleteComponent,
    DeniedComponent,
    GroupComponent,
    EntrarComponent,
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
    AcceptComponent,
    CancelComponent,
    ToggleComponent,
    ContextMenuComponent,
    LeaveComponent,
    FriendsComponent,
    GrupoEditDisplayComponent
  ],
})
export class ChatModule { }
