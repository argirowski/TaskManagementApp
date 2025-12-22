import * as Yup from "yup";

// Project validation schema
export const projectValidationSchema = Yup.object({
  projectName: Yup.string()
    .min(3, "Project name must be at least 3 characters")
    .max(50, "Project name must be less than 50 characters")
    .required("Project name is required"),
  projectDescription: Yup.string()
    .max(200, "Description must be less than 200 characters")
    .min(10, "Description must be at least 10 characters"),
});

export const tasksValidationSchema = Yup.object({
  projectTaskTitle: Yup.string()
    .required("Task title is required")
    .min(3, "Task title must be at least 3 characters")
    .max(100, "Task title must be less than 100 characters"),
  projectTaskDescription: Yup.string()
    .required("Description is required")
    .min(10, "Description must be at least 10 characters")
    .max(500, "Description must be less than 500 characters"),
});

export const registerUserValidationSchema = Yup.object({
  UserName: Yup.string()
    .required("Username is required")
    .min(3, "Username must be at least 3 characters")
    .max(20, "Username must be less than 20 characters"),
  UserEmail: Yup.string()
    .required("Email is required")
    .email("Invalid email address")
    .max(100, "Email must be less than 100 characters"),
  Password: Yup.string()
    .required("Password is required")
    .min(6, "Password must be at least 6 characters")
    .max(50, "Password must be less than 50 characters")
    .matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/,
      "Password must contain at least one uppercase letter, one lowercase letter, and one number"
    ),
  confirmPassword: Yup.string()
    .oneOf([Yup.ref("Password")], "Passwords must match")
    .required("Confirm password is required"),
});

export const loginUserValidationSchema = Yup.object({
  UserEmail: Yup.string()
    .required("Email is required")
    .email("Invalid email address")
    .max(100, "Email must be less than 100 characters"),
  Password: Yup.string()
    .required("Password is required")
    .min(6, "Password must be at least 6 characters")
    .max(50, "Password must be less than 50 characters")
    .matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/,
      "Password must contain at least one uppercase letter, one lowercase letter, and one number"
    ),
});
