import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { catchError, Observable, take } from 'rxjs';
import { UserService } from '../Services/UserService.service';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { UserLogged } from '../Models/Users/UserLogged';

//TO DO
//Limitar somente para a url conveniente
@Injectable()
export class HttpInterceptorInterceptor implements HttpInterceptor {
  constructor(private userService: UserService, private router: Router,
    private activeRoute: ActivatedRoute) { }

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    var tokenIneject = false;

    if (this.CanInjectSalkyApiToken(request)) {
      if (this.userService.hasUserOnStorage()) {
        let currentUser = this.userService.getUserFromStorage()
        if (!this.UserIsMinimalValid(currentUser) || currentUser.TokenExpire) {
          this.router.navigateByUrl('user/logout');
        }
        else if (new Date(currentUser.TokenExpire).getTime() < new Date().getTime()) {
          this.router.navigateByUrl('user/logout');
        } else {
          tokenIneject = true;
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${currentUser.token}`,
            },
          });
        }
      }
      else {
        this.router.navigateByUrl('user/login');
      }
    }

    return next.handle(request).pipe(
      catchError((error) => {
        if (tokenIneject) {
          if (error?.status === 401) {
            this.router.navigateByUrl('user/logout');
          }
        }
        throw error;
      })
    );

  }
  private CanInjectSalkyApiToken(request: HttpRequest<unknown>): boolean {
    return (
      request.url.includes(environment.apiUrl) &&
      this.userService.hasUserOnStorage() &&
      this.router.url !== '/user/login' &&
      this.router.url !== '/user/register'
    );
  }

  private UserIsMinimalValid(user: UserLogged): boolean {
    return (user.id !== '' && user.userName !== '' && user.token !== '' && user.TokenExpire !== null);
  }
}
