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

    // onLogin(): void {

    //     if (this.username === 'admin' && this.password === '123456') {

    //         this.authService.login();
    //         this.router.navigate(['/tasks']);
    //     }
    //     else {
            
    //         this.errorMessage = 'Invalid credentials';
    //     }
    // }

    // onLogin(): void {

    //     this.authService.login(this.username, this.password).subscribe({

    //         next: (success) => {

    //             if (success) {

    //                 this.router.navigate(['/tasks']);
    //             }
    //             else {

    //                 this.errorMessage = 'Invalid credentials';
    //             }
    //         }
    //     });
    // }

    onLogin(): void {
        
        // this.isLoading = true;
        this.errorMessage = '';

        // ✅ Calls corrected service
        this.authService.login(this.username, this.password).subscribe({
            next: (success) => {
            // this.isLoading = false;
            if (success) {
                this.router.navigate(['/tasks']);
            } else {
                this.errorMessage = 'Invalid credentials';
            }
            },
            error: () => {
            // this.isLoading = false;
            this.errorMessage = 'Login failed';
            }
        });
    }
}