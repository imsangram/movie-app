// Environment-specific configuration
export const BASE_URL = process.env.REACT_APP_API_BASE_URL || 'http://localhost:8001/api/';

// Other configuration constants
export const TIMEOUT_MS = parseInt(process.env.REACT_APP_API_TIMEOUT || '10000');

export const API_CONFIG = {
  baseUrl: BASE_URL,
  timeout: TIMEOUT_MS,
  headers: {
    'Content-Type': 'application/json'
  }
} as const;

// Environment detection
export const IS_DEVELOPMENT = process.env.NODE_ENV === 'development';
export const IS_PRODUCTION = process.env.NODE_ENV === 'production';
