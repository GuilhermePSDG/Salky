import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginOrRegisterComponent } from './Components/LoginOrRegister/login-or-register.component';

const routes: Routes = [
  {path:'login',component:LoginOrRegisterComponent},
  {path:'register',component:LoginOrRegisterComponent},
  {path:'**', redirectTo:'login'}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule {}
