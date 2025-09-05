
#!/bin/bash

# EC2 User Data Script for DotNet Tutor Application
# This script will run on first boot of the EC2 instance

# Update system
yum update -y

# Install Docker
yum install -y docker
systemctl start docker
systemctl enable docker

# Install Docker Compose
curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
chmod +x /usr/local/bin/docker-compose

# Install Git
yum install -y git

# Install AWS CLI v2
curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip"
unzip awscliv2.zip
./aws/install

# Create application directory
mkdir -p /opt/dotnet-tutor
cd /opt/dotnet-tutor

# Clone the repository (replace with your actual repository URL)
# git clone https://github.com/your-username/dot-net-tutor.git .

# Create environment file
cat > .env << EOF
JWT_SECRET_KEY=your-super-secret-key-that-is-at-least-32-characters-long-for-production-change-this
ASPNETCORE_ENVIRONMENT=Production
EOF

# Create data directory for SQLite database
mkdir -p data

# Set proper permissions
chown -R ec2-user:ec2-user /opt/dotnet-tutor
chmod +x /opt/dotnet-tutor/deployment/*.sh

# Add ec2-user to docker group
usermod -a -G docker ec2-user

# Create systemd service for the application
cat > /etc/systemd/system/dotnet-tutor.service << EOF
[Unit]
Description=DotNet Tutor Application
Requires=docker.service
After=docker.service

[Service]
Type=oneshot
RemainAfterExit=yes
WorkingDirectory=/opt/dotnet-tutor
ExecStart=/usr/local/bin/docker-compose up -d
ExecStop=/usr/local/bin/docker-compose down
TimeoutStartSec=0
User=ec2-user
Group=ec2-user

[Install]
WantedBy=multi-user.target
EOF

# Enable and start the service
systemctl daemon-reload
systemctl enable dotnet-tutor.service

# Install CloudWatch agent for monitoring
wget https://s3.amazonaws.com/amazoncloudwatch-agent/amazon_linux/amd64/latest/amazon-cloudwatch-agent.rpm
rpm -U ./amazon-cloudwatch-agent.rpm

# Create CloudWatch agent configuration
cat > /opt/aws/amazon-cloudwatch-agent/etc/amazon-cloudwatch-agent.json << EOF
{
    "logs": {
        "logs_collected": {
            "files": {
                "collect_list": [
                    {
                        "file_path": "/var/log/messages",
                        "log_group_name": "/aws/ec2/dotnet-tutor/system",
                        "log_stream_name": "{instance_id}"
                    },
                    {
                        "file_path": "/var/log/docker",
                        "log_group_name": "/aws/ec2/dotnet-tutor/docker",
                        "log_stream_name": "{instance_id}"
                    }
                ]
            }
        }
    },
    "metrics": {
        "namespace": "DotNetTutor/EC2",
        "metrics_collected": {
            "cpu": {
                "measurement": [
                    "cpu_usage_idle",
                    "cpu_usage_iowait",
                    "cpu_usage_user",
                    "cpu_usage_system"
                ],
                "metrics_collection_interval": 60
            },
            "disk": {
                "measurement": [
                    "used_percent"
                ],
                "metrics_collection_interval": 60,
                "resources": [
                    "*"
                ]
            },
            "diskio": {
                "measurement": [
                    "io_time"
                ],
                "metrics_collection_interval": 60,
                "resources": [
                    "*"
                ]
            },
            "mem": {
                "measurement": [
                    "mem_used_percent"
                ],
                "metrics_collection_interval": 60
            }
        }
    }
}
EOF

# Start CloudWatch agent
/opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-ctl -a fetch-config -m ec2 -c file:/opt/aws/amazon-cloudwatch-agent/etc/amazon-cloudwatch-agent.json -s

# Log completion
echo "EC2 initialization completed at $(date)" >> /var/log/user-data.log