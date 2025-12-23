import axios from "axios";
import {
  TokenData,
  getRefreshToken,
  getUserId,
  setTokenData,
  clearToken,
} from "../utils/auth";
import { RefreshTokenRequest } from "../types/types";

const API_BASE_URL = "https://localhost:7272/api";

/**
 * Refresh the access token using the refresh token
 * @returns New token data or null if refresh fails
 */
export const refreshAccessToken = async (): Promise<TokenData | null> => {
  try {
    const refreshToken = getRefreshToken();
    const userId = getUserId();

    if (!refreshToken || !userId) {
      console.error("Refresh token or userId not found");
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
    }>(`${API_BASE_URL}/Auth/refresh-token`, request);

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
  } catch (error: any) {
    console.error("Token refresh failed:", error);
    // If refresh fails, clear tokens and redirect to login
    clearToken();
    return null;
  }
};
