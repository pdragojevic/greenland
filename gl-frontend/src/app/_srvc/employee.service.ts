import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Employee } from '../_models/Employee';
import { api } from '../api';
import { EmployeeLogged } from '../_models/EmployeeLogged';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient) { }

  getEmployee(){ 
  return this.http.get<Employee>(`${api.apiUrl}/Employees/logged`);
  }

  getEmployeeLogged() {
    return this.http.get<EmployeeLogged>(`${api.apiUrl}/Employees/profile`);    
  }

  addEmployee(employee) {
    const body = JSON.stringify(employee);
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json; charset=utf-8');   

        return this.http.post<any>(`${api.apiUrl}/employees/add`, body, {headers: headers})
            .pipe(map(x => {
                return x;
            }));
  }

  promoteDeveloper(username) {
    const body = "";
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');   

    return this.http.put<any>(`${api.apiUrl}/employees/prom/dev?username=` + username, body, {headers: headers})
        .pipe(map(x => {
            return x;
        }));
  }

  promoteLeader(username) {
    const body = "";
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');   

    return this.http.put<any>(`${api.apiUrl}/employees/prom/leader?username=` + username, body, {headers: headers})
        .pipe(map(x => {
            return x;
        }));
  }

  getActivityFeedUsers(){
      return this.http.get<any>(`${api.apiUrl}/team/get`);
  }

  getTeamLeaderForTeam(team) {
    return this.http.get<any>(`${api.apiUrl}/employees/get/leader?team=` + team);
  }

}
