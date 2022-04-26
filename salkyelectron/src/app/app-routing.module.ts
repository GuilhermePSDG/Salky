import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatBoxComponent } from './Components/chat-box/chat-box.component';
import { LoginComponent } from './Components/user/login/login.component';
import { LogoutComponent } from './Components/user/logout/logout.component';
import { ProfileComponent } from './Components/user/profile/profile.component';
import { RegisterComponent } from './Components/user/register/register.component';
import { UserComponent } from './Components/user/user.component';
import { AuthGuard } from './routeGuards/auth.guard';

const routes: Routes = [
  {
    path:'',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children:[
      {
        path: 'contact/:contactId',
        component: ChatBoxComponent,
      },
      {
        path: 'user',
        component: UserComponent,
        children:[
          {
            path: 'logout',
            component: LogoutComponent,
          },
          {
            path: 'profile',
            component: ProfileComponent,
          },
        ]
      },
    ]
  },
  {
    path: 'user',
    component: UserComponent,
    children: [
      {
        path: 'login',
        component: LoginComponent,
      },
      {
        path: 'register',
        component: RegisterComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
