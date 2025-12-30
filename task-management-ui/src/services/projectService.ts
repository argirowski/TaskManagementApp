import api from "../api/axios";
import {
  Project,
  ProjectFormData,
  ProjectDetailsDTO,
  PaginatedProjects,
} from "../types/types";

const BASE_URL = "/Projects";

// No auth persistence in services: this file performs plain data fetching.
// If you later want auth headers, pass the token in from callers or read
// from a chosen storage strategy here.
const handleAuthError = (error: any) => {
  // Keep behaviour simple: rethrow and let callers handle navigation/cleanup.
  throw error;
};

// Get paginated projects
export const fetchProjects = async (
  pageNumber = 1,
  pageSize = 5
): Promise<PaginatedProjects> => {
  try {
    const response = await api.get(BASE_URL, {
      params: { pageNumber, pageSize },
    });
    return response.data;
  } catch (error) {
    throw handleAuthError(error);
  }
};

// Get single project by ID (basic info)
export const fetchProject = async (projectId: string): Promise<Project> => {
  try {
    const response = await api.get(`${BASE_URL}/${projectId}`);
    return response.data;
  } catch (error) {
    throw handleAuthError(error);
  }
};

// Get project details with tasks and users
export const fetchProjectDetails = async (
  projectId: string
): Promise<ProjectDetailsDTO> => {
  try {
    const response = await api.get(`${BASE_URL}/${projectId}`);
    return response.data;
  } catch (error) {
    throw handleAuthError(error);
  }
};

// Create new project
export const createProject = async (
  projectData: ProjectFormData
): Promise<Project> => {
  try {
    const response = await api.post(BASE_URL, projectData);
    return response.data;
  } catch (error) {
    throw handleAuthError(error);
  }
};

// Update existing project
export const updateProject = async (
  projectId: string,
  projectData: ProjectFormData
): Promise<Project> => {
  try {
    const response = await api.put(`${BASE_URL}/${projectId}`, projectData);
    return response.data;
  } catch (error) {
    throw handleAuthError(error);
  }
};

// Delete project
export const deleteProject = async (projectId: string): Promise<void> => {
  try {
    await api.delete(`${BASE_URL}/${projectId}`);
  } catch (error) {
    throw handleAuthError(error);
  }
};
