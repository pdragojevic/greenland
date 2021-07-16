import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { EmployeeLogged } from '../_models/EmployeeLogged';
import { TaskService } from '../_srvc/task.service';
import { EmployeeService } from '../_srvc/employee.service';
import {TeamService} from '../_srvc/team.service';
import { Employee } from '../_models/Employee';
import { CalComponent } from '../cal/cal.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  activities: Array<String>;
  team: Array<Employee>; 
  emp: EmployeeLogged;
  isTeamShown: boolean = true;

  

  constructor(private taskService:TaskService,
    private employeeService: EmployeeService,
    private teamService:TeamService) {  document.body.classList.add('bg-img');
    this.emp=new EmployeeLogged();
    this.team=[]
    this.activities=[]
    this.employeeService.getEmployeeLogged().pipe(first()).subscribe(emp => {
      this.emp = emp;
      if(this.emp.position === "Administration") {
        this.isTeamShown = false;
      }
    });
    this.teamService.getActivityFeedUsers().pipe(first()).subscribe(team=>{
      this.team=team;
      console.log(team);
    });
    this.taskService.getUserActivity().pipe(first()).subscribe(activities=>{
      this.activities=activities;
      this.activities.push('Activity1')
      this.activities.push('Activity2')
      this.activities.push('Activity3')
      console.log("aktivnosti:");
      
      console.log(activities)
    }); }

  ngOnInit(): void {
  
  }

}
