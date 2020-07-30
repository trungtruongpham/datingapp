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

  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  user: User;
  images: any[];
  activeIndex = 2;
  id: string = this.route.snapshot.paramMap.get('id');

  responsiveOptions: any[] = [
    {
        breakpoint: '1024px',
        numVisible: 5
    },
    {
        breakpoint: '768px',
        numVisible: 3
    },
    {
        breakpoint: '560px',
        numVisible: 1
    }
];

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.user = data.user;
      this.images = data.user.photos;
    });
  }

  getActiveIndex(): number {
    return this.activeIndex;
  }

  setActiveIndex(newValue): void {
    if (this.images && 0 <= newValue && newValue <= this.images.length - 1) {
      this.activeIndex = newValue;
    }
  }
}
