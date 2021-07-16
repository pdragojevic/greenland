import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { first } from 'rxjs/operators';
import { Employee } from '../_models/Employee';
import { EmployeeService } from '../_srvc/employee.service';
import { LoginService } from '../_srvc/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  changePassword_form: FormGroup;
  errmessage: String;
  success: boolean;
  username: String;
  emp:Employee;
  password:String;
  fail: boolean;

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private loginService: LoginService,
    private router: Router

  ) { }

  ngOnInit(): void {
    this.errmessage=""
    this.emp=new Employee();
     this.employeeService.getEmployee().subscribe(data => this.emp=data);
     this.changePassword_form = this.fb.group({
      oldPassword:['', Validators.required],
      newPassword1: ['', Validators.required],
      newPassword2: ['', Validators.required]
  });
  }
  submit(){
      this.errmessage="";
      if (this.changePassword_form.controls.newPassword1.value != this.changePassword_form.controls.newPassword2.value){
        this.errmessage="New passwords do not match!";
      }
      else{   
         this.loginService.changePassword(this.emp.username, this.changePassword_form.controls.newPassword1.value, this.changePassword_form.controls.oldPassword.value).pipe(first())
         .subscribe({
             next: () => {
              this.errmessage="Password changed sucessfuly!"
              setTimeout(() => {  this.router.navigate(['/profile']); }, 1000);

             },  
             error: err => {
              this.errmessage="You entered wrong password"
              console.log(err);
              this.changePassword_form.reset();
             }
         });
      }   

}
}
