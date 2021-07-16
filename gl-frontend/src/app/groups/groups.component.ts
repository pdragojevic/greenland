import { Component, OnInit } from '@angular/core';
import { Employee } from '../_models/Employee';
import { TeamService } from '../_srvc/team.service';
import { first } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { Route } from '@angular/compiler/src/core';
import { WorkingGroup } from '../_models/WorkingGroup';


@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})
export class GroupsComponent implements OnInit {
  AddWorkingGroup: FormGroup;

  coordinators: Array<Employee>;
  addGroupForm: FormGroup;
  groups: Array<WorkingGroup>;
  group: WorkingGroup;
  error:String;
  success:String;
  groupNames: Array<String>;


  constructor(
    private teamService: TeamService,
    private fb: FormBuilder,
    private router: Router
    ) { }

  ngOnInit(): void {
    this.success=""
    this.error=""
    this.teamService.getAllGroups().pipe(first()).subscribe(groups => {
      this.groups = groups;
      this.groupNames=[];
      groups.forEach(group => this.groupNames.push(group.workingGroupName));
    });
    

    this.addGroupForm = this.fb.group({
      groupName: ['', Validators.required],
      IdCoordinator:['', Validators.required]
    });

    this.teamService.getAllGroups().pipe(first()).subscribe(groups => { 
      this.groups = groups;
      console.log("groups: ");
      console.log(groups);
    });


    this.teamService.getAllCoordinators().pipe(first()).subscribe(coordinators => {
      this.coordinators = coordinators;
    })
    
  }
  
  Submit() {

    if(this.groupNames.includes(this.addGroupForm.controls.groupName.value)){
      this.error="That group name is already taken! Choose another one"
      this.success=""
    }
    else{

    this.error=""
    this.success="Group sucessfully added"
    this.group=new WorkingGroup();
    this.group.WorkingGroupName = this.addGroupForm.controls.groupName.value;
        
    this.group.IdCoordinator =parseInt(this.addGroupForm.controls.IdCoordinator.value.match(/\d+/), 10); 
    console.log(this.group);
    
    this.teamService.addWorkingGroup(this.group).pipe(first()).subscribe(message => { 
      console.log(message);
      });

      this.groupNames.push(this.addGroupForm.controls.groupName.value)
      console.log(this.groupNames);

  }

  }
}
