import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { map, take } from 'rxjs';
import { Contact } from 'src/app/Models/Contact';
import { UserLogged } from 'src/app/Models/UserLogged';
import { AudioService } from 'src/app/Services/AudioService';
import { ContactService } from 'src/app/Services/ContactService.service';
import { UserService } from 'src/app/Services/UserService.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './contacts-list.component.html',
  styleUrls: ['./contacts-list.component.scss'],
})
export class ContactsListComponent implements OnInit {
  public contacts?: Contact[] | null;
  public currentUser: UserLogged | null = null;
  constructor(
    public audioService: AudioService,
    public userService: UserService,
    public contactService: ContactService,
    public router: Router
  ) {}

  public MicrofoneMuted: boolean = false;
  public HeadPhoneMuted: boolean = false;
  onSelectedChanged(contact: Contact) {
    this.router.navigateByUrl(`contact/messages/${contact.contactId}`);
    console.log('navigated to ' + contact.contactId);
  }
  routerLink = '';

  ngOnInit(): void {
    this.currentUser = this.userService.getUserFromStorage();
    this.contacts = this.contactService.retriveFromStorage();

    this.userService.currentUser$.subscribe({
      next: (user) => {
        if (user) {
          this.currentUser = user;

          this.contactService.GetAllContacts().subscribe({
            next: (data: Contact[]) => {
              if(data){
                this.contacts = data;
              }
              console.log(this.contacts);
            },
            error: (f) => console.error(f),
            complete: () => {},
          });
        } else {
          this.router.navigateByUrl('user/login');
        }
      },
      error: (f) => console.error(f),
      complete: () => {},
    });
  }
}
