import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
// import { RouterLink } from '@angular/router';
import { Subject, takeUntil, finalize, switchMap, Observable, mapTo } from 'rxjs';
import { TaskStatus } from '../../models/status.model';
import { Task, CreateTask, UpdateTask } from '../../models/task.model';
import { AuthService } from '../../services/auth.service';
import { StatusService } from '../../services/status.service';
import { TaskService } from '../../services/task.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-task-manager',
  standalone: true,
//   imports: [CommonModule, RouterLink, FormsModule],
  imports: [CommonModule, FormsModule],
  templateUrl: './task-manager.component.html',
  styleUrl: './task-manager.component.scss'
})
export class TaskManagerComponent implements OnInit, OnDestroy {

    tasks: Task[] = [];
    filteredTasks: Task[] = [];
    statuses: TaskStatus[] = [];
    searchTerm = '';
    isLoading = false;
    editingTask: Task | null = null;

    // Form data for template binding
    formTitle = '';
    formDescription = '';
    formStatusId = 0;

    private destroy$ = new Subject<void>();
    username = '';

    constructor(
        private taskService: TaskService,
        private statusService: StatusService,
        private authService: AuthService,
        private router: Router
    ) {}

    ngOnInit(): void {

        this.loadData();
    }

    ngOnDestroy(): void {
        
        this.destroy$.next();
        this.destroy$.complete();
    }

    loadData(): void {

        this.loadStatuses();
        this.loadTasks();
    }

    loadStatuses(): void {

        this.statusService.getStatuses()
        .pipe(takeUntil(this.destroy$))
        .subscribe({

            next: (statuses) => this.statuses = statuses,
            error: (err) => console.error('Error loading statuses:', err)
        });
    }

    loadTasks(): void {

        this.isLoading = true;

        this.taskService.getTasks()
        .pipe(
            takeUntil(this.destroy$),
            finalize(() => this.isLoading = false)
        )
        .subscribe({
            next: (tasks) => {

                this.tasks = tasks;
                this.filteredTasks = tasks;
            },
            error: (err) => console.error('Error loading tasks:', err)
        });
    }

    filterTasks(): void {

        this.filteredTasks = this.tasks.filter(task =>

            task.title.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
            task.statusName.toLowerCase().includes(this.searchTerm.toLowerCase())
        );
    }

    editTask(task: Task): void {

        this.editingTask = task;
        this.formTitle = task.title;
        this.formDescription = task.description || '';
        this.formStatusId = task.statusId;
    }

    resetForm(): void {

        this.editingTask = null;
        this.formTitle = '';
        this.formDescription = '';
        this.formStatusId = 0;
    }

    // saveTask(form: NgForm): void {

    //     debugger;

    //     if (form.invalid || !this.formTitle || this.formStatusId === 0) return;

    //     this.isLoading = true;

    //     const taskData: CreateTask = {

    //         title: this.formTitle.trim(),
    //         description: this.formDescription.trim() || undefined,
    //         statusId: this.formStatusId
    //     };

    //     const request = this.editingTask ? this.taskService.updateTask({ ...taskData, id: this.editingTask.id } as UpdateTask) : this.taskService.createTask(taskData);

    //     request.pipe(

    //       switchMap(() => this.taskService.getTasks()),
    //       takeUntil(this.destroy$),
    //       finalize(() => this.isLoading = false)
    //     )
    //     .subscribe({
    //       next: (tasks) => {
    //         this.tasks = tasks;
    //         this.filteredTasks = tasks;
    //         this.resetForm();
    //         form.resetForm();
    //       },
    //       error: (err) => {
    //         console.error('Error saving task:', err);
    //         alert('Error saving task. Check console.');
    //       }
    //     });
    // }

    saveTask(form: NgForm): void {

        if (form.invalid || !this.formTitle || this.formStatusId === 0) return;

        this.isLoading = true;

        const taskData: CreateTask = {
            title: this.formTitle.trim(),
            description: this.formDescription.trim() || undefined,
            statusId: this.formStatusId
        };

        const request$: Observable<void> = this.editingTask ? this.taskService.updateTask({ ...taskData, id: this.editingTask.id } as UpdateTask).pipe(mapTo(void 0)) : this.taskService.createTask(taskData).pipe(mapTo(void 0));

        request$.pipe(
            switchMap(() => this.taskService.getTasks()),
            takeUntil(this.destroy$),
            finalize(() => this.isLoading = false)
        )
        .subscribe({
            next: (tasks: Task[]) => {
                
                this.tasks = tasks;
                this.filteredTasks = tasks;
                this.resetForm();
                form.resetForm();
            },
            error: (err: any) => {
                console.error('Error saving task:', err);
            }
        });
    }

    deleteTask(id: number): void {

        if (!confirm('Are you sure you want to delete this task?')) return;

        this.isLoading = true;

        this.taskService.deleteTask(id).pipe(

            switchMap(() => this.taskService.getTasks()),
            takeUntil(this.destroy$),
            finalize(() => this.isLoading = false)
        )
        .subscribe({

            next: (tasks) => {

                this.tasks = tasks;
                this.filteredTasks = tasks;
            },
            error: (err) => console.error('Error deleting task:', err)
        });
    }

    logout(): void {

        this.authService.logout();

        this.router.navigate(['/login']);
    }

    getStatusClass(statusName: string): string {

        const classes: { [key: string]: string } = {

            'Draft': 'badge-light',
            'Pending': 'badge-secondary',
            'InProgress': 'badge-primary',
            'Completed': 'badge-success',
            'Cancelled': 'badge-danger'
        };

        return classes[statusName] || '';
    }
}