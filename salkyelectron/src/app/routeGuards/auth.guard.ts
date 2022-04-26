import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UserService } from '../Services/UserService.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private userService: UserService) {}

  canActivate(): boolean {
    if (this.userService.getUserFromStorage() != null) {
      return true;
    } else {
      this.router.navigate(['/user/login']);
      return false;
    }
  }
}
