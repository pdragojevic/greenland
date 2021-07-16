import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { LoginService } from './_srvc/login.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private loginService: LoginService
) {}
  canActivate( route: ActivatedRouteSnapshot, state: RouterStateSnapshot){
    const authed = this.loginService.getAuthToken;
        if (authed) {
            return true;
        }
        this.router.navigate(['/login'], { queryParams: { backTo: state.url }});
        return false;
  }
  
}
