import axios from "axios";
import { Project, ProjectFormData, ProjectDetailsDTO } from "../types/types";

const BASE_URL = "https://localhost:7272/api/Projects";

// No auth persistence in services: this file performs plain data fetching.
// If you later want auth headers, pass the token in from callers or read
// from a chosen storage strategy here.
const handleAuthError = (error: any) => {
  // Keep behaviour simple: rethrow and let callers handle navigation/cleanup.
  throw error;
};

// Get all projects
export const fetchProjects = async (): Promise<Project[]> => {
  try {
    const response = await axios.get(BASE_URL);
    return response.data;
  } catch (error) {
    console.error("Error fetching projects:", error);
    throw handleAuthError(error);
  }
};

// Get single project by ID (basic info)
export const fetchProject = async (projectId: string): Promise<Project> => {
  try {
    const response = await axios.get(`${BASE_URL}/${projectId}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching project:", error);
    throw handleAuthError(error);
  }
};

// Get project details with tasks and users
export const fetchProjectDetails = async (
  projectId: string
): Promise<ProjectDetailsDTO> => {
  try {
    const response = await axios.get(`${BASE_URL}/${projectId}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching project details:", error);
    throw handleAuthError(error);
  }
};

// Create new project
export const createProject = async (
  projectData: ProjectFormData
): Promise<Project> => {
  try {
    const response = await axios.post(BASE_URL, projectData);
    return response.data;
  } catch (error) {
    console.error("Error creating project:", error);
    throw handleAuthError(error);
  }
};

// Update existing project
export const updateProject = async (
  projectId: string,
  projectData: ProjectFormData
): Promise<Project> => {
  try {
    const response = await axios.put(`${BASE_URL}/${projectId}`, projectData);
    return response.data;
  } catch (error) {
    console.error("Error updating project:", error);
    throw handleAuthError(error);
  }
};

// Delete project
export const deleteProject = async (projectId: string): Promise<void> => {
  try {
    await axios.delete(`${BASE_URL}/${projectId}`);
  } catch (error) {
    console.error("Error deleting project:", error);
    throw handleAuthError(error);
  }
};
