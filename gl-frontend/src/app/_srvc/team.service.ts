import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import { api } from '../api';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TeamService {

  constructor(private http: HttpClient) { }

  getActivityFeedUsers(){
    return this.http.get<any>(`${api.apiUrl}/team/get`);
}

  getAllLeaders() {
      return this.http.get<any>(`${api.apiUrl}/team/get/leaders`);
  }

  getAllCoordinators() {
      return this.http.get<any>(`${api.apiUrl}/team/get/coordinators`);
  }

  getAllGroups() {
      return this.http.get<any>(`${api.apiUrl}/team/get/groups`);
  }

  getAllTeams() { 
      return this.http.get<any>(`${api.apiUrl}/team/get/teams`);
  }

  getAllDevelopersFromTeam(team) {
      return this.http.get<any>(`${api.apiUrl}/employees/get/developers?team=` + team);
  }

  getLeaderFromTeam(team) {
    return this.http.get<any>(`${api.apiUrl}/employees/get/leader?team=` + team);
  }

  addTeam(team) {
    const body = JSON.stringify(team);
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');   

    return this.http.post<any>(`${api.apiUrl}/team/add/team `, body, {headers: headers})
        .pipe(map(x => {
            return x;
        }));
  }

  addWorkingGroup(wgroup) {
      const body = JSON.stringify(wgroup);
      let headers = new HttpHeaders();
      headers = headers.set('Content-Type', 'application/json; charset=utf-8');   
  
      return this.http.post<any>(`${api.apiUrl}/team/add/wgroup `, body, {headers: headers})
          .pipe(map(x => {
              return x;
          }));
  }

  getAllTeamsFromCoordinator(coordinator) {
      return this.http.get<any>(`${api.apiUrl}/employees/get/teams?coordinator=` + coordinator);
  }


}
 