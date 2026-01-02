import API_CONFIG, { getApiUrl } from "../../config/api";

describe("API Configuration", () => {
  const originalEnv = process.env;

  beforeEach(() => {
    // Reset process.env before each test
    jest.resetModules();
    process.env = { ...originalEnv };
  });

  afterEach(() => {
    // Restore original process.env
    process.env = originalEnv;
  });

  // Helper to safely modify process.env
  const setEnvVar = (key: string, value: string | undefined) => {
    if (value === undefined) {
      delete (process.env as Record<string, string | undefined>)[key];
    } else {
      (process.env as Record<string, string | undefined>)[key] = value;
    }
  };

  describe("API_CONFIG", () => {
    it("should have BASE_URL property", () => {
      expect(API_CONFIG).toHaveProperty("BASE_URL");
      expect(typeof API_CONFIG.BASE_URL).toBe("string");
    });

    it("should have ENDPOINTS property", () => {
      expect(API_CONFIG).toHaveProperty("ENDPOINTS");
      expect(typeof API_CONFIG.ENDPOINTS).toBe("object");
    });

    it("should have AUTH endpoints", () => {
      expect(API_CONFIG.ENDPOINTS).toHaveProperty("AUTH");
      expect(API_CONFIG.ENDPOINTS.AUTH).toHaveProperty("LOGIN");
      expect(API_CONFIG.ENDPOINTS.AUTH).toHaveProperty("REGISTER");
      expect(API_CONFIG.ENDPOINTS.AUTH).toHaveProperty("REFRESH_TOKEN");
    });

    it("should have correct AUTH endpoint values", () => {
      expect(API_CONFIG.ENDPOINTS.AUTH.LOGIN).toBe("/Auth/login");
      expect(API_CONFIG.ENDPOINTS.AUTH.REGISTER).toBe("/Auth/register");
      expect(API_CONFIG.ENDPOINTS.AUTH.REFRESH_TOKEN).toBe(
        "/Auth/refresh-token"
      );
    });

    it("should have PROJECTS endpoint", () => {
      expect(API_CONFIG.ENDPOINTS).toHaveProperty("PROJECTS");
      expect(API_CONFIG.ENDPOINTS.PROJECTS).toBe("/Projects");
    });

    it("should have TASKS endpoint", () => {
      expect(API_CONFIG.ENDPOINTS).toHaveProperty("TASKS");
      expect(API_CONFIG.ENDPOINTS.TASKS).toBe("/Tasks");
    });
  });

  describe("getApiBaseUrl", () => {
    it("should use REACT_APP_API_BASE_URL when set", () => {
      setEnvVar("REACT_APP_API_BASE_URL", "https://custom-api.example.com/api");
      setEnvVar("REACT_APP_API_URL", undefined);

      // Reload module to pick up new env vars
      jest.resetModules();
      const { API_CONFIG: newConfig } = require("../../config/api");

      expect(newConfig.BASE_URL).toBe("https://custom-api.example.com/api");
    });

    it("should use REACT_APP_API_URL when REACT_APP_API_BASE_URL is not set", () => {
      setEnvVar("REACT_APP_API_BASE_URL", undefined);
      setEnvVar("REACT_APP_API_URL", "https://fallback-api.example.com/api");

      // Reload module to pick up new env vars
      jest.resetModules();
      const { API_CONFIG: newConfig } = require("../../config/api");

      expect(newConfig.BASE_URL).toBe("https://fallback-api.example.com/api");
    });

    it("should prioritize REACT_APP_API_BASE_URL over REACT_APP_API_URL", () => {
      setEnvVar(
        "REACT_APP_API_BASE_URL",
        "https://priority-api.example.com/api"
      );
      setEnvVar("REACT_APP_API_URL", "https://fallback-api.example.com/api");

      // Reload module to pick up new env vars
      jest.resetModules();
      const { API_CONFIG: newConfig } = require("../../config/api");

      expect(newConfig.BASE_URL).toBe("https://priority-api.example.com/api");
    });

    it("should use default URL when no environment variables are set", () => {
      setEnvVar("REACT_APP_API_BASE_URL", undefined);
      setEnvVar("REACT_APP_API_URL", undefined);

      // Reload module to pick up new env vars
      jest.resetModules();
      const { API_CONFIG: newConfig } = require("../../config/api");

      expect(newConfig.BASE_URL).toBe("https://localhost:7272/api");
    });

    it("should handle empty string environment variables", () => {
      setEnvVar("REACT_APP_API_BASE_URL", "");
      setEnvVar("REACT_APP_API_URL", "");

      // Reload module to pick up new env vars
      jest.resetModules();
      const { API_CONFIG: newConfig } = require("../../config/api");

      // Empty strings are falsy, so should fall back to default
      expect(newConfig.BASE_URL).toBe("https://localhost:7272/api");
    });
  });

  describe("getApiUrl", () => {
    it("should concatenate BASE_URL with endpoint", () => {
      const endpoint = "/test-endpoint";
      const result = getApiUrl(endpoint);

      expect(result).toBe(`${API_CONFIG.BASE_URL}${endpoint}`);
    });

    it("should work with AUTH endpoints", () => {
      const loginUrl = getApiUrl(API_CONFIG.ENDPOINTS.AUTH.LOGIN);
      expect(loginUrl).toBe(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.AUTH.LOGIN}`
      );

      const registerUrl = getApiUrl(API_CONFIG.ENDPOINTS.AUTH.REGISTER);
      expect(registerUrl).toBe(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.AUTH.REGISTER}`
      );

      const refreshUrl = getApiUrl(API_CONFIG.ENDPOINTS.AUTH.REFRESH_TOKEN);
      expect(refreshUrl).toBe(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.AUTH.REFRESH_TOKEN}`
      );
    });

    it("should work with PROJECTS endpoint", () => {
      const projectsUrl = getApiUrl(API_CONFIG.ENDPOINTS.PROJECTS);
      expect(projectsUrl).toBe(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.PROJECTS}`
      );
    });

    it("should work with TASKS endpoint", () => {
      const tasksUrl = getApiUrl(API_CONFIG.ENDPOINTS.TASKS);
      expect(tasksUrl).toBe(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.TASKS}`
      );
    });

    it("should handle endpoints with leading slash", () => {
      const endpoint = "/endpoint";
      const result = getApiUrl(endpoint);

      expect(result).toBe(`${API_CONFIG.BASE_URL}/endpoint`);
    });

    it("should handle endpoints without leading slash", () => {
      const endpoint = "endpoint";
      const result = getApiUrl(endpoint);

      expect(result).toBe(`${API_CONFIG.BASE_URL}endpoint`);
    });

    it("should handle empty endpoint", () => {
      const result = getApiUrl("");

      expect(result).toBe(`${API_CONFIG.BASE_URL}`);
    });
  });
});
