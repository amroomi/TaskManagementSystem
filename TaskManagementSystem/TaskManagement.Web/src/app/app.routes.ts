import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { TaskManagerComponent } from './components/task-manager/task-manager.component';

export const routes: Routes = [

    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'tasks', component: TaskManagerComponent, canActivate: [AuthGuard] },
    // { path: 'tasks', component: TaskManagerComponent },
    { path: '**', redirectTo: '/login' }
];