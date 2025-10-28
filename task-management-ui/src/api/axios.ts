import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7272/api",
  headers: {
    "Content-Type": "application/json",
  },
});

// Attach auth token from localStorage to every request if present
api.interceptors.request.use(
  (config) => {
    try {
      const token = localStorage.getItem("authToken");
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

export default api;
