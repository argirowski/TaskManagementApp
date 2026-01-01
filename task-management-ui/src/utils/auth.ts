import { TokenData } from "../types/types";

const TOKEN_KEY = "authToken";
const REFRESH_TOKEN_KEY = "refreshToken";
const USER_NAME_KEY = "userName";
const USER_ID_KEY = "userId";

// Claim types for JWT decoding
const ClaimTypes = {
  NameIdentifier:
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
};

/**
 * Decode JWT token to extract payload
 */
export const decodeToken = (token: string): any => {
  try {
    const base64Url = token.split(".")[1];
    const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split("")
        .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
        .join("")
    );
    return JSON.parse(jsonPayload);
  } catch (e) {
    // Silently fail - token decoding errors are expected for invalid tokens
    return null;
  }
};

/**
 * Extract userId from JWT token
 */
export const getUserIdFromToken = (token: string | null): string | null => {
  if (!token) return null;
  try {
    const decoded = decodeToken(token);
    // JWT contains userId as NameIdentifier claim
    return (
      decoded[ClaimTypes.NameIdentifier] ||
      decoded.nameid ||
      decoded.sub ||
      null
    );
  } catch (e) {
    // Silently fail - userId extraction errors are expected for invalid tokens
    return null;
  }
};

export const setToken = (token: string) => {
  try {
    localStorage.setItem(TOKEN_KEY, token);
    // Extract and store userId from token
    const userId = getUserIdFromToken(token);
    if (userId) {
      localStorage.setItem(USER_ID_KEY, userId);
    }
  } catch (e) {
    // ignore storage errors
    // You may want to surface this in production
  }
};

export const setTokenData = (data: TokenData) => {
  try {
    localStorage.setItem(TOKEN_KEY, data.accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, data.refreshToken);
    localStorage.setItem(USER_NAME_KEY, data.userName);
    // Store userId if provided, otherwise extract from token
    const userId = data.userId || getUserIdFromToken(data.accessToken);
    if (userId) {
      localStorage.setItem(USER_ID_KEY, userId);
    }
  } catch (e) {
    // ignore storage errors
    // You may want to surface this in production
  }
};

export const getToken = (): string | null => {
  try {
    return localStorage.getItem(TOKEN_KEY);
  } catch (e) {
    return null;
  }
};

export const getRefreshToken = (): string | null => {
  try {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  } catch (e) {
    return null;
  }
};

export const getUserName = (): string | null => {
  try {
    return localStorage.getItem(USER_NAME_KEY);
  } catch (e) {
    return null;
  }
};

export const getUserId = (): string | null => {
  try {
    return localStorage.getItem(USER_ID_KEY);
  } catch (e) {
    // Fallback: try to extract from token
    const token = getToken();
    return token ? getUserIdFromToken(token) : null;
  }
};

export const clearToken = () => {
  try {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(USER_NAME_KEY);
    localStorage.removeItem(USER_ID_KEY);
  } catch (e) {
    // ignore
  }
};

export const hasToken = (): boolean => Boolean(getToken());

/**
 * Check if token is expired (or will expire soon - within 5 minutes)
 */
export const isTokenExpired = (token: string | null): boolean => {
  if (!token) return true;
  try {
    const decoded = decodeToken(token);
    if (!decoded || !decoded.exp) return true;
    // Check if token expires within 5 minutes (300 seconds)
    const expirationTime = decoded.exp * 1000; // Convert to milliseconds
    const currentTime = Date.now();
    const bufferTime = 5 * 60 * 1000; // 5 minutes buffer
    return currentTime >= expirationTime - bufferTime;
  } catch (e) {
    return true;
  }
};

const authUtils = {
  setToken,
  setTokenData,
  getToken,
  getRefreshToken,
  getUserName,
  getUserId,
  clearToken,
  hasToken,
  decodeToken,
  getUserIdFromToken,
  isTokenExpired,
};

export default authUtils;
