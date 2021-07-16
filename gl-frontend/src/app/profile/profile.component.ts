import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../_srvc/employee.service';
import { first } from 'rxjs/operators';
import { EmployeeLogged } from '../_models/EmployeeLogged';
import { $ } from 'protractor';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  emp: EmployeeLogged;
  pipe = new DatePipe('en_US');
  isTeamShown: boolean = true;
  constructor( private employeeService: EmployeeService) { }

  ngOnInit(){
    document.body.classList.add('bg-img');
    this.emp=new EmployeeLogged();
    this.employeeService.getEmployeeLogged().pipe(first()).subscribe(emp => {
      this.emp = emp;
      if(this.emp.position === "Administration") {
        console.log('mijenjam')
        this.isTeamShown = false;
      }
      this.emp.hireDate = this.pipe.transform(this.emp.hireDate, 'longDate');
      this.emp.birthDate = this.pipe.transform(this.emp.birthDate, 'longDate');  
  });
    
  }
};