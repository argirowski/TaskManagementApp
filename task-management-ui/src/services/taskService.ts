import api from "../api/axios";
import { SingleTaskDTO, TaskFormData } from "../types/types";

const BASE_URL = "/Tasks";

// No auth persistence in services: perform plain data fetching.
// If auth headers are needed later, pass token from callers or implement
// a storage strategy here.
const handleAuthError = (error: unknown): never => {
  // Simple: rethrow and let callers handle navigation/cleanup.
  throw error;
};

// Get single task by project and task ID
export const fetchTask = async (
  projectId: string,
  taskId: string
): Promise<SingleTaskDTO> => {
  try {
    const response = await api.get(
      `${BASE_URL}/project/${projectId}/task/${taskId}`
    );
    return response.data;
  } catch (error) {
    throw handleAuthError(error);
  }
};

// Create new task for a project
export const createTask = async (
  projectId: string,
  taskData: TaskFormData
): Promise<SingleTaskDTO> => {
  try {
    const response = await api.post(
      `${BASE_URL}/project/${projectId}`,
      taskData
    );
    return response.data;
  } catch (error) {
    throw handleAuthError(error);
  }
};

// Update existing task
export const updateTask = async (
  projectId: string,
  taskId: string,
  taskData: TaskFormData
): Promise<SingleTaskDTO> => {
  try {
    const response = await api.put(
      `${BASE_URL}/project/${projectId}/task/${taskId}`,
      taskData
    );
    return response.data;
  } catch (error) {
    throw handleAuthError(error);
  }
};

// Delete task
export const deleteTask = async (
  projectId: string,
  taskId: string
): Promise<void> => {
  try {
    await api.delete(`${BASE_URL}/project/${projectId}/task/${taskId}`);
  } catch (error) {
    throw handleAuthError(error);
  }
};
