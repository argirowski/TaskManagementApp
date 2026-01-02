// Mock the axios instance - must be before imports
jest.mock("../../api/axios", () => {
  return {
    __esModule: true,
    default: {
      get: jest.fn(),
      post: jest.fn(),
      put: jest.fn(),
      delete: jest.fn(),
    },
  };
});

// eslint-disable-next-line import/first
import {
  fetchTask,
  createTask,
  updateTask,
  deleteTask,
} from "../../services/taskService";
// eslint-disable-next-line import/first
import { SingleTaskDTO, TaskFormData } from "../../types/types";
// eslint-disable-next-line import/first
import api from "../../api/axios";
// eslint-disable-next-line import/first
import { AxiosError } from "axios";

const mockedApi = api as jest.Mocked<typeof api>;

describe("Task Service", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  describe("fetchTask", () => {
    const projectId = "project-123";
    const taskId = "task-456";
    const mockTask: SingleTaskDTO = {
      projectTaskTitle: "Test Task",
      projectTaskDescription: "This is a test task description",
    };

    it("should fetch a task successfully", async () => {
      mockedApi.get.mockResolvedValueOnce({
        data: mockTask,
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await fetchTask(projectId, taskId);

      expect(mockedApi.get).toHaveBeenCalledWith(
        `/Tasks/project/${projectId}/task/${taskId}`
      );
      expect(result).toEqual(mockTask);
    });

    it("should throw error when API call fails", async () => {
      const error = new AxiosError("Network Error");
      mockedApi.get.mockRejectedValueOnce(error);

      await expect(fetchTask(projectId, taskId)).rejects.toThrow();
      expect(mockedApi.get).toHaveBeenCalledWith(
        `/Tasks/project/${projectId}/task/${taskId}`
      );
    });

    it("should throw error for 404 Not Found", async () => {
      const error = new AxiosError("Not Found");
      error.response = {
        status: 404,
        statusText: "Not Found",
        data: { error: "Task not found" },
        headers: {},
        config: {} as any,
      };
      mockedApi.get.mockRejectedValueOnce(error);

      await expect(fetchTask(projectId, taskId)).rejects.toThrow();
    });
  });

  describe("createTask", () => {
    const projectId = "project-123";
    const taskData: TaskFormData = {
      projectTaskTitle: "New Task",
      projectTaskDescription: "This is a new task description",
    };
    const mockCreatedTask: SingleTaskDTO = {
      projectTaskTitle: "New Task",
      projectTaskDescription: "This is a new task description",
    };

    it("should create a task successfully", async () => {
      mockedApi.post.mockResolvedValueOnce({
        data: mockCreatedTask,
        status: 201,
        statusText: "Created",
        headers: {},
        config: {} as any,
      });

      const result = await createTask(projectId, taskData);

      expect(mockedApi.post).toHaveBeenCalledWith(
        `/Tasks/project/${projectId}`,
        taskData
      );
      expect(result).toEqual(mockCreatedTask);
    });

    it("should throw error when API call fails", async () => {
      const error = new AxiosError("Network Error");
      mockedApi.post.mockRejectedValueOnce(error);

      await expect(createTask(projectId, taskData)).rejects.toThrow();
      expect(mockedApi.post).toHaveBeenCalledWith(
        `/Tasks/project/${projectId}`,
        taskData
      );
    });

    it("should throw error for 400 Bad Request", async () => {
      const error = new AxiosError("Bad Request");
      error.response = {
        status: 400,
        statusText: "Bad Request",
        data: { error: "Invalid task data" },
        headers: {},
        config: {} as any,
      };
      mockedApi.post.mockRejectedValueOnce(error);

      await expect(createTask(projectId, taskData)).rejects.toThrow();
    });
  });

  describe("updateTask", () => {
    const projectId = "project-123";
    const taskId = "task-456";
    const taskData: TaskFormData = {
      projectTaskTitle: "Updated Task",
      projectTaskDescription: "This is an updated task description",
    };
    const mockUpdatedTask: SingleTaskDTO = {
      projectTaskTitle: "Updated Task",
      projectTaskDescription: "This is an updated task description",
    };

    it("should update a task successfully", async () => {
      mockedApi.put.mockResolvedValueOnce({
        data: mockUpdatedTask,
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await updateTask(projectId, taskId, taskData);

      expect(mockedApi.put).toHaveBeenCalledWith(
        `/Tasks/project/${projectId}/task/${taskId}`,
        taskData
      );
      expect(result).toEqual(mockUpdatedTask);
    });

    it("should throw error when API call fails", async () => {
      const error = new AxiosError("Network Error");
      mockedApi.put.mockRejectedValueOnce(error);

      await expect(updateTask(projectId, taskId, taskData)).rejects.toThrow();
      expect(mockedApi.put).toHaveBeenCalledWith(
        `/Tasks/project/${projectId}/task/${taskId}`,
        taskData
      );
    });

    it("should throw error for 404 Not Found", async () => {
      const error = new AxiosError("Not Found");
      error.response = {
        status: 404,
        statusText: "Not Found",
        data: { error: "Task not found" },
        headers: {},
        config: {} as any,
      };
      mockedApi.put.mockRejectedValueOnce(error);

      await expect(updateTask(projectId, taskId, taskData)).rejects.toThrow();
    });
  });

  describe("deleteTask", () => {
    const projectId = "project-123";
    const taskId = "task-456";

    it("should delete a task successfully", async () => {
      mockedApi.delete.mockResolvedValueOnce({
        data: undefined,
        status: 204,
        statusText: "No Content",
        headers: {},
        config: {} as any,
      });

      await deleteTask(projectId, taskId);

      expect(mockedApi.delete).toHaveBeenCalledWith(
        `/Tasks/project/${projectId}/task/${taskId}`
      );
    });

    it("should throw error when API call fails", async () => {
      const error = new AxiosError("Network Error");
      mockedApi.delete.mockRejectedValueOnce(error);

      await expect(deleteTask(projectId, taskId)).rejects.toThrow();
      expect(mockedApi.delete).toHaveBeenCalledWith(
        `/Tasks/project/${projectId}/task/${taskId}`
      );
    });

    it("should throw error for 404 Not Found", async () => {
      const error = new AxiosError("Not Found");
      error.response = {
        status: 404,
        statusText: "Not Found",
        data: { error: "Task not found" },
        headers: {},
        config: {} as any,
      };
      mockedApi.delete.mockRejectedValueOnce(error);

      await expect(deleteTask(projectId, taskId)).rejects.toThrow();
    });

    it("should throw error for 403 Forbidden", async () => {
      const error = new AxiosError("Forbidden");
      error.response = {
        status: 403,
        statusText: "Forbidden",
        data: { error: "You don't have permission to delete this task" },
        headers: {},
        config: {} as any,
      };
      mockedApi.delete.mockRejectedValueOnce(error);

      await expect(deleteTask(projectId, taskId)).rejects.toThrow();
    });
  });
});
