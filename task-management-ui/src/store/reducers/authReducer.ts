import {
  AuthState,
  AuthActionTypes,
  LOGIN_REQUEST,
  LOGIN_SUCCESS,
  LOGIN_FAILURE,
  LOGOUT,
  REFRESH_TOKEN_REQUEST,
  REFRESH_TOKEN_SUCCESS,
  REFRESH_TOKEN_FAILURE,
  SET_AUTH_LOADING,
  CLEAR_AUTH_ERROR,
} from "../types/authTypes";

const initialState: AuthState = {
  user: null,
  token: null,
  refreshToken: null,
  isAuthenticated: false,
  loading: false,
  error: null,
  tokenExpiry: null,
};

const authReducer = (
  state: AuthState = initialState,
  action: AuthActionTypes
): AuthState => {
  switch (action.type) {
    case LOGIN_REQUEST:
      return {
        ...state,
        loading: true,
        error: null,
      };

    case LOGIN_SUCCESS:
      return {
        ...state,
        loading: false,
        user: action.payload.user,
        token: action.payload.token,
        refreshToken: action.payload.refreshToken || null,
        tokenExpiry: action.payload.tokenExpiry || null,
        isAuthenticated: true,
        error: null,
      };

    case LOGIN_FAILURE:
      return {
        ...state,
        loading: false,
        user: null,
        token: null,
        refreshToken: null,
        tokenExpiry: null,
        isAuthenticated: false,
        error: action.payload,
      };

    case LOGOUT:
      return {
        ...initialState,
        loading: false, // Keep loading false on logout
      };

    case REFRESH_TOKEN_REQUEST:
      return {
        ...state,
        loading: true,
        error: null,
      };

    case REFRESH_TOKEN_SUCCESS:
      return {
        ...state,
        loading: false,
        token: action.payload.token,
        refreshToken: action.payload.refreshToken || state.refreshToken,
        tokenExpiry: action.payload.tokenExpiry || null,
        error: null,
      };

    case REFRESH_TOKEN_FAILURE:
      return {
        ...state,
        loading: false,
        error: action.payload,
      };

    case SET_AUTH_LOADING:
      return {
        ...state,
        loading: action.payload,
      };

    case CLEAR_AUTH_ERROR:
      return {
        ...state,
        error: null,
      };

    default:
      return state;
  }
};

export default authReducer;
