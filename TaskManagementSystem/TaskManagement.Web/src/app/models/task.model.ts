export interface Task {

    id: number;
    title: string;
    description?: string;
    statusId: number;
    statusName: string;
    createdByUserId: number;
    createdByUsername: string;
}

export interface CreateTask {

    title: string;
    description?: string;
    statusId: number;
}

export interface UpdateTask {
    
    id: number;
    title: string;
    description?: string;
    statusId: number;
}