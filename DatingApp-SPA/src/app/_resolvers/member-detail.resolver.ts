import { Injectable } from '@angular/core';
import { User } from '../_model/User';
import {
  Resolve,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MemberDetailResolver implements Resolve<User> {
  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): User | import('rxjs').Observable<User> | Promise<User> {
    return this.userService.getUser(route.paramMap.get('id')).pipe(
      catchError((error) => {
        this.alertify.error('Problem retrieving data');
        this.router.navigate(['/members']);
        return of(null);
      })
    );
  }

  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService
  ) {}
}
