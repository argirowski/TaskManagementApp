import { Dispatch } from "redux";
import axios from "axios";
import {
  LOGIN_REQUEST,
  LOGIN_SUCCESS,
  LOGIN_FAILURE,
  LOGOUT,
  REFRESH_TOKEN_REQUEST,
  REFRESH_TOKEN_SUCCESS,
  REFRESH_TOKEN_FAILURE,
  SET_AUTH_LOADING,
  CLEAR_AUTH_ERROR,
  AuthActionTypes,
  User,
} from "../types/authTypes";
import { LoginFormData } from "../../types/types";

const API_BASE_URL = "https://localhost:7272/api";

// Action Creators
export const loginRequest = (): AuthActionTypes => ({
  type: LOGIN_REQUEST,
});

export const loginSuccess = (
  user: User,
  token: string,
  refreshToken?: string,
  tokenExpiry?: number
): AuthActionTypes => ({
  type: LOGIN_SUCCESS,
  payload: { user, token, refreshToken, tokenExpiry },
});

export const loginFailure = (error: string): AuthActionTypes => ({
  type: LOGIN_FAILURE,
  payload: error,
});

export const logout = (): AuthActionTypes => {
  // Clear localStorage on logout
  localStorage.removeItem("authToken");
  localStorage.removeItem("refreshToken");
  localStorage.removeItem("user");
  localStorage.removeItem("tokenExpiry");

  return {
    type: LOGOUT,
  };
};

export const refreshTokenRequest = (): AuthActionTypes => ({
  type: REFRESH_TOKEN_REQUEST,
});

export const refreshTokenSuccess = (
  token: string,
  refreshToken?: string,
  tokenExpiry?: number
): AuthActionTypes => ({
  type: REFRESH_TOKEN_SUCCESS,
  payload: { token, refreshToken, tokenExpiry },
});

export const refreshTokenFailure = (error: string): AuthActionTypes => ({
  type: REFRESH_TOKEN_FAILURE,
  payload: error,
});

export const setAuthLoading = (loading: boolean): AuthActionTypes => ({
  type: SET_AUTH_LOADING,
  payload: loading,
});

export const clearAuthError = (): AuthActionTypes => ({
  type: CLEAR_AUTH_ERROR,
});

// Thunk Action Creators
export const loginUser = (credentials: LoginFormData) => {
  return async (dispatch: Dispatch<AuthActionTypes>) => {
    dispatch(loginRequest());

    try {
      const response = await axios.post(
        `${API_BASE_URL}/Auth/login`,
        credentials
      );

      const { user, token, refreshToken } = response.data;

      // Calculate token expiry (assuming 1 hour if not provided)
      const tokenExpiry = Date.now() + 60 * 60 * 1000; // 1 hour from now

      // Store in localStorage
      localStorage.setItem("authToken", token);
      localStorage.setItem("user", JSON.stringify(user));
      localStorage.setItem("tokenExpiry", tokenExpiry.toString());

      if (refreshToken) {
        localStorage.setItem("refreshToken", refreshToken);
      }

      dispatch(loginSuccess(user, token, refreshToken, tokenExpiry));
    } catch (error: any) {
      let errorMessage = "Login failed. Please try again.";

      if (error.response?.data?.message) {
        errorMessage = error.response.data.message;
      } else if (error.response?.status === 401) {
        errorMessage = "Invalid email or password";
      }

      dispatch(loginFailure(errorMessage));
    }
  };
};

export const refreshAuthToken = () => {
  return async (dispatch: Dispatch<AuthActionTypes>) => {
    dispatch(refreshTokenRequest());

    try {
      const refreshToken = localStorage.getItem("refreshToken");

      if (!refreshToken) {
        throw new Error("No refresh token available");
      }

      const response = await axios.post(`${API_BASE_URL}/Auth/refresh`, {
        refreshToken,
      });

      const { token, refreshToken: newRefreshToken } = response.data;
      const tokenExpiry = Date.now() + 60 * 60 * 1000; // 1 hour from now

      // Update localStorage
      localStorage.setItem("authToken", token);
      localStorage.setItem("tokenExpiry", tokenExpiry.toString());

      if (newRefreshToken) {
        localStorage.setItem("refreshToken", newRefreshToken);
      }

      dispatch(refreshTokenSuccess(token, newRefreshToken, tokenExpiry));
    } catch (error: any) {
      // Refresh failed, user needs to login again
      dispatch(refreshTokenFailure("Session expired. Please login again."));
      dispatch(logout());
    }
  };
};

export const initializeAuth = () => {
  return async (dispatch: Dispatch<AuthActionTypes>) => {
    dispatch(setAuthLoading(true));

    try {
      const token = localStorage.getItem("authToken");
      const user = localStorage.getItem("user");
      const tokenExpiry = localStorage.getItem("tokenExpiry");
      const refreshToken = localStorage.getItem("refreshToken");

      if (token && user && tokenExpiry) {
        const parsedUser = JSON.parse(user);
        const expiryTime = parseInt(tokenExpiry);

        // Check if token is expired
        if (Date.now() >= expiryTime) {
          // Token expired, try to refresh
          if (refreshToken) {
            await dispatch(refreshAuthToken() as any);
          } else {
            // No refresh token, logout
            dispatch(logout());
          }
        } else {
          // Token still valid
          dispatch(
            loginSuccess(
              parsedUser,
              token,
              refreshToken || undefined,
              expiryTime
            )
          );
        }
      }
    } catch (error) {
      // Error initializing auth, logout
      dispatch(logout());
    } finally {
      dispatch(setAuthLoading(false));
    }
  };
};
