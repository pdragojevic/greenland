import { Component, OnInit } from '@angular/core';
import { Employee } from '../_models/Employee';
import { TeamService } from '../_srvc/team.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-promotion',
  templateUrl: './promotion.component.html',
  styleUrls: ['./promotion.component.css']
})
export class PromotionComponent implements OnInit {
  coordinators: Array<Employee>;
  leaders: Array<Employee>;

  constructor(private teamService: TeamService) { }

  ngOnInit(): void {
    this.teamService.getAllCoordinators().pipe(first()).subscribe(coordinators => {
      this.coordinators = coordinators;
      console.log("Coordinators: " + coordinators);
    }); 

    this.teamService.getAllLeaders().pipe(first()).subscribe(leaders => {
      this.leaders = leaders;
      console.log("Leaders: " + leaders);
    });

    this.teamService.getAllGroups().pipe(first()).subscribe(groups => {
      console.log("Groups: " + groups);
    });

    this.teamService.getAllTeams().pipe(first()).subscribe(teams => {
      console.log("Teams " + teams);
    });

  }

}
