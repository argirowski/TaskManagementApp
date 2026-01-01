import axios, { AxiosError } from "axios";
import {
  getRefreshToken,
  getUserId,
  setTokenData,
  clearToken,
} from "../utils/auth";
import { RefreshTokenRequest, TokenData } from "../types/types";
import { getApiUrl, API_CONFIG } from "../config/api";

/**
 * Refresh the access token using the refresh token
 * @returns New token data or null if refresh fails
 */
export const refreshAccessToken = async (): Promise<TokenData | null> => {
  try {
    const refreshToken = getRefreshToken();
    const userId = getUserId();

    if (!refreshToken || !userId) {
      return null;
    }

    const request: RefreshTokenRequest = {
      userId: userId,
      refreshToken: refreshToken,
    };

    const response = await axios.post<{
      AccessToken?: string;
      RefreshToken?: string;
      UserName?: string;
      accessToken?: string;
      refreshToken?: string;
      userName?: string;
    }>(getApiUrl(API_CONFIG.ENDPOINTS.AUTH.REFRESH_TOKEN), request);

    // Extract token data (handling both camelCase and PascalCase)
    const accessToken =
      response?.data?.accessToken || response?.data?.AccessToken;
    const newRefreshToken =
      response?.data?.refreshToken || response?.data?.RefreshToken;
    const userName = response?.data?.userName || response?.data?.UserName;

    if (accessToken && newRefreshToken && userName) {
      const tokenData: TokenData = {
        accessToken,
        refreshToken: newRefreshToken,
        userName,
        userId: userId, // Preserve userId
      };

      setTokenData(tokenData);
      return tokenData;
    }

    return null;
  } catch (error) {
    // If refresh fails, clear tokens and redirect to login
    // Error is intentionally ignored - token refresh failure is handled by clearing tokens
    if (error instanceof AxiosError) {
      // Could log error here if needed for debugging
      // console.error('Token refresh failed:', error.response?.status, error.message);
    }
    clearToken();
    return null;
  }
};
