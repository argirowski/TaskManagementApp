import axios from "axios";
import { SingleTaskDTO, TaskFormData } from "../types/types";

const BASE_URL = "https://localhost:7272/api/Tasks";

// Get single task by project and task ID
export const fetchTask = async (
  projectId: string,
  taskId: string
): Promise<SingleTaskDTO> => {
  try {
    const response = await axios.get(
      `${BASE_URL}/project/${projectId}/task/${taskId}`
    );
    return response.data;
  } catch (error) {
    console.error("Error fetching task:", error);
    throw error;
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
      taskData
    );
    return response.data;
  } catch (error) {
    console.error("Error creating task:", error);
    throw error;
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
      taskData
    );
    return response.data;
  } catch (error) {
    console.error("Error updating task:", error);
    throw error;
  }
};

// Delete task
export const deleteTask = async (
  projectId: string,
  taskId: string
): Promise<void> => {
  try {
    await axios.delete(`${BASE_URL}/project/${projectId}/task/${taskId}`);
  } catch (error) {
    console.error("Error deleting task:", error);
    throw error;
  }
};
