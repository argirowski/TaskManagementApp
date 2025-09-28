import axios from "axios";
import { SingleTaskDTO, TaskFormData } from "../types/types";
import store from "../store/store";
import { logout } from "../store/actions/authActions";

const BASE_URL = "https://localhost:7272/api/Tasks";

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

// Get single task by project and task ID
export const fetchTask = async (
  projectId: string,
  taskId: string
): Promise<SingleTaskDTO> => {
  try {
    const response = await axios.get(
      `${BASE_URL}/project/${projectId}/task/${taskId}`,
      { headers: getAuthHeaders() }
    );
    return response.data;
  } catch (error) {
    console.error("Error fetching task:", error);
    return handleAuthError(error);
  }
};

// Create new task for a project
export const createTask = async (
  projectId: string,
  taskData: TaskFormData
): Promise<SingleTaskDTO> => {
  try {
    const response = await axios.post(
      `${BASE_URL}/project/${projectId}`,
      taskData,
      { headers: getAuthHeaders() }
    );
    return response.data;
  } catch (error) {
    console.error("Error creating task:", error);
    return handleAuthError(error);
  }
};

// Update existing task
export const updateTask = async (
  projectId: string,
  taskId: string,
  taskData: TaskFormData
): Promise<SingleTaskDTO> => {
  try {
    const response = await axios.put(
      `${BASE_URL}/project/${projectId}/task/${taskId}`,
      taskData,
      { headers: getAuthHeaders() }
    );
    return response.data;
  } catch (error) {
    console.error("Error updating task:", error);
    return handleAuthError(error);
  }
};

// Delete task
export const deleteTask = async (
  projectId: string,
  taskId: string
): Promise<void> => {
  try {
    await axios.delete(`${BASE_URL}/project/${projectId}/task/${taskId}`, {
      headers: getAuthHeaders(),
    });
  } catch (error) {
    console.error("Error deleting task:", error);
    return handleAuthError(error);
  }
};
