// Mock axios - must be before imports
jest.mock("axios");

// Mock auth utilities - path relative to test file location
jest.mock("../../utils/auth", () => ({
  getRefreshToken: jest.fn(),
  getUserId: jest.fn(),
  setTokenData: jest.fn(),
  clearToken: jest.fn(),
}));

// Mock API config - path relative to test file location
jest.mock("../../config/api", () => ({
  API_CONFIG: {
    BASE_URL: "https://localhost:7272/api",
    ENDPOINTS: {
      AUTH: {
        REFRESH_TOKEN: "/Auth/refresh-token",
      },
    },
  },
  getApiUrl: jest.fn(
    (endpoint: string) => `https://localhost:7272/api${endpoint}`
  ),
}));

// eslint-disable-next-line import/first
import { refreshAccessToken } from "../../services/authService";
// eslint-disable-next-line import/first
import axios, { AxiosError } from "axios";
// eslint-disable-next-line import/first
import {
  getRefreshToken,
  getUserId,
  setTokenData,
  clearToken,
} from "../../utils/auth";
// eslint-disable-next-line import/first
import { getApiUrl } from "../../config/api";

const mockedAxios = axios as jest.Mocked<typeof axios>;

const mockedGetRefreshToken = getRefreshToken as jest.MockedFunction<
  typeof getRefreshToken
>;
const mockedGetUserId = getUserId as jest.MockedFunction<typeof getUserId>;
const mockedSetTokenData = setTokenData as jest.MockedFunction<
  typeof setTokenData
>;
const mockedClearToken = clearToken as jest.MockedFunction<typeof clearToken>;
const mockedGetApiUrl = getApiUrl as jest.MockedFunction<typeof getApiUrl>;

describe("Auth Service", () => {
  beforeEach(() => {
    jest.clearAllMocks();
    mockedGetApiUrl.mockImplementation(
      (endpoint: string) => `https://localhost:7272/api${endpoint}`
    );
  });

  describe("refreshAccessToken", () => {
    const mockRefreshToken = "refresh-token-123";
    const mockUserId = "user-123";
    const mockAccessToken = "access-token-456";
    const mockNewRefreshToken = "refresh-token-789";
    const mockUserName = "testuser";

    it("should successfully refresh token with camelCase response", async () => {
      mockedGetRefreshToken.mockReturnValue(mockRefreshToken);
      mockedGetUserId.mockReturnValue(mockUserId);
      mockedAxios.post.mockResolvedValueOnce({
        data: {
          accessToken: mockAccessToken,
          refreshToken: mockNewRefreshToken,
          userName: mockUserName,
        },
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await refreshAccessToken();

      expect(mockedGetRefreshToken).toHaveBeenCalled();
      expect(mockedGetUserId).toHaveBeenCalled();
      expect(mockedAxios.post).toHaveBeenCalledWith(
        "https://localhost:7272/api/Auth/refresh-token",
        {
          userId: mockUserId,
          refreshToken: mockRefreshToken,
        }
      );
      expect(mockedSetTokenData).toHaveBeenCalledWith({
        accessToken: mockAccessToken,
        refreshToken: mockNewRefreshToken,
        userName: mockUserName,
        userId: mockUserId,
      });
      expect(result).toEqual({
        accessToken: mockAccessToken,
        refreshToken: mockNewRefreshToken,
        userName: mockUserName,
        userId: mockUserId,
      });
      expect(mockedClearToken).not.toHaveBeenCalled();
    });

    it("should successfully refresh token with PascalCase response", async () => {
      mockedGetRefreshToken.mockReturnValue(mockRefreshToken);
      mockedGetUserId.mockReturnValue(mockUserId);
      mockedAxios.post.mockResolvedValueOnce({
        data: {
          AccessToken: mockAccessToken,
          RefreshToken: mockNewRefreshToken,
          UserName: mockUserName,
        },
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await refreshAccessToken();

      expect(mockedAxios.post).toHaveBeenCalledWith(
        "https://localhost:7272/api/Auth/refresh-token",
        {
          userId: mockUserId,
          refreshToken: mockRefreshToken,
        }
      );
      expect(mockedSetTokenData).toHaveBeenCalledWith({
        accessToken: mockAccessToken,
        refreshToken: mockNewRefreshToken,
        userName: mockUserName,
        userId: mockUserId,
      });
      expect(result).toEqual({
        accessToken: mockAccessToken,
        refreshToken: mockNewRefreshToken,
        userName: mockUserName,
        userId: mockUserId,
      });
      expect(mockedClearToken).not.toHaveBeenCalled();
    });

    it("should return null when refresh token is missing", async () => {
      mockedGetRefreshToken.mockReturnValue(null);
      mockedGetUserId.mockReturnValue(mockUserId);

      const result = await refreshAccessToken();

      expect(mockedGetRefreshToken).toHaveBeenCalled();
      expect(mockedGetUserId).toHaveBeenCalled();
      expect(mockedAxios.post).not.toHaveBeenCalled();
      expect(result).toBeNull();
      expect(mockedSetTokenData).not.toHaveBeenCalled();
      expect(mockedClearToken).not.toHaveBeenCalled();
    });

    it("should return null when userId is missing", async () => {
      mockedGetRefreshToken.mockReturnValue(mockRefreshToken);
      mockedGetUserId.mockReturnValue(null);

      const result = await refreshAccessToken();

      expect(mockedGetRefreshToken).toHaveBeenCalled();
      expect(mockedGetUserId).toHaveBeenCalled();
      expect(mockedAxios.post).not.toHaveBeenCalled();
      expect(result).toBeNull();
      expect(mockedSetTokenData).not.toHaveBeenCalled();
      expect(mockedClearToken).not.toHaveBeenCalled();
    });

    it("should return null when both refresh token and userId are missing", async () => {
      mockedGetRefreshToken.mockReturnValue(null);
      mockedGetUserId.mockReturnValue(null);

      const result = await refreshAccessToken();

      expect(mockedGetRefreshToken).toHaveBeenCalled();
      expect(mockedGetUserId).toHaveBeenCalled();
      expect(mockedAxios.post).not.toHaveBeenCalled();
      expect(result).toBeNull();
      expect(mockedSetTokenData).not.toHaveBeenCalled();
      expect(mockedClearToken).not.toHaveBeenCalled();
    });

    it("should return null when response is missing accessToken", async () => {
      mockedGetRefreshToken.mockReturnValue(mockRefreshToken);
      mockedGetUserId.mockReturnValue(mockUserId);
      mockedAxios.post.mockResolvedValueOnce({
        data: {
          refreshToken: mockNewRefreshToken,
          userName: mockUserName,
        },
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await refreshAccessToken();

      expect(mockedAxios.post).toHaveBeenCalled();
      expect(mockedSetTokenData).not.toHaveBeenCalled();
      expect(result).toBeNull();
      expect(mockedClearToken).not.toHaveBeenCalled();
    });

    it("should return null when response is missing refreshToken", async () => {
      mockedGetRefreshToken.mockReturnValue(mockRefreshToken);
      mockedGetUserId.mockReturnValue(mockUserId);
      mockedAxios.post.mockResolvedValueOnce({
        data: {
          accessToken: mockAccessToken,
          userName: mockUserName,
        },
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await refreshAccessToken();

      expect(mockedAxios.post).toHaveBeenCalled();
      expect(mockedSetTokenData).not.toHaveBeenCalled();
      expect(result).toBeNull();
      expect(mockedClearToken).not.toHaveBeenCalled();
    });

    it("should return null when response is missing userName", async () => {
      mockedGetRefreshToken.mockReturnValue(mockRefreshToken);
      mockedGetUserId.mockReturnValue(mockUserId);
      mockedAxios.post.mockResolvedValueOnce({
        data: {
          accessToken: mockAccessToken,
          refreshToken: mockNewRefreshToken,
        },
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await refreshAccessToken();

      expect(mockedAxios.post).toHaveBeenCalled();
      expect(mockedSetTokenData).not.toHaveBeenCalled();
      expect(result).toBeNull();
      expect(mockedClearToken).not.toHaveBeenCalled();
    });

    it("should clear tokens and return null when API call fails with AxiosError", async () => {
      mockedGetRefreshToken.mockReturnValue(mockRefreshToken);
      mockedGetUserId.mockReturnValue(mockUserId);
      const error = new AxiosError("Unauthorized");
      error.response = {
        status: 401,
        statusText: "Unauthorized",
        data: { error: "Invalid refresh token" },
        headers: {},
        config: {} as any,
      };
      mockedAxios.post.mockRejectedValueOnce(error);

      const result = await refreshAccessToken();

      expect(mockedAxios.post).toHaveBeenCalled();
      expect(mockedSetTokenData).not.toHaveBeenCalled();
      expect(mockedClearToken).toHaveBeenCalled();
      expect(result).toBeNull();
    });

    it("should clear tokens and return null when API call fails with network error", async () => {
      mockedGetRefreshToken.mockReturnValue(mockRefreshToken);
      mockedGetUserId.mockReturnValue(mockUserId);
      const error = new Error("Network Error");
      mockedAxios.post.mockRejectedValueOnce(error);

      const result = await refreshAccessToken();

      expect(mockedAxios.post).toHaveBeenCalled();
      expect(mockedSetTokenData).not.toHaveBeenCalled();
      expect(mockedClearToken).toHaveBeenCalled();
      expect(result).toBeNull();
    });

    it("should clear tokens and return null when API call fails with 400 Bad Request", async () => {
      mockedGetRefreshToken.mockReturnValue(mockRefreshToken);
      mockedGetUserId.mockReturnValue(mockUserId);
      const error = new AxiosError("Bad Request");
      error.response = {
        status: 400,
        statusText: "Bad Request",
        data: { error: "Invalid request" },
        headers: {},
        config: {} as any,
      };
      mockedAxios.post.mockRejectedValueOnce(error);

      const result = await refreshAccessToken();

      expect(mockedAxios.post).toHaveBeenCalled();
      expect(mockedSetTokenData).not.toHaveBeenCalled();
      expect(mockedClearToken).toHaveBeenCalled();
      expect(result).toBeNull();
    });

    it("should preserve userId when refreshing token", async () => {
      const customUserId = "custom-user-456";
      mockedGetRefreshToken.mockReturnValue(mockRefreshToken);
      mockedGetUserId.mockReturnValue(customUserId);
      mockedAxios.post.mockResolvedValueOnce({
        data: {
          accessToken: mockAccessToken,
          refreshToken: mockNewRefreshToken,
          userName: mockUserName,
        },
        status: 200,
        statusText: "OK",
        headers: {},
        config: {} as any,
      });

      const result = await refreshAccessToken();

      expect(mockedSetTokenData).toHaveBeenCalledWith({
        accessToken: mockAccessToken,
        refreshToken: mockNewRefreshToken,
        userName: mockUserName,
        userId: customUserId,
      });
      expect(result?.userId).toBe(customUserId);
    });
  });
});
