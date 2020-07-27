import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_model/User';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
  user: User;
  id: string = this.route.snapshot.paramMap.get('id');

  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser(): void {
    this.userService.getUser(this.id).subscribe(
      (user: User) => {
        
        this.user = user;
        this.alertify.success('Get user success');
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }
}
