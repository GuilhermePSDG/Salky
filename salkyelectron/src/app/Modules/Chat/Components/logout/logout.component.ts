import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SalkyWebSocket } from 'src/app/Services/SalykWsClient.service';

@Component({
  selector: 'NO-SELECTOR',
  template: '',
  styles: ['']
})
export class LogoutComponent implements OnInit {

  constructor(private router: Router, private chatService: SalkyWebSocket) { }

  ngOnInit(): void {
    console.log("LOGOUT PAGE");
    try {
      this.chatService.close();
    }catch(e){};
    localStorage.clear()
    this.router.navigateByUrl("");
  }

}
