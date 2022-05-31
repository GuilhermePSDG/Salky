import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { catchError, Observable, take } from 'rxjs';
import { UserService } from '../Services/UserService.service';
import { UserLogged } from '../Models/Users/UserLogged';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';

//TO DO
//Limitar somente para a url conveniente
@Injectable()
export class HttpInterceptorInterceptor implements HttpInterceptor {
  constructor(private userService: UserService, private router: Router,
    private activeRoute : ActivatedRoute) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    if (this.CanInjectSalkyApiToken(request)) {
      //Se tiver usuario no storage
      if (this.userService.hasUserOnStorage()) {

        let currentUser = this.userService.getUserFromStorage();
        //Se o token ta expirado, faz o logout
        if (
          new Date(currentUser.TokenExpire).getTime() < new Date().getTime()
        ) {
          this.router.navigateByUrl('user/logout');
        }
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${currentUser.token}`,
          },
        });
      }
      //Se não manda pro login
      else {
        this.router.navigateByUrl('user/login');
      }
    }else{
      console.log(this.activeRoute.snapshot.url)
      console.log("Não foi possivel injetar o toeken" + "para : " + request.url + " na rota : " + this.router.url)
    }

    return next.handle(request).pipe(
      catchError((error) => {
        if (this.CanInjectSalkyApiToken(request) && error) {
          if (error.status === 401) {
            console.log(this.activeRoute.snapshot.url)
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
}
