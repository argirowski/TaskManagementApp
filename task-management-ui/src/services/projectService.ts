import axios from "axios";
import { Project, ProjectFormData, ProjectDetailsDTO } from "../types/types";

const BASE_URL = "https://localhost:7272/api/Projects";

// Get all projects
export const fetchProjects = async (): Promise<Project[]> => {
  try {
    const response = await axios.get(BASE_URL);
    return response.data;
  } catch (error) {
    console.error("Error fetching projects:", error);
    throw error;
  }
};

// Get single project by ID (basic info)
export const fetchProject = async (projectId: string): Promise<Project> => {
  try {
    const response = await axios.get(`${BASE_URL}/${projectId}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching project:", error);
    throw error;
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
    throw error;
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
    throw error;
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
    throw error;
  }
};

// Delete project
export const deleteProject = async (projectId: string): Promise<void> => {
  try {
    await axios.delete(`${BASE_URL}/${projectId}`);
  } catch (error) {
    console.error("Error deleting project:", error);
    throw error;
  }
};
