import { Component } from '@angular/core';
import { LoginService } from './_srvc/login.service';
import { EmployeeService } from './_srvc/employee.service';
import { first } from 'rxjs/operators';
import { EmployeeLogged } from './_models/EmployeeLogged';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'gl-frontend';
  auth: String;
  isTeamShown: boolean = true;

  x = location.pathname;
  promoteActive: boolean;
  emp: EmployeeLogged;

  constructor(private loginService: LoginService, private employeeService: EmployeeService, private router: Router) { 
    this.router.events.subscribe((x) => {
      this.employeeService.getEmployeeLogged().pipe(first()).subscribe(emp => {
        this.emp = emp;
        this.isTeamShown=true;
        if(this.emp.position === "Administration") {
          this.isTeamShown = false;
        }
      });
  });
  
  }

  ngOnInit(){
    this.loginService.userAuthToken.subscribe(auth => this.auth = auth);
    this.employeeService.getEmployeeLogged().pipe(first()).subscribe(emp => {
      this.emp = emp;
    });
  }  
  logout() {
    this.loginService.logout();
    console.log("logged out");
}

  search(sf) {
    const google = 'https://www.google.com/search?q=site%3A+';
    const site = '';
    const url = google + site + '+' + sf.value.searchtext;
    const win = window.open(url, '_blank');
    win.focus();
  }
}
