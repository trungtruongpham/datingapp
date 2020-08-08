import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_model/User';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'user/GetUsers');
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'user/GetUser/' + id);
  }

  // tslint:disable-next-line: typedef
  updateUser(id: string, user: User) {
    return this.http.put(this.baseUrl + 'user/UpdateUser/' + id, user);
  }

  // tslint:disable-next-line: typedef
  setMainPhoto(userId: string, id: string) {
    return this.http.post(
      this.baseUrl + 'user/' + userId + '/photos/' + id + '/setMain',
      {}
    );
  }

  // tslint:disable-next-line: typedef
  deletePhoto(userId: string, id: string) {
    return this.http.delete(this.baseUrl + 'user/' + userId + '/photos/' + id);
  }
}
