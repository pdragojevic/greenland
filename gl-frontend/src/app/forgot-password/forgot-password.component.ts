import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute,Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { LoginService } from '../_srvc/login.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  password_form: FormGroup;
  id:String;
 
  errmessage:String;


  constructor(private fb:FormBuilder,
    private Activatedroute:ActivatedRoute,
               private router:Router, private  loginService :LoginService){}

  ngOnInit(): void {
    document.body.classList.add('bg-img');
    this.errmessage=""
    this.id =this.Activatedroute.snapshot.queryParamMap.get('guid');
    this.password_form=this.fb.group({
      password: ['', Validators.required]
  });

  }

  submit(){        
    this.loginService.sendNewPassword(this.password_form.controls.password.value, this.id).pipe(first())
    .subscribe({
        next: () => {
         this.errmessage="Password changed sucessfuly!"
         setTimeout(() => {  this.router.navigate(['/']); }, 1000);

        },  
        error: err => {
         this.errmessage="Something went wrong, please try again"
         console.log(err);
        }
    });

}
}