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
  fetchProjects,
  fetchProject,
  fetchProjectDetails,
  createProject,
  updateProject,
  deleteProject,
} from "../../services/projectService";
// eslint-disable-next-line import/first
import {
  Project,
  ProjectFormData,
  ProjectDetailsDTO,
  PaginatedProjects,
} from "../../types/types";
// eslint-disable-next-line import/first
import api from "../../api/axios";
// eslint-disable-next-line import/first
import { AxiosError } from "axios";

const mockedApi = api as jest.Mocked<typeof api>;

describe("Project Service", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  describe("fetchProjects", () => {
    const mockPaginatedProjects: PaginatedProjects = {
      items: [
        {
          id: "project-1",
          projectName: "Project 1",
          projectDescription: "Description 1",
        },
        {
          id: "project-2",
          projectName: "Project 2",
          projectDescription: "Description 2",
        },
      ],
      page: 1,
      pageSize: 5,
      totalCount: 2,
      totalPages: 1,
    };

    it("should fetch paginated projects with default parameters", async () => {
      mockedApi.get.mockResolvedValueOnce({
        data: mockPaginatedProjects,
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await fetchProjects();

      expect(mockedApi.get).toHaveBeenCalledWith("/Projects", {
        params: { pageNumber: 1, pageSize: 5 },
      });
      expect(result).toEqual(mockPaginatedProjects);
    });

    it("should fetch paginated projects with custom parameters", async () => {
      mockedApi.get.mockResolvedValueOnce({
        data: mockPaginatedProjects,
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await fetchProjects(2, 10);

      expect(mockedApi.get).toHaveBeenCalledWith("/Projects", {
        params: { pageNumber: 2, pageSize: 10 },
      });
      expect(result).toEqual(mockPaginatedProjects);
    });

    it("should throw error when API call fails", async () => {
      const error = new AxiosError("Network Error");
      mockedApi.get.mockRejectedValueOnce(error);

      await expect(fetchProjects()).rejects.toThrow();
      expect(mockedApi.get).toHaveBeenCalledWith("/Projects", {
        params: { pageNumber: 1, pageSize: 5 },
      });
    });

    it("should throw error for 401 Unauthorized", async () => {
      const error = new AxiosError("Unauthorized");
      error.response = {
        status: 401,
        statusText: "Unauthorized",
        data: { error: "Unauthorized access" },
        headers: {},
        config: {} as any,
      };
      mockedApi.get.mockRejectedValueOnce(error);

      await expect(fetchProjects()).rejects.toThrow();
    });
  });

  describe("fetchProject", () => {
    const projectId = "project-123";
    const mockProject: Project = {
      id: projectId,
      projectName: "Test Project",
      projectDescription: "Test Description",
    };

    it("should fetch a single project successfully", async () => {
      mockedApi.get.mockResolvedValueOnce({
        data: mockProject,
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await fetchProject(projectId);

      expect(mockedApi.get).toHaveBeenCalledWith(`/Projects/${projectId}`);
      expect(result).toEqual(mockProject);
    });

    it("should throw error when API call fails", async () => {
      const error = new AxiosError("Network Error");
      mockedApi.get.mockRejectedValueOnce(error);

      await expect(fetchProject(projectId)).rejects.toThrow();
      expect(mockedApi.get).toHaveBeenCalledWith(`/Projects/${projectId}`);
    });

    it("should throw error for 404 Not Found", async () => {
      const error = new AxiosError("Not Found");
      error.response = {
        status: 404,
        statusText: "Not Found",
        data: { error: "Project not found" },
        headers: {},
        config: {} as any,
      };
      mockedApi.get.mockRejectedValueOnce(error);

      await expect(fetchProject(projectId)).rejects.toThrow();
    });
  });

  describe("fetchProjectDetails", () => {
    const projectId = "project-123";
    const mockProjectDetails: ProjectDetailsDTO = {
      projectName: "Test Project",
      projectDescription: "Test Description",
      users: [
        {
          id: "user-1",
          userName: "john_doe",
          userEmail: "john@example.com",
        },
      ],
      tasks: [
        {
          id: "task-1",
          projectTaskTitle: "Task 1",
          projectTaskDescription: "Task Description 1",
        },
      ],
    };

    it("should fetch project details successfully", async () => {
      mockedApi.get.mockResolvedValueOnce({
        data: mockProjectDetails,
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await fetchProjectDetails(projectId);

      expect(mockedApi.get).toHaveBeenCalledWith(`/Projects/${projectId}`);
      expect(result).toEqual(mockProjectDetails);
    });

    it("should throw error when API call fails", async () => {
      const error = new AxiosError("Network Error");
      mockedApi.get.mockRejectedValueOnce(error);

      await expect(fetchProjectDetails(projectId)).rejects.toThrow();
      expect(mockedApi.get).toHaveBeenCalledWith(`/Projects/${projectId}`);
    });

    it("should throw error for 404 Not Found", async () => {
      const error = new AxiosError("Not Found");
      error.response = {
        status: 404,
        statusText: "Not Found",
        data: { error: "Project not found" },
        headers: {},
        config: {} as any,
      };
      mockedApi.get.mockRejectedValueOnce(error);

      await expect(fetchProjectDetails(projectId)).rejects.toThrow();
    });
  });

  describe("createProject", () => {
    const projectData: ProjectFormData = {
      projectName: "New Project",
      projectDescription: "New Project Description",
    };
    const mockCreatedProject: Project = {
      id: "project-123",
      projectName: "New Project",
      projectDescription: "New Project Description",
    };

    it("should create a project successfully", async () => {
      mockedApi.post.mockResolvedValueOnce({
        data: mockCreatedProject,
        status: 201,
        statusText: "Created",
        headers: {},
        config: {} as any,
      });

      const result = await createProject(projectData);

      expect(mockedApi.post).toHaveBeenCalledWith("/Projects", projectData);
      expect(result).toEqual(mockCreatedProject);
    });

    it("should throw error when API call fails", async () => {
      const error = new AxiosError("Network Error");
      mockedApi.post.mockRejectedValueOnce(error);

      await expect(createProject(projectData)).rejects.toThrow();
      expect(mockedApi.post).toHaveBeenCalledWith("/Projects", projectData);
    });

    it("should throw error for 400 Bad Request", async () => {
      const error = new AxiosError("Bad Request");
      error.response = {
        status: 400,
        statusText: "Bad Request",
        data: { error: "Project name already exists" },
        headers: {},
        config: {} as any,
      };
      mockedApi.post.mockRejectedValueOnce(error);

      await expect(createProject(projectData)).rejects.toThrow();
    });
  });

  describe("updateProject", () => {
    const projectId = "project-123";
    const projectData: ProjectFormData = {
      projectName: "Updated Project",
      projectDescription: "Updated Description",
    };
    const mockUpdatedProject: Project = {
      id: projectId,
      projectName: "Updated Project",
      projectDescription: "Updated Description",
    };

    it("should update a project successfully", async () => {
      mockedApi.put.mockResolvedValueOnce({
        data: mockUpdatedProject,
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await updateProject(projectId, projectData);

      expect(mockedApi.put).toHaveBeenCalledWith(
        `/Projects/${projectId}`,
        projectData
      );
      expect(result).toEqual(mockUpdatedProject);
    });

    it("should throw error when API call fails", async () => {
      const error = new AxiosError("Network Error");
      mockedApi.put.mockRejectedValueOnce(error);

      await expect(updateProject(projectId, projectData)).rejects.toThrow();
      expect(mockedApi.put).toHaveBeenCalledWith(
        `/Projects/${projectId}`,
        projectData
      );
    });

    it("should throw error for 404 Not Found", async () => {
      const error = new AxiosError("Not Found");
      error.response = {
        status: 404,
        statusText: "Not Found",
        data: { error: "Project not found" },
        headers: {},
        config: {} as any,
      };
      mockedApi.put.mockRejectedValueOnce(error);

      await expect(updateProject(projectId, projectData)).rejects.toThrow();
    });

    it("should throw error for 403 Forbidden", async () => {
      const error = new AxiosError("Forbidden");
      error.response = {
        status: 403,
        statusText: "Forbidden",
        data: { error: "You don't have permission to update this project" },
        headers: {},
        config: {} as any,
      };
      mockedApi.put.mockRejectedValueOnce(error);

      await expect(updateProject(projectId, projectData)).rejects.toThrow();
    });
  });

  describe("deleteProject", () => {
    const projectId = "project-123";

    it("should delete a project successfully", async () => {
      mockedApi.delete.mockResolvedValueOnce({
        data: undefined,
        status: 204,
        statusText: "No Content",
        headers: {},
        config: {} as any,
      });

      await deleteProject(projectId);

      expect(mockedApi.delete).toHaveBeenCalledWith(`/Projects/${projectId}`);
    });

    it("should throw error when API call fails", async () => {
      const error = new AxiosError("Network Error");
      mockedApi.delete.mockRejectedValueOnce(error);

      await expect(deleteProject(projectId)).rejects.toThrow();
      expect(mockedApi.delete).toHaveBeenCalledWith(`/Projects/${projectId}`);
    });

    it("should throw error for 404 Not Found", async () => {
      const error = new AxiosError("Not Found");
      error.response = {
        status: 404,
        statusText: "Not Found",
        data: { error: "Project not found" },
        headers: {},
        config: {} as any,
      };
      mockedApi.delete.mockRejectedValueOnce(error);

      await expect(deleteProject(projectId)).rejects.toThrow();
    });

    it("should throw error for 403 Forbidden", async () => {
      const error = new AxiosError("Forbidden");
      error.response = {
        status: 403,
        statusText: "Forbidden",
        data: { error: "You don't have permission to delete this project" },
        headers: {},
        config: {} as any,
      };
      mockedApi.delete.mockRejectedValueOnce(error);

      await expect(deleteProject(projectId)).rejects.toThrow();
    });
  });
});
