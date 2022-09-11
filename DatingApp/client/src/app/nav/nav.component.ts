import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs/internal/Observable';
import { User } from '../models/user';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  loggedIn: boolean = false;
  model: any = {};
  currentUser$?: Observable<User>;

  constructor(
    private router: Router,
    private accountService: AccountService,
    private toastr: ToastrService
  ) {
    this.currentUser$ = this.accountService.currentUser$;
  }

  ngOnInit(): void {
    // this.getCurrentUser();
  }
  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/members');
        console.log(response);
      },
      error: ({ error }) => {
        if (!(error.errors && typeof error.errors === 'object')) {
          return this.toastr.error(error) as any;
        }
        for (const key in error.errors) {
          this.toastr.error(error.errors[key]);
        }
      },
    });
  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

  // getCurrentUser() {
  //   this.accountService.currentUser$.subscribe({
  //     next: (user) => {
  //       this.loggedIn = !!user;
  //       console.log('first');
  //     },
  //     error: (error) => {
  //       console.log(error);
  //     },
  //   });
  // }
}
