import { Component } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { User } from '../_modules/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {
  model: any = {};
  userData$: Observable<User | null>;

  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.userData$ = this.accountService.currentUser$;
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => {
        this.router.navigateByUrl('/members'),
        this.model = {};
      }
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
