import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_model/User';
import { PaginatedResult } from '../_model/Pagination';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getUsers(
    page?: number,
    itemsperPage?: number
  ): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<
      User[]
    >();
    let params = new HttpParams();

    if (page != null && itemsperPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsperPage.toString());
    }

    return this.http
      .get<User[]>(this.baseUrl + 'user/GetUsers', {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  getUser(id: string): Observable<User> {
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
