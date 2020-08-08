import {
  Component,
  OnInit,
  Input,
  ViewChild,
  Output,
  EventEmitter,
} from '@angular/core';
import { Photo } from 'src/app/_model/Photo';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FileUpload } from 'primeng/fileupload';
import { FileUploadService } from 'src/app/_services/file-upload.service';
import { UserService } from 'src/app/_services/user.service';
import { User } from 'src/app/_model/User';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() getMemberPhotoChange = new EventEmitter<string>();
  @ViewChild('fileInput') fileInput: FileUpload;
  uploadedFiles: any[] = [];
  baseUrl = environment.apiUrl;
  url: string;
  currentMainPhoto: Photo;
  user: User;

  constructor(
    private authService: AuthService,
    private alertifyService: AlertifyService,
    private fileUploadService: FileUploadService,
    private userService: UserService
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

  setMainPhoto(photo: Photo): void {
    this.userService
      .setMainPhoto(this.authService.decodedToken.nameid, photo.id)
      .subscribe(
        () => {
          this.currentMainPhoto = this.photos.filter(
            (p) => p.isMain === true
          )[0];
          this.currentMainPhoto.isMain = false;
          photo.isMain = true;
          this.authService.changeMemberPhoto(photo.url);
          this.authService.currentUser.photoUrl = photo.url;
          localStorage.setItem(
            'user',
            JSON.stringify(this.authService.currentUser)
          );
        },
        (error) => {
          this.alertifyService.error(error);
        }
      );
  }

  deletePhoto(id: string): void {
    this.alertifyService.confirm(
      'Are you sure you want to delete this photo?',
      () => {
        this.userService
          .deletePhoto(this.authService.decodedToken.nameid, id)
          .subscribe(
            () => {
              this.photos.splice(
                this.photos.findIndex((p) => p.id === id),
                1
              );
              this.alertifyService.success('Photo has been deleted');
            },
            (error) => {
              this.alertifyService.error('Failed to delete the photo');
            }
          );
      }
    );
  }
}
