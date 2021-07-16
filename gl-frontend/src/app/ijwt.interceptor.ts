import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginService } from './_srvc/login.service';
import { api } from './api'

@Injectable()
export class IjwtInterceptor implements HttpInterceptor {

  constructor(private loginService: LoginService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        const authToken = this.loginService.getAuthToken;
        const logged = authToken && request.url.startsWith(api.apiUrl);
        if (logged) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${authToken}`
                }
            });
        }
    return next.handle(request);
  }
}
