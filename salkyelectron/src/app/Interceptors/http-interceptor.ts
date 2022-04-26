import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { catchError, Observable, take } from 'rxjs';
import { UserService } from '../Services/UserService.service';
import { UserLogged } from '../Models/UserLogged';
import { Router } from '@angular/router';

//TO DO
//Limitar somente para a url conveniente
@Injectable()
export class HttpInterceptorInterceptor implements HttpInterceptor {
  constructor(private userService: UserService,private router : Router) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    console.log("huasdhuasuhahusd")
    let currentUser = this.userService.getUserFromStorage();
    if (currentUser) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser.token}`,
        },
      });
    }
    return next.handle(request).pipe(
      catchError((error) => {
        if (error) {
          this.userService.clearStorage();
          this.router.navigateByUrl("user/login");
        }
        console.error(error);
        throw error;
      })
    );

  }
}
