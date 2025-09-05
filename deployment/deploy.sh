#!/bin/bash

# Deployment script for DotNet Tutor Application on EC2
# This script should be run on the EC2 instance to deploy updates

set -e  # Exit on any error

# Configuration
APP_DIR="/opt/dotnet-tutor"
BACKUP_DIR="/opt/dotnet-tutor-backup"
LOG_FILE="/var/log/dotnet-tutor-deploy.log"

# Function to log messages
log() {
    echo "[$(date '+%Y-%m-%d %H:%M:%S')] $1" | tee -a "$LOG_FILE"
}

# Function to create backup
create_backup() {
    log "Creating backup..."
    if [ -d "$APP_DIR" ]; then
        sudo rm -rf "$BACKUP_DIR"
        sudo cp -r "$APP_DIR" "$BACKUP_DIR"
        log "Backup created at $BACKUP_DIR"
    fi
}

# Function to rollback
rollback() {
    log "Rolling back to previous version..."
    if [ -d "$BACKUP_DIR" ]; then
        sudo rm -rf "$APP_DIR"
        sudo mv "$BACKUP_DIR" "$APP_DIR"
        cd "$APP_DIR"
        sudo docker-compose down
        sudo docker-compose up -d
        log "Rollback completed"
    else
        log "No backup found for rollback"
        exit 1
    fi
}

# Function to check if services are healthy
check_health() {
    log "Checking application health..."
    
    # Wait for services to start
    sleep 30
    
    # Check if containers are running
    if ! sudo docker-compose ps | grep -q "Up"; then
        log "ERROR: Some containers are not running"
        return 1
    fi
    
    # Check API health
    if ! curl -f http://localhost:5000/health > /dev/null 2>&1; then
        log "ERROR: API health check failed"
        return 1
    fi
    
    # Check frontend health
    if ! curl -f http://localhost:3000/health > /dev/null 2>&1; then
        log "ERROR: Frontend health check failed"
        return 1
    fi
    
    # Check main proxy health
    if ! curl -f http://localhost/health > /dev/null 2>&1; then
        log "ERROR: Proxy health check failed"
        return 1
    fi
    
    log "All health checks passed"
    return 0
}

# Main deployment function
deploy() {
    log "Starting deployment..."
    
    # Create backup
    create_backup
    
    # Navigate to app directory
    cd "$APP_DIR"
    
    # Pull latest changes (if using git)
    if [ -d ".git" ]; then
        log "Pulling latest changes from git..."
        git pull origin main
    fi
    
    # Stop existing containers
    log "Stopping existing containers..."
    sudo docker-compose down
    
    # Remove old images to free space
    log "Cleaning up old Docker images..."
    sudo docker image prune -f
    
    # Build and start new containers
    log "Building and starting new containers..."
    sudo docker-compose build --no-cache
    sudo docker-compose up -d
    
    # Check health
    if check_health; then
        log "Deployment completed successfully"
        
        # Clean up backup after successful deployment
        sudo rm -rf "$BACKUP_DIR"
        log "Backup cleaned up"
        
        # Show running containers
        log "Running containers:"
        sudo docker-compose ps
        
    else
        log "Health check failed, rolling back..."
        rollback
        
        # Check health after rollback
        if check_health; then
            log "Rollback successful"
        else
            log "ERROR: Rollback failed, manual intervention required"
            exit 1
        fi
    fi
}

# Handle script arguments
case "${1:-deploy}" in
    "deploy")
        deploy
        ;;
    "rollback")
        rollback
        ;;
    "health")
        check_health
        ;;
    "logs")
        sudo docker-compose logs -f
        ;;
    "status")
        sudo docker-compose ps
        ;;
    *)
        echo "Usage: $0 {deploy|rollback|health|logs|status}"
        echo "  deploy   - Deploy the application (default)"
        echo "  rollback - Rollback to previous version"
        echo "  health   - Check application health"
        echo "  logs     - Show application logs"
        echo "  status   - Show container status"
        exit 1
        ;;
esac