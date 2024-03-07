import { Component } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { User } from '../_modules/user';
import { Observable, of } from 'rxjs';
import { Router } from '@angular/router';
import { Toast, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  model: any = {};
  userData: User | null = localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user') || '{}') : null;

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => this.router.navigateByUrl('/members'),
      error: error => this.toastr.error("Invalid username or password. Please try again.")
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}
