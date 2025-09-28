// Auth Action Types
export const LOGIN_REQUEST = "LOGIN_REQUEST";
export const LOGIN_SUCCESS = "LOGIN_SUCCESS";
export const LOGIN_FAILURE = "LOGIN_FAILURE";

export const LOGOUT = "LOGOUT";

export const REFRESH_TOKEN_REQUEST = "REFRESH_TOKEN_REQUEST";
export const REFRESH_TOKEN_SUCCESS = "REFRESH_TOKEN_SUCCESS";
export const REFRESH_TOKEN_FAILURE = "REFRESH_TOKEN_FAILURE";

export const SET_AUTH_LOADING = "SET_AUTH_LOADING";
export const CLEAR_AUTH_ERROR = "CLEAR_AUTH_ERROR";

// Auth State Interface
export interface User {
  id: string;
  userName: string;
  userEmail: string;
}

export interface AuthState {
  user: User | null;
  token: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
  tokenExpiry: number | null;
}

// Auth Action Interfaces
export interface LoginRequestAction {
  type: typeof LOGIN_REQUEST;
}

export interface LoginSuccessAction {
  type: typeof LOGIN_SUCCESS;
  payload: {
    user: User;
    token: string;
    refreshToken?: string;
    tokenExpiry?: number;
  };
}

export interface LoginFailureAction {
  type: typeof LOGIN_FAILURE;
  payload: string;
}

export interface LogoutAction {
  type: typeof LOGOUT;
}

export interface RefreshTokenRequestAction {
  type: typeof REFRESH_TOKEN_REQUEST;
}

export interface RefreshTokenSuccessAction {
  type: typeof REFRESH_TOKEN_SUCCESS;
  payload: {
    token: string;
    refreshToken?: string;
    tokenExpiry?: number;
  };
}

export interface RefreshTokenFailureAction {
  type: typeof REFRESH_TOKEN_FAILURE;
  payload: string;
}

export interface SetAuthLoadingAction {
  type: typeof SET_AUTH_LOADING;
  payload: boolean;
}

export interface ClearAuthErrorAction {
  type: typeof CLEAR_AUTH_ERROR;
}

export type AuthActionTypes =
  | LoginRequestAction
  | LoginSuccessAction
  | LoginFailureAction
  | LogoutAction
  | RefreshTokenRequestAction
  | RefreshTokenSuccessAction
  | RefreshTokenFailureAction
  | SetAuthLoadingAction
  | ClearAuthErrorAction;
