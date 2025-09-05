#!/bin/bash

# Local testing script for DotNet Tutor Application
# This script helps test the Docker setup locally before deploying to EC2

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Function to check if command exists
command_exists() {
    command -v "$1" >/dev/null 2>&1
}

# Check prerequisites
check_prerequisites() {
    print_status "Checking prerequisites..."
    
    if ! command_exists docker; then
        print_error "Docker is not installed. Please install Docker first."
        exit 1
    fi
    
    if ! command_exists docker-compose; then
        print_error "Docker Compose is not installed. Please install Docker Compose first."
        exit 1
    fi
    
    if ! docker info >/dev/null 2>&1; then
        print_error "Docker daemon is not running. Please start Docker first."
        exit 1
    fi
    
    print_status "Prerequisites check passed!"
}

# Function to create environment file
create_env_file() {
    if [ ! -f ".env" ]; then
        print_status "Creating .env file..."
        cat > .env << EOF
JWT_SECRET_KEY=local-development-secret-key-that-is-at-least-32-characters-long
ASPNETCORE_ENVIRONMENT=Development
EOF
        print_status ".env file created"
    else
        print_status ".env file already exists"
    fi
}

# Function to create data directory
create_data_dir() {
    if [ ! -d "data" ]; then
        print_status "Creating data directory..."
        mkdir -p data
        print_status "Data directory created"
    else
        print_status "Data directory already exists"
    fi
}

# Function to build and start containers
start_application() {
    print_status "Building and starting containers..."
    
    # Stop any existing containers
    docker-compose down 2>/dev/null || true
    
    # Build and start
    docker-compose build
    docker-compose up -d
    
    print_status "Containers started successfully!"
}

# Function to wait for services
wait_for_services() {
    print_status "Waiting for services to be ready..."
    
    # Wait for backend
    print_status "Waiting for backend API..."
    for i in {1..30}; do
        if curl -f http://localhost:5000/health >/dev/null 2>&1; then
            print_status "Backend API is ready!"
            break
        fi
        if [ $i -eq 30 ]; then
            print_error "Backend API failed to start within 30 seconds"
            return 1
        fi
        sleep 1
    done
    
    # Wait for frontend
    print_status "Waiting for frontend..."
    for i in {1..30}; do
        if curl -f http://localhost:3000/health >/dev/null 2>&1; then
            print_status "Frontend is ready!"
            break
        fi
        if [ $i -eq 30 ]; then
            print_error "Frontend failed to start within 30 seconds"
            return 1
        fi
        sleep 1
    done
    
    # Wait for proxy
    print_status "Waiting for proxy..."
    for i in {1..30}; do
        if curl -f http://localhost/health >/dev/null 2>&1; then
            print_status "Proxy is ready!"
            break
        fi
        if [ $i -eq 30 ]; then
            print_error "Proxy failed to start within 30 seconds"
            return 1
        fi
        sleep 1
    done
}

# Function to run health checks
run_health_checks() {
    print_status "Running health checks..."
    
    # Check container status
    print_status "Container status:"
    docker-compose ps
    
    # Check individual services
    if curl -f http://localhost:5000/health >/dev/null 2>&1; then
        print_status "✓ Backend API health check passed"
    else
        print_error "✗ Backend API health check failed"
        return 1
    fi
    
    if curl -f http://localhost:3000/health >/dev/null 2>&1; then
        print_status "✓ Frontend health check passed"
    else
        print_error "✗ Frontend health check failed"
        return 1
    fi
    
    if curl -f http://localhost/health >/dev/null 2>&1; then
        print_status "✓ Proxy health check passed"
    else
        print_error "✗ Proxy health check failed"
        return 1
    fi
    
    print_status "All health checks passed!"
}

# Function to show application URLs
show_urls() {
    print_status "Application is ready! Access URLs:"
    echo "  Main Application: http://localhost"
    echo "  Frontend Only:    http://localhost:3000"
    echo "  Backend API:      http://localhost:5000"
    echo "  API Swagger:      http://localhost:5000/swagger"
    echo ""
    print_status "To view logs: docker-compose logs -f"
    print_status "To stop: docker-compose down"
}

# Function to cleanup
cleanup() {
    print_status "Cleaning up..."
    docker-compose down
    docker system prune -f
    print_status "Cleanup completed"
}

# Main function
main() {
    case "${1:-start}" in
        "start")
            check_prerequisites
            create_env_file
            create_data_dir
            start_application
            wait_for_services
            run_health_checks
            show_urls
            ;;
        "stop")
            print_status "Stopping application..."
            docker-compose down
            print_status "Application stopped"
            ;;
        "restart")
            print_status "Restarting application..."
            docker-compose down
            docker-compose up -d
            wait_for_services
            run_health_checks
            show_urls
            ;;
        "logs")
            docker-compose logs -f
            ;;
        "status")
            docker-compose ps
            ;;
        "health")
            run_health_checks
            ;;
        "cleanup")
            cleanup
            ;;
        *)
            echo "Usage: $0 {start|stop|restart|logs|status|health|cleanup}"
            echo "  start   - Start the application (default)"
            echo "  stop    - Stop the application"
            echo "  restart - Restart the application"
            echo "  logs    - Show application logs"
            echo "  status  - Show container status"
            echo "  health  - Run health checks"
            echo "  cleanup - Stop and cleanup containers"
            exit 1
            ;;
    esac
}

# Run main function
main "$@"