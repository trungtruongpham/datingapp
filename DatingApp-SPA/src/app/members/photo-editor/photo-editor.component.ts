import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Photo } from 'src/app/_model/Photo';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FileUploadModule, FileUpload } from 'primeng/fileupload';
import { CoreEnvironment } from '@angular/compiler/src/compiler_facade_interface';
import { FileUploadService } from 'src/app/_services/file-upload.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @ViewChild('fileInput') fileInput: FileUpload;
  uploadedFiles: any[] = [];
  baseUrl = environment.apiUrl;
  url: string;

  constructor(
    private authService: AuthService,
    private alertifyService: AlertifyService,
    private fileUploadService: FileUploadService
  ) {
    this.url =
      this.baseUrl + 'user/' + this.authService.decodedToken.nameid + '/photos';
  }

  ngOnInit(): void {}

  selectHandler(): void {
    console.log(this.fileInput.files);
    console.log(this.fileInput.name);
  }

  uploadHandler(): void {
    console.log(this.uploadedFiles);
    this.fileUploadService.uploadFile(this.fileInput.files[0]).subscribe(
      () => {
        this.alertifyService.success('Upload file success');
      },
      (error) => {
        this.alertifyService.error(error);
      }
    );
  }
}
