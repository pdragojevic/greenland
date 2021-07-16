import { Component, OnInit } from '@angular/core';
import { Team } from '../_models/Team';
import { WorkingGroup } from '../_models/WorkingGroup';
import { EmployeeService } from '../_srvc/employee.service';
import { TeamService } from '../_srvc/team.service';
import { first } from 'rxjs/operators';
import { Employee } from '../_models/Employee';
import { FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Route } from '@angular/compiler/src/core';
import { Router } from '@angular/router';

const teamValidator: ValidatorFn = (fg: FormGroup) => {
  const idPos =parseInt(fg.get('idCompanyPosition').value.match(/\d+/), 10);
  
 return idPos==4 
   ? null 
   : { dev: true };
};

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css']
})
export class EmployeesComponent implements OnInit {
  addEmployeeForm: FormGroup;
  teams: Array<Team>;
  groups: Array<WorkingGroup>; 
  leaders: Array<Employee>;
  developersFromTeam: Array<Employee>;
  teamTest: Team;
  groupTest: WorkingGroup;
  employeeTest: Employee;
  i:number;
  teamSelected:boolean=false;
  selectedTeam:Team;
  showT:boolean=false;
  addE:boolean=false;
  leader:Employee;
  success:String="";
  noLeader:boolean=false
  noDevs:boolean=false

  constructor(private employeeService: EmployeeService, private teamService: TeamService,private fb:FormBuilder,private router: Router) { }

  ngOnInit(): void {
    this.success="Employee sucessfully added!"

    this.addEmployeeForm = this.fb.group({
      firstName: [''],
      lastName: [''],
      idCompanyPosition: ['', Validators.required],
      idTeam: [''],
      birthDate: [''],
      hireDate: [''],
      gender: [''],
      email: [''],
      username: [''],
      password: [''],

  }, { validator: teamValidator});

    this.showT=false;
    this.teamSelected=false;
    this.addE=false;
   this.teamService.getAllTeams().pipe(first()).subscribe(teams => { //test get all teams
      this.teams = teams;
      console.log("Teams: ");
      console.log(teams);
    });
    

  }
  promoteLeader(username,firstName,lastName){
    this.employeeService.promoteLeader(username).pipe(first()).subscribe(leader => {
      console.log(username);
      console.log("promoted");
      this.success=`${firstName} ${lastName} promoted!`
      setTimeout(() => {  this.openTeam(this.i); }, 2000);
      
      
    });  

  }

  promoteDev(username,firstName,lastName){
    this.employeeService.promoteDeveloper
    (username).pipe(first()).subscribe(leaders => {
      console.log(username);
      console.log("promoted");
      this.success=`${firstName} ${lastName} promoted!`
      setTimeout(() => {  this.openTeam(this.i); }, 2000);
    });  

  }

  openTeam(i){
    this.i=i;
    this.success=""
    this.teamSelected=true
    console.log(i);
    this.selectedTeam=this.teams[i]
    console.log("selected team;");
    
    console.log(this.selectedTeam);
    this.teamService.getAllDevelopersFromTeam(this.selectedTeam.teamName).pipe(first()).subscribe(devs => {
      if (devs[0]==null){
        this.noDevs=true;
      }
      else{
        this.noDevs=false;
      this.developersFromTeam = devs;
      console.log("Devs: ")
      console.log(this.developersFromTeam);
    }
    });

     this.teamService.getLeaderFromTeam(this.selectedTeam.teamName).pipe(first()).subscribe(leader => {
      if (leader==null){
        this.noLeader=true;
      }
      else{
        this.noLeader=false;
      
      this.leader = leader;
      console.log("Leader: ")
      console.log(leader);
      }
    }); 
    
  }
  showTeams(){
    if(this.showT==false){
      this.showT=true
      this.addE=false
    }
    else{
      this.showT=false
    } 
  }
  showAddEmployee(){
    this.success=""
    if(this.addE==false){
      this.addE=true
      this.showT=false
    }
    else{
      this.addE=false
    } 
  }



  Submit(){
    this.employeeTest = new Employee();
    this.employeeTest.firstName = this.addEmployeeForm.controls.firstName.value;
    this.employeeTest.lastName  = this.addEmployeeForm.controls.lastName.value;
    this.employeeTest.idCompanyPosition = parseInt(this.addEmployeeForm.controls.idCompanyPosition.value.match(/\d+/), 10);
    this.employeeTest.username = this.addEmployeeForm.controls.username.value;
    this.employeeTest.password = this.addEmployeeForm.controls.password.value;
    this.employeeTest.gender = this.addEmployeeForm.controls.gender.value;
    this.employeeTest.email = this.addEmployeeForm.controls.email.value;
    if(this.employeeTest.idCompanyPosition==4){
      this.employeeTest.idTeam = parseInt(this.addEmployeeForm.controls.idTeam.value.match(/\d+/), 10); 
    }
    this.employeeTest.birthDate=this.addEmployeeForm.controls.birthDate.value;
    this.employeeTest.hireDate=this.addEmployeeForm.controls.hireDate.value;
    console.log(this.employeeTest.idTeam);
    

  this.employeeService.addEmployee(this.employeeTest).pipe(first()).subscribe(message => { 
    console.log(message);
    this.success="Employee sucessfully added!"
    setTimeout(() => {  this.addE=false; }, 2000);
    setTimeout(() => {  this.addEmployeeForm.reset(); }, 1800);
    setTimeout(() => {  location.reload(); }, 2100);
    });

  }

}
