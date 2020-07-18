import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  baseUrl = 'http://localhost:5000/api/auth/';

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
            console.log(localStorage.getItem('token'));
          }
        })
      );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'Register', model);
  }
}

