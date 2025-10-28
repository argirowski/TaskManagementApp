const TOKEN_KEY = "authToken";

export const setToken = (token: string) => {
  try {
    localStorage.setItem(TOKEN_KEY, token);
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

export const clearToken = () => {
  try {
    localStorage.removeItem(TOKEN_KEY);
  } catch (e) {
    // ignore
  }
};

export const hasToken = (): boolean => Boolean(getToken());

const authUtils = {
  setToken,
  getToken,
  clearToken,
  hasToken,
};

export default authUtils;
