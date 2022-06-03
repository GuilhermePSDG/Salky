import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatBoxComponent } from './Components/chat-box/chat-box.component';
import { MainComponent } from './main/main.component';
import { LogoutComponent } from './Components/logout/logout.component';
import { FriendsComponent } from './Components/friends/friends.component';

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
