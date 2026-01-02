import {
  decodeToken,
  getUserIdFromToken,
  setToken,
  setTokenData,
  getToken,
  getRefreshToken,
  getUserName,
  getUserId,
  clearToken,
  hasToken,
  isTokenExpired,
} from "../../utils/auth";
import { JWTPayload } from "../../types/types";

// Mock localStorage
const localStorageMock = (() => {
  let store: Record<string, string> = {};

  return {
    getItem: (key: string): string | null => store[key] || null,
    setItem: (key: string, value: string): void => {
      store[key] = value.toString();
    },
    removeItem: (key: string): void => {
      delete store[key];
    },
    clear: (): void => {
      store = {};
    },
  };
})();

// Helper function to create a mock JWT token
const createMockJWT = (payload: JWTPayload): string => {
  const header = { alg: "HS256", typ: "JWT" };
  const encodedHeader = btoa(JSON.stringify(header));
  const encodedPayload = btoa(JSON.stringify(payload));
  return `${encodedHeader}.${encodedPayload}.signature`;
};

describe("Auth Utilities", () => {
  beforeEach(() => {
    // Clear localStorage before each test
    localStorageMock.clear();
    // Replace global localStorage with mock
    Object.defineProperty(window, "localStorage", {
      value: localStorageMock,
      writable: true,
    });
  });

  describe("decodeToken", () => {
    it("should decode a valid JWT token", () => {
      const payload: JWTPayload = {
        sub: "user123",
        exp: Math.floor(Date.now() / 1000) + 3600,
        iat: Math.floor(Date.now() / 1000),
      };
      const token = createMockJWT(payload);

      const decoded = decodeToken(token);

      expect(decoded).not.toBeNull();
      expect(decoded?.sub).toBe("user123");
      expect(decoded?.exp).toBe(payload.exp);
    });

    it("should return null for invalid token format", () => {
      const invalidToken = "not.a.valid.token.format";

      const decoded = decodeToken(invalidToken);

      expect(decoded).toBeNull();
    });

    it("should return null for token with invalid base64", () => {
      const invalidToken = "invalid.base64.!!!";

      const decoded = decodeToken(invalidToken);

      expect(decoded).toBeNull();
    });

    it("should return null for empty token", () => {
      const decoded = decodeToken("");

      expect(decoded).toBeNull();
    });

    it("should decode token with NameIdentifier claim", () => {
      const payload: JWTPayload = {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier":
          "user456",
        exp: Math.floor(Date.now() / 1000) + 3600,
      };
      const token = createMockJWT(payload);

      const decoded = decodeToken(token);

      expect(decoded).not.toBeNull();
      expect(
        decoded?.[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        ]
      ).toBe("user456");
    });
  });

  describe("getUserIdFromToken", () => {
    it("should extract userId from NameIdentifier claim", () => {
      const payload: JWTPayload = {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier":
          "user123",
        exp: Math.floor(Date.now() / 1000) + 3600,
      };
      const token = createMockJWT(payload);

      const userId = getUserIdFromToken(token);

      expect(userId).toBe("user123");
    });

    it("should extract userId from nameid claim", () => {
      const payload: JWTPayload = {
        nameid: "user456",
        exp: Math.floor(Date.now() / 1000) + 3600,
      };
      const token = createMockJWT(payload);

      const userId = getUserIdFromToken(token);

      expect(userId).toBe("user456");
    });

    it("should extract userId from sub claim", () => {
      const payload: JWTPayload = {
        sub: "user789",
        exp: Math.floor(Date.now() / 1000) + 3600,
      };
      const token = createMockJWT(payload);

      const userId = getUserIdFromToken(token);

      expect(userId).toBe("user789");
    });

    it("should return null for null token", () => {
      const userId = getUserIdFromToken(null);

      expect(userId).toBeNull();
    });

    it("should return null for invalid token", () => {
      const userId = getUserIdFromToken("invalid.token");

      expect(userId).toBeNull();
    });

    it("should return null when no userId claim exists", () => {
      const payload: JWTPayload = {
        exp: Math.floor(Date.now() / 1000) + 3600,
      };
      const token = createMockJWT(payload);

      const userId = getUserIdFromToken(token);

      expect(userId).toBeNull();
    });

    it("should prioritize NameIdentifier over nameid", () => {
      const payload: JWTPayload = {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier":
          "user123",
        nameid: "user456",
        sub: "user789",
      };
      const token = createMockJWT(payload);

      const userId = getUserIdFromToken(token);

      expect(userId).toBe("user123");
    });
  });

  describe("setToken", () => {
    it("should set token in localStorage", () => {
      const payload: JWTPayload = {
        sub: "user123",
        exp: Math.floor(Date.now() / 1000) + 3600,
      };
      const token = createMockJWT(payload);

      setToken(token);

      expect(localStorage.getItem("authToken")).toBe(token);
    });

    it("should extract and store userId from token", () => {
      const payload: JWTPayload = {
        sub: "user123",
        exp: Math.floor(Date.now() / 1000) + 3600,
      };
      const token = createMockJWT(payload);

      setToken(token);

      expect(localStorage.getItem("authToken")).toBe(token);
      expect(localStorage.getItem("userId")).toBe("user123");
    });

    it("should handle token without userId gracefully", () => {
      const payload: JWTPayload = {
        exp: Math.floor(Date.now() / 1000) + 3600,
      };
      const token = createMockJWT(payload);

      setToken(token);

      expect(localStorage.getItem("authToken")).toBe(token);
      expect(localStorage.getItem("userId")).toBeNull();
    });
  });

  describe("setTokenData", () => {
    it("should set all token data in localStorage", () => {
      const tokenData = {
        accessToken: "access_token_123",
        refreshToken: "refresh_token_456",
        userName: "john_doe",
        userId: "user123",
      };

      setTokenData(tokenData);

      expect(localStorage.getItem("authToken")).toBe("access_token_123");
      expect(localStorage.getItem("refreshToken")).toBe("refresh_token_456");
      expect(localStorage.getItem("userName")).toBe("john_doe");
      expect(localStorage.getItem("userId")).toBe("user123");
    });

    it("should extract userId from token if not provided", () => {
      const payload: JWTPayload = {
        sub: "user123",
        exp: Math.floor(Date.now() / 1000) + 3600,
      };
      const token = createMockJWT(payload);

      const tokenData = {
        accessToken: token,
        refreshToken: "refresh_token_456",
        userName: "john_doe",
      };

      setTokenData(tokenData);

      expect(localStorage.getItem("userId")).toBe("user123");
    });

    it("should use provided userId over token extraction", () => {
      const payload: JWTPayload = {
        sub: "different_user",
        exp: Math.floor(Date.now() / 1000) + 3600,
      };
      const token = createMockJWT(payload);

      const tokenData = {
        accessToken: token,
        refreshToken: "refresh_token_456",
        userName: "john_doe",
        userId: "user123",
      };

      setTokenData(tokenData);

      expect(localStorage.getItem("userId")).toBe("user123");
    });
  });

  describe("getToken", () => {
    it("should return token from localStorage", () => {
      localStorage.setItem("authToken", "test_token_123");

      const token = getToken();

      expect(token).toBe("test_token_123");
    });

    it("should return null when token does not exist", () => {
      const token = getToken();

      expect(token).toBeNull();
    });
  });

  describe("getRefreshToken", () => {
    it("should return refresh token from localStorage", () => {
      localStorage.setItem("refreshToken", "refresh_token_123");

      const refreshToken = getRefreshToken();

      expect(refreshToken).toBe("refresh_token_123");
    });

    it("should return null when refresh token does not exist", () => {
      const refreshToken = getRefreshToken();

      expect(refreshToken).toBeNull();
    });
  });

  describe("getUserName", () => {
    it("should return username from localStorage", () => {
      localStorage.setItem("userName", "john_doe");

      const userName = getUserName();

      expect(userName).toBe("john_doe");
    });

    it("should return null when username does not exist", () => {
      const userName = getUserName();

      expect(userName).toBeNull();
    });
  });

  describe("clearToken", () => {
    it("should remove all token-related data from localStorage", () => {
      localStorage.setItem("authToken", "token");
      localStorage.setItem("refreshToken", "refresh");
      localStorage.setItem("userName", "user");
      localStorage.setItem("userId", "id");

      clearToken();

      expect(localStorage.getItem("authToken")).toBeNull();
      expect(localStorage.getItem("refreshToken")).toBeNull();
      expect(localStorage.getItem("userName")).toBeNull();
      expect(localStorage.getItem("userId")).toBeNull();
    });

    it("should handle clearing when no tokens exist", () => {
      clearToken();

      expect(localStorage.getItem("authToken")).toBeNull();
    });
  });

  describe("hasToken", () => {
    it("should return true when token exists", () => {
      localStorage.setItem("authToken", "test_token");

      const result = hasToken();

      expect(result).toBe(true);
    });

    it("should return false when token does not exist", () => {
      const result = hasToken();

      expect(result).toBe(false);
    });

    it("should return false when token is empty string", () => {
      localStorage.setItem("authToken", "");

      const result = hasToken();

      expect(result).toBe(false);
    });
  });

  describe("isTokenExpired", () => {
    it("should return true for null token", () => {
      const result = isTokenExpired(null);

      expect(result).toBe(true);
    });

    it("should return true for expired token", () => {
      const payload: JWTPayload = {
        exp: Math.floor(Date.now() / 1000) - 3600, // Expired 1 hour ago
      };
      const token = createMockJWT(payload);

      const result = isTokenExpired(token);

      expect(result).toBe(true);
    });

    it("should return false for valid token", () => {
      const payload: JWTPayload = {
        exp: Math.floor(Date.now() / 1000) + 3600, // Valid for 1 hour
      };
      const token = createMockJWT(payload);

      const result = isTokenExpired(token);

      expect(result).toBe(false);
    });

    it("should return true for token expiring within 5 minutes", () => {
      const payload: JWTPayload = {
        exp: Math.floor(Date.now() / 1000) + 240, // Expires in 4 minutes
      };
      const token = createMockJWT(payload);

      const result = isTokenExpired(token);

      expect(result).toBe(true);
    });

    it("should return false for token expiring after 5 minutes", () => {
      const payload: JWTPayload = {
        exp: Math.floor(Date.now() / 1000) + 360, // Expires in 6 minutes
      };
      const token = createMockJWT(payload);

      const result = isTokenExpired(token);

      expect(result).toBe(false);
    });

    it("should return true for token without exp claim", () => {
      const payload: JWTPayload = {};
      const token = createMockJWT(payload);

      const result = isTokenExpired(token);

      expect(result).toBe(true);
    });

    it("should return true for invalid token", () => {
      const result = isTokenExpired("invalid.token");

      expect(result).toBe(true);
    });
  });
});
