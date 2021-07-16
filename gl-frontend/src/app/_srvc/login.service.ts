import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, first} from 'rxjs/operators';
import { Employee } from '../_models/Employee';
import { api } from '../api';

@Injectable({ providedIn: 'root' })
export class LoginService {
    private userAuthTokenSubject: BehaviorSubject<String>;
    public userAuthToken: Observable<String>;
    private logged: boolean;
    
    constructor(
        private router: Router,
        private http: HttpClient,
    ) {
        this.userAuthTokenSubject = new BehaviorSubject<String>(localStorage.getItem('authToken'));
        this.userAuthToken = this.userAuthTokenSubject.asObservable();
        
    }

    public get isLogged(): boolean {
        return this.logged;
    }
    public get getAuthToken(): String {
        return this.userAuthTokenSubject.value;
    }
    public get getLoggedEmployee(): Observable<Employee> {
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json; charset=utf-8'); 
        return this.http.post<Employee>(`${api.apiUrl}/employees/logged`, "", {headers: headers})
        .pipe(map(emp => {
            return emp;
        }));
    }
    
   

    login(username, password) {

        const body = JSON.stringify({username: username, password: password});
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json; charset=utf-8');   

        return this.http.post<any>(`${api.apiUrl}/login`, body, {headers: headers})
            .pipe(map(auth => { 
                localStorage.setItem('authToken', auth.token);
                this.userAuthTokenSubject.next(auth.token);
                return auth;
            }));
    }

    logout() {
        //this.logged = false;
        this.userAuthTokenSubject.next(null);
        localStorage.removeItem('authToken');
        this.router.navigate(['/login']);
    }  

    reset(mail){
        const body = JSON.stringify({mail:mail});
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json; charset=utf-8');   
        return this.http.post<any>(`${api.apiUrl}/forgot`, body, {headers: headers})
            .pipe(map(x => {
                return x;
            }));
    }

    changePassword(Username, NewPassword, OldPassword){
        const body = JSON.stringify({Username: Username, OldPassword: OldPassword, NewPassword: NewPassword});
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json; charset=utf-8');   
        return this.http.put<any>(`${api.apiUrl}/Employees/updateProfile`, body, {headers: headers})
            .pipe(map(x => {
                return x;
            }));
    }

    sendNewPassword(password,guid){
        const body = JSON.stringify({password:password,guid:guid});
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json; charset=utf-8');   
        return this.http.post<any>(`${api.apiUrl}/change-password`, body, {headers: headers})
            .pipe(map(x => {
                return x;
            }));
    }
}