import { HttpClient } from '@angular/common/http';
import { map, Observable, ReplaySubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserLogged } from '../Models/Users/UserLogged';
import { Injectable } from '@angular/core';
import { User } from '../Models/Users/User';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private currentUserSource = new ReplaySubject<UserLogged>(1);
  public currentUser$ = this.currentUserSource.asObservable();

  private baseUrl = environment.apiUrl + '/user';
  constructor(private http: HttpClient) {
    var json = localStorage.getItem('UserLogged');
    if (json) {
      this.currentUserSource.next(JSON.parse(json) as UserLogged);
    }
  }

  public login(userName: string, password: string): Observable<UserLogged> {
    return this.http
      .post<any>(`${this.baseUrl}/login`, {
        userName: userName,
        password: password,
      })
      .pipe(
        take(1),
        map((res : any) => {
          var usr = res.data;
          this.setCurrentUser(usr);
          return usr;
        })
      );
  }

  ChangeUserPicture(base64: string): Observable<void> {
    return this.http
      .put<any>(`${this.baseUrl}/foto`, {
        value: base64,
      })
      .pipe(
        take(1),
        map((x) => {
          var usr = this.getUserFromStorage();
          this.setCurrentUser(usr);
        })
      );
  }

  public register(userName: string, password: string): Observable<UserLogged> {
    return this.http
      .post<any>(`${this.baseUrl}/cadastro`, {
        userName: userName,
        password: password,
      })
      .pipe(
        take(1),
        map((res: any) => {
          var usr = res.data;
          this.setCurrentUser(usr);
          return usr;
        })
      );
  }

  public searchUser(userName: string): Observable<User[]> {
    return this.http
      .get<User[]>(`${this.baseUrl}/search?UserName=${userName}`)
      .pipe(
        take(1),
        map((n) => {
          return n;
        })
      );
  }

  public clearStorage(): void {
    localStorage.removeItem('UserLogged');
  }

  public getUserFromStorage(): UserLogged {
    var json = localStorage.getItem('UserLogged');
    if (json) {
      return JSON.parse(json) as UserLogged;
    } else {
      throw new Error('User cannot be null here');
    }
  }
  public hasUserOnStorage(): boolean {
    var json = localStorage.getItem('UserLogged');
    return json ? true : false;
  }

  
  private setCurrentUser(user: UserLogged): void {
    localStorage.setItem('UserLogged', JSON.stringify(user));
    this.currentUserSource.next(user);
  }
}
