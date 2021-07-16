import { Component, OnInit } from '@angular/core';
import { TeamService } from '../_srvc/team.service';
import { Team } from '../_models/Team';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { Employee } from '../_models/Employee';
import { WorkingGroup } from '../_models/WorkingGroup';


@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css']
})
export class TeamsComponent implements OnInit {
  addTeamForm: FormGroup;
  teamTest: Team;
  leaders:Array<Employee>;
  groups:Array<WorkingGroup>;
  teams:Array<Team>;
  teamNames:Array<String>;
  error:String;
  team:Team;
  success:String;

  constructor(private fb:FormBuilder, private teamService: TeamService) { }

  ngOnInit(): void {
    document.body.classList.add('bg-img');
    this.error=""
    this.success=""

    this.teamService.getAllTeams().pipe(first()).subscribe(teams => { //test get all teams
      this.teams = teams;
      this.teamNames=[];
      teams.forEach(team => this.teamNames.push(team.teamName));
      console.log("imena timova");
    
      console.log(this.teamNames);
    });
    

    
    this.addTeamForm = this.fb.group({
      teamName: ['', Validators.required],
      IdWorkingGroup:['', Validators.required],
      IdTeamLeader:['', Validators.required]

      


  });

  


  this.teamService.getAllLeaders().pipe(first()).subscribe(leaders => {
    this.leaders = leaders;
    console.log("Leaders: ");
    console.log(leaders);
  });

  this.teamService.getAllGroups().pipe(first()).subscribe(groups => { 
    this.groups = groups;
    console.log("groups: ");
    console.log(groups);
  });


  }

  



  Submit(){



    console.log(this.addTeamForm.controls.teamName.value);
    console.log(this.teamNames.includes(this.addTeamForm.controls.teamName.value));
    
    if(this.teamNames.includes(this.addTeamForm.controls.teamName.value)){
      this.error="That team name is already taken! Choose another one"
      this.success=""
    }
    else{
      this.error=""
      this.success="Team sucessfully added"
      

    
        this.teamTest = new Team();
        this.teamTest.teamName = this.addTeamForm.controls.teamName.value;
        
        this.teamTest.IdTeamLeader =parseInt(this.addTeamForm.controls.IdTeamLeader.value.match(/\d+/), 10);
        console.log(parseInt(this.addTeamForm.controls.IdTeamLeader.value.match(/\d+/), 10));
         
        console.log(this.teamTest.IdTeamLeader);
        
        
        this.teamTest.IdWorkingGroup = parseInt(this.addTeamForm.controls.IdWorkingGroup.value.match(/\d+/), 10);
        console.log(parseInt(this.addTeamForm.controls.IdWorkingGroup.value.match(/\d+/), 10));
         
        console.log(this.teamTest.IdWorkingGroup);
        
        console.log(this.teamTest);
        


      this.teamService.addTeam(this.teamTest).pipe(first()).subscribe(message => { 
        console.log(message);
        });

        this.teamNames.push(this.addTeamForm.controls.teamName.value)
  }

  }


      
  }

