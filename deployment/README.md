# DotNet Tutor - AWS EC2 Deployment Guide

This guide provides step-by-step instructions for deploying the DotNet Tutor application to AWS EC2 using Docker containers.

## Architecture Overview

The application consists of:
- **Backend**: .NET 8 Web API with SQLite database
- **Frontend**: React application built with Vite
- **Reverse Proxy**: Nginx for load balancing and SSL termination
- **Infrastructure**: AWS EC2 with CloudWatch monitoring

## Prerequisites

1. AWS Account with appropriate permissions
2. AWS CLI configured locally
3. EC2 Key Pair created
4. Domain name (optional, for SSL)

## Deployment Options

### Option 1: Automated CloudFormation Deployment

1. **Deploy Infrastructure**:
   ```bash
   aws cloudformation create-stack \
     --stack-name dotnet-tutor \
     --template-body file://deployment/cloudformation-template.yaml \
     --parameters ParameterKey=KeyPairName,ParameterValue=your-key-pair \
                  ParameterKey=InstanceType,ParameterValue=t3.medium \
     --capabilities CAPABILITY_IAM
   ```

2. **Wait for Stack Creation**:
   ```bash
   aws cloudformation wait stack-create-complete --stack-name dotnet-tutor
   ```

3. **Get Instance IP**:
   ```bash
   aws cloudformation describe-stacks \
     --stack-name dotnet-tutor \
     --query 'Stacks[0].Outputs[?OutputKey==`PublicIP`].OutputValue' \
     --output text
   ```

### Option 2: Manual EC2 Setup

1. **Launch EC2 Instance**:
   - AMI: Amazon Linux 2
   - Instance Type: t3.medium or larger
   - Security Group: Allow ports 22, 80, 443
   - User Data: Use `deployment/ec2-user-data.sh`

2. **Connect to Instance**:
   ```bash
   ssh -i your-key.pem ec2-user@your-instance-ip
   ```

## Application Deployment

### Initial Setup

1. **Upload Application Code**:
   ```bash
   # Option A: Using Git (recommended)
   sudo git clone https://github.com/your-username/dot-net-tutor.git /opt/dotnet-tutor
   
   # Option B: Using SCP
   scp -i your-key.pem -r . ec2-user@your-instance-ip:/tmp/dotnet-tutor
   sudo mv /tmp/dotnet-tutor /opt/dotnet-tutor
   ```

2. **Set Permissions**:
   ```bash
   sudo chown -R ec2-user:ec2-user /opt/dotnet-tutor
   sudo chmod +x /opt/dotnet-tutor/deployment/*.sh
   ```

3. **Configure Environment**:
   ```bash
   cd /opt/dotnet-tutor
   
   # Edit environment variables
   sudo nano .env
   
   # Update JWT secret key (IMPORTANT!)
   JWT_SECRET_KEY=your-super-secret-key-that-is-at-least-32-characters-long-for-production
   ```

### Deploy Application

1. **Initial Deployment**:
   ```bash
   cd /opt/dotnet-tutor
   sudo ./deployment/deploy.sh deploy
   ```

2. **Check Status**:
   ```bash
   sudo ./deployment/deploy.sh status
   ```

3. **View Logs**:
   ```bash
   sudo ./deployment/deploy.sh logs
   ```

## Configuration

### Environment Variables

Create or update `.env` file in the application root:

```env
# Security
JWT_SECRET_KEY=your-super-secret-key-that-is-at-least-32-characters-long-for-production

# Environment
ASPNETCORE_ENVIRONMENT=Production

# Optional: Database (if using external database)
# CONNECTION_STRING=your-database-connection-string
```

### SSL/HTTPS Setup (Optional)

1. **Obtain SSL Certificate**:
   ```bash
   # Using Let's Encrypt (recommended)
   sudo yum install -y certbot
   sudo certbot certonly --standalone -d your-domain.com
   ```

2. **Update Nginx Configuration**:
   - Uncomment HTTPS server block in `nginx/nginx.conf`
   - Update certificate paths
   - Update domain name

3. **Restart Services**:
   ```bash
   sudo docker-compose restart nginx
   ```

## Monitoring and Maintenance

### Health Checks

- **Application Health**: `http://your-domain/health`
- **API Health**: `http://your-domain/api/health`
- **Container Status**: `sudo docker-compose ps`

### Logs

- **Application Logs**: `sudo docker-compose logs -f`
- **System Logs**: `sudo tail -f /var/log/messages`
- **CloudWatch**: Available in AWS Console

### Updates

1. **Deploy Updates**:
   ```bash
   cd /opt/dotnet-tutor
   git pull origin main
   sudo ./deployment/deploy.sh deploy
   ```

2. **Rollback if Needed**:
   ```bash
   sudo ./deployment/deploy.sh rollback
   ```

### Backup

1. **Database Backup**:
   ```bash
   sudo cp /opt/dotnet-tutor/data/DotNetTutorDb.db /backup/
   ```

2. **Full Application Backup**:
   ```bash
   sudo tar -czf /backup/dotnet-tutor-$(date +%Y%m%d).tar.gz /opt/dotnet-tutor
   ```

## Troubleshooting

### Common Issues

1. **Containers Not Starting**:
   ```bash
   sudo docker-compose logs
   sudo docker system prune -f
   sudo ./deployment/deploy.sh deploy
   ```

2. **Database Issues**:
   ```bash
   # Check database file permissions
   ls -la /opt/dotnet-tutor/data/
   
   # Reset database (WARNING: This will delete all data)
   sudo rm /opt/dotnet-tutor/data/DotNetTutorDb.db
   sudo docker-compose restart backend
   ```

3. **Port Conflicts**:
   ```bash
   # Check what's using ports
   sudo netstat -tlnp | grep :80
   sudo netstat -tlnp | grep :443
   ```

4. **Memory Issues**:
   ```bash
   # Check memory usage
   free -h
   sudo docker system prune -a
   ```

### Performance Tuning

1. **Increase Instance Size**: Upgrade to larger EC2 instance type
2. **Database Optimization**: Consider RDS for production
3. **CDN**: Use CloudFront for static assets
4. **Load Balancing**: Use Application Load Balancer for multiple instances

## Security Considerations

1. **Change Default Secrets**: Update JWT secret key
2. **Firewall Rules**: Restrict SSH access to specific IPs
3. **SSL/TLS**: Enable HTTPS in production
4. **Updates**: Keep system and Docker images updated
5. **Monitoring**: Set up CloudWatch alarms

## Cost Optimization

1. **Instance Sizing**: Start with t3.small and scale as needed
2. **Reserved Instances**: Use for long-term deployments
3. **Spot Instances**: Consider for development environments
4. **CloudWatch**: Monitor and set up billing alerts

## Support

For issues and questions:
1. Check application logs
2. Review CloudWatch metrics
3. Consult AWS documentation
4. Contact system administrator

---

**Important**: Always test deployments in a staging environment before production!