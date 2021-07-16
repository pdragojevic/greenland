import { Component, OnInit } from '@angular/core';  
import { trigger, state, transition, style, animate, keyframes } from '@angular/animations'; /* added imports for animating */
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { LoginService } from '../_srvc/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  animations: [
    trigger('loginScreenAnimation', [
      state('invalid', style({})),
      state('unchecked', style({})),
      state('completed', style({ transform: 'translateX(150%)'})),
      transition('unchecked => completed', animate("100ms 1000ms")),
      transition('unchecked => invalid', animate(750, keyframes([
        style({ transform: 'translateX(-10%)'}),
        style({ transform: 'translateX(10%)'}),
        style({ transform: 'translateX(-10%)'}),
        style({ transform: 'translateX(10%)'}),

      ])))
    ])
  ]
})
export class LoginComponent implements OnInit {
  login_form: FormGroup;
  password_form: FormGroup;
  dis = false;
  errmessage: String;
  loginScreenAnimation: string = "unchecked";
  forgotPass: boolean = false;
  resetmessage: String;

  constructor(
    private fb:FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private LoginService: LoginService
  ) { 
    if (this.LoginService.getAuthToken) {
      this.router.navigate(['/home']);
    }
  }

  ngOnInit() {
    this.errmessage=""
      this.login_form = this.fb.group({
          username: ['', Validators.required],
          password: ['', Validators.required],
      });
      this.password_form=this.fb.group({
        email: ['', Validators.required]
    });
  }

  Submit() {
    this.dis = true;
   
    this.LoginService.login(this.login_form.controls.username.value, this.login_form.controls.password.value)
        .pipe(first())
        .subscribe({
            next: () => {
                this.errmessage = "";
                const ret = this.route.snapshot.queryParams['backTo'] || '/';
                this.router.navigateByUrl(ret);
            },  
            error: err => {
              this.dis = false;
              if (err.status == 401)
              this.errmessage = "Wrong password. Try again or click Forgot password to reset it.";
              this.loginScreenAnimation = 'invalid';   /* triggers shaking animation when login is invalid.
              put it inside the code for checking credentials */
              this.login_form.reset();

            }
        });
}
  setBackToUnchecked() {
    this.loginScreenAnimation = 'unchecked';
  }
  forgotPassword(){
    if (this.forgotPass==false){
      this.forgotPass=true;
      this.resetmessage="";
      this.errmessage = ""
    }
    else
      this.forgotPass=false;
      this.resetmessage="";
      this.errmessage = ""

  }
  sendMail(){
    this.LoginService.reset(this.password_form.controls.email.value)
        .pipe(first())
        .subscribe({
            next: () => {
              this.resetmessage="An email to reset the password has been sent!"
            },  
            error: err => {
              this.resetmessage="Please enter a valid email address!"
              console.log(err);            

            }
        });

  }


}
