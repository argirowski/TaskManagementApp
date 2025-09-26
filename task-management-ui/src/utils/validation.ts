import * as Yup from "yup";

// Project validation schema
export const projectValidationSchema = Yup.object({
  projectName: Yup.string()
    .min(3, "Project name must be at least 3 characters")
    .max(30, "Project name must be less than 30 characters")
    .required("Project name is required"),
  projectDescription: Yup.string().max(
    200,
    "Description must be less than 200 characters"
  ),
});

export const tasksValidationSchema = Yup.object({
  projectTaskTitle: Yup.string()
    .min(3, "Task title must be at least 3 characters")
    .max(200, "Task title must be less than 30 characters")
    .required("Task title is required"),
  projectTaskDescription: Yup.string().max(
    1000,
    "Description must be less than 500 characters"
  ),
});

export const registerUserValidationSchema = Yup.object({
  UserName: Yup.string()
    .min(3, "Username must be at least 3 characters")
    .max(20, "Username must be less than 20 characters")
    .required("Username is required"),
  UserEmail: Yup.string()
    .email("Invalid email address")
    .required("Email is required"),
  Password: Yup.string()
    .min(6, "Password must be at least 6 characters")
    .matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/,
      "Password must contain at least one uppercase letter, one lowercase letter, and one number"
    )
    .required("Password is required"),
  confirmPassword: Yup.string()
    .oneOf([Yup.ref("Password")], "Passwords must match")
    .required("Confirm password is required"),
});

export const loginUserValidationSchema = Yup.object({
  email: Yup.string()
    .email("Invalid email address")
    .required("Email is required"),
  password: Yup.string()
    .min(6, "Password must be at least 6 characters")
    .required("Password is required"),
});
