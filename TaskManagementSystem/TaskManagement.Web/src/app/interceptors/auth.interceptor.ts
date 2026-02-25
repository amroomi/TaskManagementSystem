import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {  // ✅ HttpInterceptorFn
  const authService = inject(AuthService);
  const token = authService.getAuthHeader();
  
  console.log('Interceptor:', req.url, token ? 'OK' : 'No token');
  
  if (token) {
    return next(req.clone({
      setHeaders: { Authorization: token }
    }));
  }
  return next(req);
};