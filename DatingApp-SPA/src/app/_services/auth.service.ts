import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) { }

  // tslint:disable-next-line: typedef
  login(model: any) {
    return this.http.post(this.baseUrl + 'Login', model)
      .pipe(
        map((response: any) => {
          const token = response;
          console.log(token);
          if (token) {
            localStorage.setItem('token', response);
            this.decodedToken = this.jwtHelper.decodeToken(token);
            console.log(this.decodedToken);
          }
        })
      );
  }

  register(model: any): any {
    return this.http.post(this.baseUrl + 'Register', model);
  }

  loggedIn(): boolean {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}

