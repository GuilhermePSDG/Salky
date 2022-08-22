import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LogoutComponent } from './Modules/Chat/Pages/logout/logout.component';
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
  { path: 'user/logout', component: LogoutComponent },

  { path: '**', redirectTo: 'main' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
