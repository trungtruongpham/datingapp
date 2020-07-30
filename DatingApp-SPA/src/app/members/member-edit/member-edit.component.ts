import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_model/User';
import { ActivationEnd, ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private alertify: AlertifyService
  ) {}
  @ViewChild('editForm') editForm: NgForm;
  user: User;
  @HostListener('window:beforeunload', ['$event'])
  // tslint:disable-next-line: typedef
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.user = data.user;
    });
  }

  updateUser(): void {
    console.log(this.user);
    this.alertify.success('update successful');
    this.editForm.reset(this.user);
  }
}
