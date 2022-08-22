import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { FriendsComponent } from './Pages/friends/friends.component';
import { GroupComponent } from './Pages/Group/group.component';
import { LogoutComponent } from './Pages/logout/logout.component';

const routes: Routes = [
  {
    path: '',
    component : MainComponent,
    children: [
      {
        path: 'group/:id',
        component: GroupComponent,
      },
      {
        path: 'friends',
        component: FriendsComponent,
      },
    ],
  },
  { path: 'user/logout', component: LogoutComponent },


  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ChatRoutingModule {}
