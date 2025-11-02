import React from "react";
import { Routes, Route } from "react-router-dom";
import HomePage from "../pages/HomePage";
import LoginForm from "../features/auth/LoginForm";
import RegisterForm from "../features/auth/RegisterForm";
import ProjectList from "../features/projects/ProjectList";
import ProjectForm from "../features/projects/ProjectForm";
import ProjectCard from "../features/projects/ProjectCard";
import TaskView from "../features/tasks/TaskView";
import TaskForm from "../features/tasks/TaskForm";
import ProtectedRoute from "../components/auth/ProtectedRoute";

const AppRoutes: React.FC = () => {
  return (
    <Routes>
      {/* Public Routes */}
      <Route path="/" element={<HomePage />} />
      <Route path="/login" element={<LoginForm />} />
      <Route path="/register" element={<RegisterForm />} />

      {/* Protected Routes (nested under ProtectedRoute) */}
      <Route element={<ProtectedRoute />}>
        <Route path="/projects" element={<ProjectList />} />
        <Route path="/projects/new" element={<ProjectForm />} />
        <Route path="/projects/:id/edit" element={<ProjectForm />} />
        <Route
          path="/projects/:projectId/tasks/:taskId"
          element={<TaskView />}
        />
        <Route path="/projects/:projectId/tasks/new" element={<TaskForm />} />
        <Route
          path="/projects/:projectId/tasks/:taskId/edit"
          element={<TaskForm />}
        />
        <Route path="/projects/:id" element={<ProjectCard />} />
      </Route>
    </Routes>
  );
};

export default AppRoutes;
