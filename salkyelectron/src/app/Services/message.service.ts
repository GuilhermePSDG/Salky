import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Message } from '../Models/Message';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  constructor(private http: HttpClient) {}
  private baseUrl = `${environment.apiUrl}/message`;
  public getMessages(contactId: string): Observable<Message[]> {
    return this.http
      .get<Message[]>(`${this.baseUrl}/${contactId}`)
      .pipe(take(1));
  }
}
