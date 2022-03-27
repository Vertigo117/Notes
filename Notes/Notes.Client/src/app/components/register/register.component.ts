import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, FormGroupDirective, NgForm, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserLoginDto } from 'src/app/models/user-login-dto.model';
import { UserUpsertDto } from 'src/app/models/user-upsert-dto.model';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  public registerForm: FormGroup = new FormGroup({
    name: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    confirmPassword: new FormControl('', [Validators.required, this.confirmPassword()])
  });

  public hide = true;
  public isInLoadingState = false;

  constructor(private router: Router, private accountService: AccountService) { }

  ngOnInit(): void {
  }

  public onSubmit(): void {
    if (this.registerForm.invalid) {
      return;
    }

    this.isInLoadingState = true;
    this.accountService.register(this.registerForm.value as UserUpsertDto).subscribe({
      next: response => {
        let credentials: UserLoginDto = {
          password: this.registerForm.controls['password'].value,
          email: response.email
        }
        this.accountService.login(credentials).subscribe({
          next: () => {
            if (this.accountService.isAuthenticated) {
              this.router.navigateByUrl('/navigation/home');
            }
          },
          error: () => this.isInLoadingState = false
        })
      },
      error: () => this.isInLoadingState = false
    });
  }

  public onCancel(): void {
    this.router.navigateByUrl('/login');
  }

  private confirmPassword(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password: string = control.parent?.get('password')?.value;
      const confirmPassword: string = control.value;
      return password !== confirmPassword ? { confirmPassword: true } : null
    }
  }

}
