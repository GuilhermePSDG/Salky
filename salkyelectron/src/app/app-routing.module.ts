import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LogoutComponent } from './Modules/Chat/Components/Shared/user/logout/logout.component';
import { AuthGuard } from './routeGuards/auth.guard';

const routes: Routes = [

  {
    path: 'auth',
    loadChildren: () =>
      import('./Modules/Auth/Auth.module').then((x) => x.AuthModule),
  },

  {
    path: 'main',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./Modules/Chat/Chat.module').then((x) => x.ChatModule),
  },
  {path:'user/logout',component:LogoutComponent},

  { path: '**', redirectTo: 'main' },

  // {
  //   path: '',
  //   runGuardsAndResolvers: 'always',
  //   canActivate: [AuthGuard],
  //   children: [
  //     {
  //       path: '',
  //       redirectTo: 'main',
  //       pathMatch: 'full',
  //     },
  //     {
  //       path: 'main',
  //       component: MainComponent,
  //       children: [
  //         {
  //           path: 'group/:id',
  //           component: ChatBoxComponent,
  //         },
  //         {
  //           path:"friends",
  //           component : FriendsComponent,
  //         }
  //       ],
  //     },
  //   ],
  // },

  // {
  //   path: 'user',
  //   children: [
  //     {
  //       path: 'logout',
  //       component: LogoutComponent,
  //     },
  //     {
  //       path: 'login',
  //       component: LoginComponent,
  //     },
  //     {
  //       path: 'register',
  //       component: LoginComponent,
  //     },
  //   ],
  // },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
