import axios, { AxiosError, InternalAxiosRequestConfig } from "axios";
import { getToken, isTokenExpired } from "../utils/auth";
import { refreshAccessToken } from "../services/authService";

const api = axios.create({
  baseURL: "https://localhost:7272/api",
  headers: {
    "Content-Type": "application/json",
  },
});

let isRefreshing = false;
let failedQueue: Array<{
  resolve: (value?: any) => void;
  reject: (error?: any) => void;
}> = [];

const processQueue = (error: any, token: string | null = null) => {
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
        // TS: headers can be a plain object or AxiosHeaders; cast to any for assignment
        (config.headers as any)["Authorization"] = `Bearer ${token}`;
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
    if (error.response?.status === 401 && originalRequest && !originalRequest._retry) {
      // If we're already refreshing, queue this request
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        })
          .then((token) => {
            if (originalRequest.headers) {
              (originalRequest.headers as any)["Authorization"] = `Bearer ${token}`;
            }
            return api(originalRequest);
          })
          .catch((err) => {
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
            (originalRequest.headers as any)["Authorization"] = `Bearer ${newTokenData.accessToken}`;
          }

          // Process queued requests
          processQueue(null, newTokenData.accessToken);

          // Retry the original request
          return api(originalRequest);
        } else {
          // Refresh failed - clear tokens and redirect to login
          processQueue(new Error("Token refresh failed"), null);
          
          // Only redirect if we're in the browser and not already on login page
          if (typeof window !== "undefined" && !window.location.pathname.includes("/login")) {
            window.location.href = "/login";
          }
          
          return Promise.reject(error);
        }
      } catch (refreshError) {
        // Refresh failed - clear tokens and redirect to login
        processQueue(refreshError, null);
        
        // Only redirect if we're in the browser and not already on login page
        if (typeof window !== "undefined" && !window.location.pathname.includes("/login")) {
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
