import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AnyCatcher } from 'rxjs/internal/AnyCatcher';
import { LoaddingService } from 'src/app/Services/loadding.service';
import { ShowInfoService } from 'src/app/Services/show-info.service';
import { UserService } from 'src/app/Services/UserService.service';

@Component({
  selector: 'app-login-or-register',
  templateUrl: './login-or-register.component.html',
  styleUrls: ['./login-or-register.component.scss'],
})
export class LoginOrRegisterComponent implements OnInit {
  form: FormGroup;
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
    private fb: FormBuilder,
    private router: Router
  ) {
    this.mode = router.url.includes('auth/register') ? 'register' : 'login';
    this.form = fb.group({
      username: ['', [Validators.pattern('^[A-Za-z0-9]{3,32}$')]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }
  ngOnInit(): void {}
  public login() {
    this.doAuth((a, b) => this.userService.login(a, b));
  }
  public register() {
    this.doAuth((a, b) => this.userService.register(a, b));
  }
  public doAuth(
    authMethod: (username: string, password: string) => Observable<void>
  ) {
    this.loaddinService.Show();
    authMethod(
      this.form.controls['username'].value,
      this.form.controls['password'].value
    )
      .subscribe({
        next: (s) => this.router.navigateByUrl(''),
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
