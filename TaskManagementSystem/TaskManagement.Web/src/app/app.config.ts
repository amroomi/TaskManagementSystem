import { ApplicationConfig } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';  // ✅ withInterceptors
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app.routes';
import { AuthInterceptor } from './interceptors/auth.interceptor';  // ✅ Functional interceptor

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding()),
    
    // ✅ HttpInterceptorFn → withInterceptors(array)
    provideHttpClient(
      withInterceptors([AuthInterceptor])  // ✅ Direct array
    ),
    
    provideAnimationsAsync()
  ]
};