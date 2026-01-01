import axios, { AxiosError, InternalAxiosRequestConfig } from "axios";
import { getToken } from "../utils/auth";
import { refreshAccessToken } from "../services/authService";
import { API_CONFIG } from "../config/api";

const api = axios.create({
  baseURL: API_CONFIG.BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

let isRefreshing = false;
let failedQueue: Array<{
  resolve: (value?: string | null) => void;
  reject: (error?: unknown) => void;
}> = [];

const processQueue = (error: unknown, token: string | null = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
};

// Attach auth token from localStorage to every request if present
api.interceptors.request.use(
  (config) => {
    try {
      const token = getToken();
      if (token && config && config.headers) {
        // TypeScript limitation: Axios headers can be AxiosHeaders or plain object
        // Using type assertion for header assignment
        (config.headers as Record<string, string>)[
          "Authorization"
        ] = `Bearer ${token}`;
      }
    } catch (e) {
      // ignore localStorage errors (e.g., in some environments)
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Handle 401 errors and automatically refresh token
api.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const originalRequest = error.config as InternalAxiosRequestConfig & {
      _retry?: boolean;
    };

    // If error is 401 and we haven't retried yet
    if (
      error.response?.status === 401 &&
      originalRequest &&
      !originalRequest._retry
    ) {
      // If we're already refreshing, queue this request
      if (isRefreshing) {
        return new Promise<string | null>((resolve, reject) => {
          failedQueue.push({
            resolve: (token?: string | null) => resolve(token ?? null),
            reject: (error?: unknown) => reject(error),
          });
        })
          .then((token) => {
            if (originalRequest.headers && token) {
              // TypeScript limitation: Axios headers can be AxiosHeaders or plain object
              // Using type assertion for header assignment
              (originalRequest.headers as Record<string, string>)[
                "Authorization"
              ] = `Bearer ${token}`;
            }
            return api(originalRequest);
          })
          .catch((err: unknown) => {
            return Promise.reject(err);
          });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const newTokenData = await refreshAccessToken();

        if (newTokenData && newTokenData.accessToken) {
          // Update the original request with new token
          if (originalRequest.headers) {
            // TypeScript limitation: Axios headers can be AxiosHeaders or plain object
            // Using type assertion for header assignment
            (originalRequest.headers as Record<string, string>)[
              "Authorization"
            ] = `Bearer ${newTokenData.accessToken}`;
          }

          // Process queued requests
          processQueue(null, newTokenData.accessToken);

          // Retry the original request
          return api(originalRequest);
        } else {
          // Refresh failed - clear tokens and redirect to login
          processQueue(new Error("Token refresh failed"), null);

          // Only redirect if we're in the browser and not already on login page
          if (
            typeof window !== "undefined" &&
            !window.location.pathname.includes("/login")
          ) {
            window.location.href = "/login";
          }

          return Promise.reject(error);
        }
      } catch (refreshError) {
        // Refresh failed - clear tokens and redirect to login
        processQueue(refreshError, null);

        // Only redirect if we're in the browser and not already on login page
        if (
          typeof window !== "undefined" &&
          !window.location.pathname.includes("/login")
        ) {
          window.location.href = "/login";
        }

        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    }

    // For non-401 errors or if retry already attempted, just reject
    return Promise.reject(error);
  }
);

export default api;
