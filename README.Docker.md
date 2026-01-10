# Docker Setup Guide

This guide explains how to run the Task Management Application using Docker and Docker Compose.

## Prerequisites

- Docker Desktop (or Docker Engine + Docker Compose)
- At least 4GB of available RAM
- Ports 3000, 8080, and 1433 available

## Quick Start

1. **Build and start all services:**

   ```bash
   docker-compose up --build
   ```

2. **Access the application:**
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:8080
   - Swagger UI: http://localhost:8080/swagger
   - SQL Server: localhost:1433

## Services

The docker-compose setup includes three services:

### 1. SQL Server (`sqlserver`)

- Microsoft SQL Server 2022
- Port: 1433
- Database: TaskManagementApp
- Credentials:
  - Username: `sa`
  - Password: `YourSTRONG!Passw0rd`

### 2. Backend API (`api`)

- .NET 9.0 ASP.NET Core Web API
- Port: 8080
- Automatically runs database migrations on startup
- Health check endpoint available

### 3. Frontend (`frontend`)

- React application served via Nginx
- Port: 3000
- Production build optimized for performance

## Environment Variables

### Backend API

- `ASPNETCORE_ENVIRONMENT`: Set to `Production` in Docker
- `ConnectionStrings__AppConnectionString`: Database connection string
- `Cors__AllowedOrigins`: CORS allowed origins (configured for Docker networking)
- `JWT__*`: JWT configuration settings

### Frontend

- `REACT_APP_API_BASE_URL`: Backend API URL (default: `http://localhost:8080/api`)

## Common Commands

### Start services in detached mode:

```bash
docker-compose up -d
```

### View logs:

```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f api
docker-compose logs -f frontend
docker-compose logs -f sqlserver
```

### Stop services:

```bash
docker-compose down
```

### Stop and remove volumes (clears database):

```bash
docker-compose down -v
```

### Rebuild specific service:

```bash
docker-compose build api
docker-compose build frontend
```

### Execute commands in containers:

```bash
# Access API container shell
docker exec -it taskmanagement-api bash

# Access SQL Server
docker exec -it taskmanagement-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourSTRONG!Passw0rd
```

## Troubleshooting

### Port conflicts

If ports are already in use, modify the port mappings in `docker-compose.yml`:

```yaml
ports:
  - "3001:80" # Change frontend port
  - "8081:8080" # Change API port
  - "1434:1433" # Change SQL Server port
```

### Database connection issues

- Ensure SQL Server container is healthy before API starts
- Check connection string in docker-compose.yml
- Verify SQL Server is accepting connections: `docker-compose logs sqlserver`

### CORS errors

- Verify CORS configuration in `API/Program.cs`
- Check that frontend URL matches allowed origins
- Ensure environment variables are set correctly

### Frontend can't reach API

- Verify `REACT_APP_API_BASE_URL` environment variable
- Check network connectivity: `docker network inspect taskmanagement-taskmanagement-network`
- Ensure API is running: `docker-compose ps`

## Development vs Production

### Development

For local development without Docker:

- Backend: Run from Visual Studio or `dotnet run` in API folder
- Frontend: Run `yarn start` in task-management-ui folder
- Database: Use local SQL Server instance

### Production

For production deployment:

- Use Docker Compose or deploy containers individually
- Update environment variables for production values
- Use strong passwords and secrets management
- Configure HTTPS/TLS certificates
- Set up proper backup strategies for database

## Security Notes

⚠️ **Important**: The default passwords and keys in this setup are for development only. For production:

1. Change SQL Server password
2. Generate a strong JWT secret key
3. Use environment variables or secrets management
4. Enable HTTPS
5. Configure firewall rules
6. Use Docker secrets or external secrets management

## Volumes

- `sqlserver_data`: Persistent storage for SQL Server database
- `api_logs`: Log files from the API service

Data persists even after containers are stopped. To reset:

```bash
docker-compose down -v
```
