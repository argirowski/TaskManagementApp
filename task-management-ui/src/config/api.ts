/**
 * API Configuration
 * Reads from environment variables with fallback to default values
 */

const getApiBaseUrl = (): string => {
  // React apps require REACT_APP_ prefix for environment variables
  return (
    process.env.REACT_APP_API_BASE_URL ||
    process.env.REACT_APP_API_URL ||
    "https://localhost:7272/api"
  );
};

export const API_CONFIG = {
  BASE_URL: getApiBaseUrl(),
  // API endpoints
  ENDPOINTS: {
    AUTH: {
      LOGIN: "/Auth/login",
      REGISTER: "/Auth/register",
      REFRESH_TOKEN: "/Auth/refresh-token",
    },
    PROJECTS: "/Projects",
    TASKS: "/Tasks",
  },
} as const;

// Helper function to build full API URL
export const getApiUrl = (endpoint: string): string => {
  return `${API_CONFIG.BASE_URL}${endpoint}`;
};

export default API_CONFIG;
