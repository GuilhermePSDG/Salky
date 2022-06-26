import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  UntypedFormBuilder,
  FormControl,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AnyCatcher } from 'rxjs/internal/AnyCatcher';
import { UserLogged } from 'src/app/Models/Users/UserLogged';
import { LoaddingService } from 'src/app/Services/loadding.service';
import { Destroyable } from 'src/app/Services/SalkyEvents';
import { SalkyWebSocket } from 'src/app/Services/SalykWsClient.service';
import { ShowInfoService } from 'src/app/Services/show-info.service';
import { UserService } from 'src/app/Services/UserService.service';

@Component({
  selector: 'app-login-or-register',
  templateUrl: './login-or-register.component.html',
  styleUrls: ['./login-or-register.component.scss'],
})
export class LoginOrRegisterComponent implements OnInit {
  form: UntypedFormGroup;
  public geralError?: string;
  mode: 'login' | 'register';

  public touchedAndError(
    field: 'username' | 'password',
    errorName: 'required' | '*' | 'minlength'
  ): boolean {
    var control = this.getControl(field);
    return (
      control.touched &&
      control.errors != null &&
      (errorName === '*' || control.errors[errorName] != undefined)
    );
  }
  public getControl(field: string): AbstractControl {
    return this.form.controls[field];
  }

  public get f(): any {
    return this.form.controls;
  }
  constructor(
    private userService: UserService,
    private loaddinService: LoaddingService,
    private show: ShowInfoService,
    private fb: UntypedFormBuilder,
    private ws: SalkyWebSocket,
    private router: Router
  ) {
    this.mode = router.url.includes('auth/register') ? 'register' : 'login';
    this.form = fb.group({
      username: ['', [Validators.pattern('^[A-Za-z0-9]{3,32}$')]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }
  ngOnInit(): void { }
  public login() {
    this.doAuth((a, b) => this.userService.login(a, b));
  }
  public register() {
    this.doAuth((a, b) => this.userService.register(a, b));
  }
  public doAuth(
    authMethod: (username: string, password: string) => Observable<UserLogged>
  ) {
    this.loaddinService.Show();
    authMethod(
      this.form.controls['username'].value,
      this.form.controls['password'].value
    )
      .subscribe({
        next: (usr) => {
          var list: Destroyable[] = [];
          list.push(this.ws.On("open", "*").Build<any>(x => {
            list.forEach(x => x.destroy());
            this.router.navigateByUrl('')
          }));
          list.push(this.ws.On('close', '*').Build<any>(x => {
            list.forEach(x => x.destroy());
          }));
          this.ws.connect(usr);
        },
        error: (err: any) => {
          this.geralError = err?.error?.message;
          if (err.status === 0) {
            this.show.showStyleWithOtherMsg(
              'Algo de errado não está certo..',
              'Parece que há algum problema em nossos servidores, tente novamente mais tarde.',
              'danger'
            );
          }
        },
      })
      .add(() => {
        this.loaddinService.Hide();
      });
  }
}
