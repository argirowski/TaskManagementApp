# Environment Configuration

This project uses environment variables to configure the API base URL.

## Setup

1. **Create a `.env` file** in the root of `task-management-ui` directory:

   ```env
   REACT_APP_API_BASE_URL=https://localhost:7272/api
   ```

2. **For different environments**, you can create:
   - `.env.development` - Development environment
   - `.env.production` - Production environment
   - `.env.test` - Test environment

## Environment Variables

### `REACT_APP_API_BASE_URL`

- **Description**: Base URL for the API endpoints
- **Default**: `https://localhost:7272/api` (fallback if not set)
- **Example Development**: `https://localhost:7272/api`
- **Example Production**: `https://api.yourdomain.com/api`

## Important Notes

- React requires the `REACT_APP_` prefix for environment variables to be accessible in the browser
- Environment variables are embedded at build time, not runtime
- After changing `.env` file, restart the development server (`npm start` or `yarn start`)
- The `.env` file is gitignored - never commit sensitive credentials
- Use `.env.example` as a template for team members

## Usage in Code

The API configuration is centralized in `src/config/api.ts`:

```typescript
import { API_CONFIG, getApiUrl } from "../config/api";

// Use the base URL
const baseUrl = API_CONFIG.BASE_URL;

// Use endpoint constants
const loginUrl = API_CONFIG.ENDPOINTS.AUTH.LOGIN;

// Build full URL
const fullUrl = getApiUrl(API_CONFIG.ENDPOINTS.AUTH.LOGIN);
```
