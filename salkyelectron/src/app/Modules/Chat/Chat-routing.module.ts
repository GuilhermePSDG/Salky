import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatBoxComponent } from './Components/chat-box/chat-box.component';
import { FriendsComponent } from './Components/friends/friends.component';
import { MainComponent } from './Components/main/main.component';
import { LogoutComponent } from './Components/Shared/user/logout/logout.component';

const routes: Routes = [
  {
    path: '',
    component : MainComponent,
    children: [
      {
        path: 'group/:id',
        component: ChatBoxComponent,
      },
      {
        path: 'friends',
        component: FriendsComponent,
      },
    ],
  },


  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ChatRoutingModule {}
