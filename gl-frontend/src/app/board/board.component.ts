import { Component, OnInit } from '@angular/core';
import { GroupsComponent } from '../groups/groups.component';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {
  teamsShown:Boolean;
  groupsShown:Boolean;

  constructor() { }

  ngOnInit(): void {
    document.body.classList.add('bg-img');
    this.teamsShown=false;
    this.groupsShown=false;
  }

  groups(){
    if (this.groupsShown){
      this.groupsShown=false;
    }
    else if(this.teamsShown){
      this.groupsShown=true;
      this.teamsShown=false;
    }
    else{
      this.groupsShown=true;
    }
  
  }
  teams(){
    if (this.teamsShown){
      this.teamsShown=false;
    }
    else if(this.groupsShown){
      this.groupsShown=false;
      this.teamsShown=true;
    }
    else{
      this.teamsShown=true;
    }

  }

}


