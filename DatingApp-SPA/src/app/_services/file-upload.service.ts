import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { AlertifyService } from './alertify.service';

@Injectable({
  providedIn: 'root',
})
export class FileUploadService {
  baseUrl = environment.apiUrl;

  constructor(
    private httpClient: HttpClient,
    private authService: AuthService,
    private altertify: AlertifyService
  ) {}

  uploadFile(fileToUpload: File): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('File', fileToUpload, fileToUpload.name);
    return this.httpClient.post(
      this.baseUrl + 'user/' + this.authService.decodedToken.nameid + '/photos',
      formData
    );
  }
}
