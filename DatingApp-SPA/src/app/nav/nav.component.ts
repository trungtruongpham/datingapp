import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(private authService: AuthService) { }

  // tslint:disable-next-line: typedef
  ngOnInit() {
  }

  login(): void{
    this.authService.login(this.model).subscribe(next => {
      console.log('login successfully');
    }, error => {
      console.log('failed to login');
    });
  }

  loggedIn(): boolean{
    const token = localStorage.getItem('token');
    return !!token;
  }

  logOut(): void{
    localStorage.removeItem('token');
    console.log('log out');
  }
}
