import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Employee } from '../_models/Employee';
import { Task } from '../_models/Task';
import { api } from '../api';
import { map } from 'rxjs/operators';
import { Iteration } from '../_models/Iterations';
import { TaskEmployee } from '../_models/TaskEmployee';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private http: HttpClient) { }

  getAll(team){ //tested
    var url = `${api.apiUrl}/tasks/get`;
    if(team != -1) {
      url += `?team=` + team;
    }
     return this.http.get<any>(url);
  }

  getTaskBetween(team, start, end) { //tested
    var url = `${api.apiUrl}/tasks/between?start=`+ start + `&end=` + end;
    if(team != -1) {
      url += `&team=` + team;
    }
    return this.http.get<any>(url)
  }

  getUserActivity() { 
    return this.http.get<any>(`${api.apiUrl}/activityfeed/get`)
  }

  insertTask(Task) { //tested
    const body = JSON.stringify(Task);
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json; charset=utf-8');   

        return this.http.post<Task>(`${api.apiUrl}/tasks`, body, {headers: headers})
            .pipe(map(x => { 
              return x;
            }));
  }

  getFirstIteration()  {
    return this.http.get<Iteration>(`${api.apiUrl}/iterations/get/current`);        
  }

  getIterations(iteration) {
    const body = iteration;
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');   

    return this.http.post<Array<Iteration>>(`${api.apiUrl}/iterations/get`, body, {headers: headers})
        .pipe(map(x => { 
          return x;
        }));
  }

  deleteTask(id) { //prima id taska i brise ga
    return this.http.delete<any>(`${api.apiUrl}/tasks/` + id); //ako je sve okej vraca status 204 deleted
  }


  updateTaskStatus(id, newStatus) { 
    return this.http.put<any>(`${api.apiUrl}/tasks/updatestatus?id=` + id + "&newStatus=" + newStatus, " ");
  }

  updateTask(task) {
    const body = JSON.stringify(task);
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');   

    return this.http.put<any>(`${api.apiUrl}/tasks/update`, body, {headers: headers})
        .pipe(map(x => { 
          return x;
        }));
  }

  updateTaskEmployee(idTask, idEmployee) {
    var te = new TaskEmployee();
    te.IdTask = idTask;
    te.IdEmployee = idEmployee;
    
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');   

    const body = JSON.stringify(te);
    return this.http.put<any>(`${api.apiUrl}/tasks/update/task/employee`, body, {headers: headers})
        .pipe(map(x => { 
          return x; 
        }));
  }


  getTaskEmployee(idTask) {
    return this.http.get<any>(`${api.apiUrl}/get/taskemployee`) //ovo vraca bas TaskEmployee objekt
  }

  getEmployeeForTask(idTask) { //ovo vraca employea koji radi na tasku sa idTask
    return this.http.get<any>(`${api.apiUrl}/tasks/get/employee/task?taskId=${idTask}`);
  }





}
