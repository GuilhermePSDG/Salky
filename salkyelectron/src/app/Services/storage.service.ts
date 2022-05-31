import { Injectable } from '@angular/core';
import {
  filter,
  iif,
  mergeMap,
  Observable,
  of,
  ReplaySubject,
  switchMap,
} from 'rxjs';
import { Group } from '../Models/GroupModels/Group';
import { UserLogged } from '../Models/Users/UserLogged';

@Injectable({
  providedIn: 'root',
})
export class StorageService {
  constructor() {}

  public get CurrentUser(): UserLogged {
    return this.GetRequired<UserLogged>('UserLogged');
  }
  public set CurrentUser(user: UserLogged) {
    this.Set('UserLogged', user);
  }

  private Get<T>(name: string): T | null {
    var result = localStorage.getItem(name);
    if (!result) return null;
    return JSON.parse(result) as T;
  }

  private GetRequired<T>(name: string): T {
    var result = localStorage.getItem(name);
    if (!result) {
      throw new Error('Cannot get required item from storage');
    }
    return JSON.parse(result) as T;
  }

  private Set(name: string, data: any): void {
    var dataJson = JSON.stringify(data);
    localStorage.setItem(name, dataJson);
    this.listener.next({
      key: name,
      data: data,
    });
  }
  private listener = new ReplaySubject<keyvalue>(1);
  private listener$ = this.listener.asObservable();
}
interface keyvalue {
  key: string;
  data: any;
}
