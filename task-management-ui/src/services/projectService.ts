import axios from "axios";
import { Project, ProjectFormData, ProjectDetailsDTO } from "../types/types";
import store from "../store/store";
import { logout } from "../store/actions/authActions";

const BASE_URL = "https://localhost:7272/api/Projects";

// Get auth headers with current token from Redux store
const getAuthHeaders = () => {
  const state = store.getState();
  const token = state.auth.token;

  return token ? { Authorization: `Bearer ${token}` } : {};
};

// Handle auth errors globally
const handleAuthError = (error: any) => {
  if (error.response?.status === 401 || error.response?.status === 403) {
    // Token is invalid or expired, logout user
    store.dispatch(logout());
  }
  throw error;
};

// Get all projects
export const fetchProjects = async (): Promise<Project[]> => {
  try {
    const response = await axios.get(BASE_URL, {
      headers: getAuthHeaders(),
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching projects:", error);
    return handleAuthError(error);
  }
};

// Get single project by ID (basic info)
export const fetchProject = async (projectId: string): Promise<Project> => {
  try {
    const response = await axios.get(`${BASE_URL}/${projectId}`, {
      headers: getAuthHeaders(),
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching project:", error);
    return handleAuthError(error);
  }
};

// Get project details with tasks and users
export const fetchProjectDetails = async (
  projectId: string
): Promise<ProjectDetailsDTO> => {
  try {
    const response = await axios.get(`${BASE_URL}/${projectId}`, {
      headers: getAuthHeaders(),
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching project details:", error);
    return handleAuthError(error);
  }
};

// Create new project
export const createProject = async (
  projectData: ProjectFormData
): Promise<Project> => {
  try {
    const response = await axios.post(BASE_URL, projectData, {
      headers: getAuthHeaders(),
    });
    return response.data;
  } catch (error) {
    console.error("Error creating project:", error);
    return handleAuthError(error);
  }
};

// Update existing project
export const updateProject = async (
  projectId: string,
  projectData: ProjectFormData
): Promise<Project> => {
  try {
    const response = await axios.put(`${BASE_URL}/${projectId}`, projectData, {
      headers: getAuthHeaders(),
    });
    return response.data;
  } catch (error) {
    console.error("Error updating project:", error);
    return handleAuthError(error);
  }
};

// Delete project
export const deleteProject = async (projectId: string): Promise<void> => {
  try {
    await axios.delete(`${BASE_URL}/${projectId}`, {
      headers: getAuthHeaders(),
    });
  } catch (error) {
    console.error("Error deleting project:", error);
    return handleAuthError(error);
  }
};
