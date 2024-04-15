import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { User } from '../_modules/user';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http : HttpClient ) { }

  getUsersWithRoles() {
    return this.http.get<User[]>(this.baseUrl + 'admin/users-with-roles');
  }

  updateUserRoles(username: string, roles: string[]) {
  return this.http.post<string[]>(this.baseUrl + 'admin/edit-roles/'
    + username + '?roles=' + roles, {});
  }

}

