import { Component } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { User } from '../_modules/user';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  model: any = {};
  userData: User | null = localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user') || '{}') : null;

  constructor(public accountService: AccountService) { }

  ngOnInit() {
  }
  
  login() {
    this.accountService.login(this.model).subscribe({
      next: response => { 
      console.log(response)
    }, 
    error: error => console.log(error)
    })
  }

  logout() {
    this.accountService.logout();
  }

}
