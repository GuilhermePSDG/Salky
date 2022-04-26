import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, ReplaySubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Contact } from '../Models/Contact';

@Injectable({
  providedIn: 'root',
})
export class ContactService {
  private currentContacts = new ReplaySubject<Contact[] | null>(1);
  public currentContacts$ = this.currentContacts.asObservable();


  private apiBaseUrl = `${environment.apiUrl}/contact`;
  constructor(private http: HttpClient) {}

  GetAllContacts(): Observable<Contact[]> {
    return this.http.get<Contact[]>(this.apiBaseUrl).pipe(take(1),map(f => {
      this.setLocalStorage(f);
      this.currentContacts.next(f);
      return f;
    }));
  }




  public clearStorage() : void{
    localStorage.removeItem("Contacts");
  }


  public retriveFromStorage() : Contact[] | null {
    var json = localStorage.getItem("Contacts");
    if(json)
    {
      return JSON.parse(json) as Contact[]
    }
    return null;
  }

  public findInLocalStorage(contactId : string) : Contact | undefined{
    return this.retriveFromStorage()?.filter(x => x.contactId === contactId)[0];
  }

  public setLocalStorage(contacts: Contact[]): void {
    localStorage.setItem("Contacts", JSON.stringify(contacts));
  }

}
