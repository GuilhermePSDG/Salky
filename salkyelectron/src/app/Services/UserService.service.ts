import { HttpClient } from '@angular/common/http';
import { map, Observable, ReplaySubject, take } from 'rxjs';
import { User } from 'src/app/Models/UserWsServer';
import { environment } from 'src/environments/environment';
import { UserLogged } from '../Models/UserLogged';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private currentUserSource = new ReplaySubject<UserLogged | null>(1);
  public currentUser$ = this.currentUserSource.asObservable();


  private baseUrl = environment.apiUrl + '/user';
  constructor(private http: HttpClient) {}

  public login(userName: string, password: string): Observable<void>  {
    return this.http
      .post<UserLogged>(`${this.baseUrl}/login`, {
        userName: userName,
        password: password,
      })
      .pipe(
        take(1),
        map((usr: UserLogged) => {
          this.setCurrentUser(usr);
        })
      );
  }

  public register(userName: string, password: string) {

  }
  public clearStorage() : void{
    localStorage.removeItem("UserLogged");
  }


  public getUserFromStorage() : UserLogged | null {
    var json = localStorage.getItem("UserLogged");
    if(json)
    {
      return JSON.parse(json) as UserLogged
    }
    return null;
  }

  public setCurrentUser(user: UserLogged): void {
    localStorage.setItem("UserLogged", JSON.stringify(user));
    this.currentUserSource.next(user);
  }
}
