export interface Project {
  id: string;
  projectName: string;
  projectDescription: string;
}

export interface UserDTO {
  userName: string;
  userEmail: string;
  password?: string;
}

export interface TaskDTO {
  projectTaskTitle: string;
  projectTaskDescription: string;
}

export interface UserDetailsDTO {
  id: string;
  userName: string;
  userEmail: string;
  password?: string;
}

export interface TaskDetailsDTO {
  id: string;
  projectTaskTitle: string;
  projectTaskDescription: string;
}

export interface ProjectDetailsDTO {
  projectName: string;
  projectDescription?: string;
  users: UserDetailsDTO[];
  tasks: TaskDetailsDTO[];
}
