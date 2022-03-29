import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserLoginDto } from 'src/app/models/user-login-dto.model';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public loginForm: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required])
  });

  public hide = true;
  public isInLoadingState = false;

  constructor(
    private router: Router, 
    private accountService: AccountService) { }

  ngOnInit(): void {
  }

  public onRegister(): void {
    this.router.navigateByUrl('/register');
  }

  public onSubmit(): void {
    if (!this.loginForm.valid) {
      return;
    }

    this.isInLoadingState = true;
    this.accountService.login(this.loginForm.value as UserLoginDto).subscribe({
      next: response => {
        this.isInLoadingState = false;
        
        if (this.accountService.isAuthenticated) {
          this.router.navigateByUrl('/navigation/home');
        }
      },
      error: () => this.isInLoadingState = false
    })
  }

}
