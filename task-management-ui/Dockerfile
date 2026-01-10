# Build stage
FROM node:20-alpine AS build
WORKDIR /app

# Copy package files
COPY package.json yarn.lock ./

# Install dependencies
RUN yarn install --frozen-lockfile

# Copy source code
COPY . .

# Accept build arguments for React environment variables
ARG REACT_APP_API_BASE_URL=http://localhost:8080/api
ENV REACT_APP_API_BASE_URL=$REACT_APP_API_BASE_URL

# Build the app
RUN yarn build

# Production stage
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy nginx configuration first (changes less frequently)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Remove default nginx static assets
RUN rm -rf ./*

# Copy built app from build stage
COPY --from=build /app/build .

# Expose port 80
EXPOSE 80

# Health check (nginx.conf includes /health endpoint)
# Install wget for healthcheck, then remove it to keep image small
RUN apk add --no-cache wget
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost/health || exit 1

CMD ["nginx", "-g", "daemon off;"]

