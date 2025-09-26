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

export interface SingleTaskDTO {
  projectTaskTitle: string;
  projectTaskDescription: string;
}

export interface ProjectDetailsDTO {
  projectName: string;
  projectDescription?: string;
  users: UserDetailsDTO[];
  tasks: TaskDetailsDTO[];
}

export interface LoaderComponentProps {
  message?: string;
  variant?:
    | "primary"
    | "secondary"
    | "success"
    | "danger"
    | "warning"
    | "info"
    | "light"
    | "dark";
  fullScreen?: boolean;
}

export interface EmptyStateComponentProps {
  title: string;
  message?: string;
  actionText?: string;
  onAction?: () => void;
  icon?: React.ReactNode;
}

export interface ConfirmDialogComponentProps {
  show: boolean;
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
  onConfirm: () => void;
  onCancel: () => void;
  variant?: "danger" | "primary" | "warning";
}

export interface AlertComponentProps {
  show: boolean;
  variant:
    | "success"
    | "danger"
    | "warning"
    | "info"
    | "primary"
    | "secondary"
    | "light"
    | "dark";
  message: string;
  dismissible?: boolean;
  onClose?: () => void;
}

export interface LoginFormData {
  email: string;
  password: string;
}

export interface RegisterFormData {
  UserName: string;
  UserEmail: string;
  Password: string;
  confirmPassword: string;
}

export interface ProjectFormData {
  projectName: string;
  projectDescription: string;
}

export interface Project extends ProjectFormData {
  id: string;
}

export interface TaskFormData {
  projectTaskTitle: string;
  projectTaskDescription: string;
}
