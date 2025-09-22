import React from "react";
import { Routes, Route } from "react-router-dom";
import HomePage from "../pages/HomePage";
import LoginForm from "../features/auth/LoginForm";
import RegisterForm from "../features/auth/RegisterForm";
import ProjectList from "../features/projects/ProjectList";
import ProjectForm from "../features/projects/ProjectForm";
import ProjectCard from "../features/projects/ProjectCard";

const AppRoutes: React.FC = () => {
  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/login" element={<LoginForm />} />
      <Route path="/register" element={<RegisterForm />} />
      <Route path="/projects" element={<ProjectList />} />
      <Route path="/projects/new" element={<ProjectForm />} />
      <Route path="/projects/:id/edit" element={<ProjectForm />} />
      <Route path="/projects/:id" element={<ProjectCard />} />
    </Routes>
  );
};

export default AppRoutes;
