import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AnyCatcher } from 'rxjs/internal/AnyCatcher';
import { UserService } from 'src/app/Services/UserService.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  public userModel = {
    password : '',
    username:'',
  };

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {}

  public login() {
    console.log(this.userModel);
    this.userService
      .login(this.userModel.username, this.userModel.password)
      .subscribe({
        complete: () => this.router.navigate(['']),
        error: (err: any) => {
          if (err.status == 401) {
            console.log('Unautorized');
          }
          console.error(err);
        },
      });
  }
}
