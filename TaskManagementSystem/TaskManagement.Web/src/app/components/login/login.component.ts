import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

    username = '';
    password = '';
    errorMessage = '';

    constructor(private authService: AuthService, private router: Router) {}

    onLogin(): void {
        
        this.errorMessage = '';

        this.authService.login(this.username, this.password).subscribe({
            next: (success) => {
            if (success) {
                this.router.navigate(['/tasks']);
            } else {
                this.errorMessage = 'Invalid credentials';
            }
            },
            error: () => {
            this.errorMessage = 'Login failed';
            }
        });
    }
}