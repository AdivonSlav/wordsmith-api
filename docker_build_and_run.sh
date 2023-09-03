#!/bin/bash

# Define vars
api_tag="wordsmith/wordsmith_api"
api_dockerfile="Wordsmith.API/Dockerfile"

identityserver_tag="wordsmith/wordsmith_identityserver"
identityserver_dockerfile="Wordsmith.IdentityServer/Dockerfile"

# Build image
echo "Building image $api_tag..."
docker build -f $api_dockerfile -t $api_tag:latest .

# Check build status
if [ $? -eq 0 ]; then
  echo "$api_tag built succesfully"
else
  echo "$api_tag build failed"
  exit 1
fi

# Build image
echo "Building image $identityserver_tag..."
docker build -f $identityserver_dockerfile -t $identityserver_tag:latest .

# Check build status
if [ $? -eq 0 ]; then
  echo "$identityserver_tag built succesfully"
else
  echo "$identityserver_tag build failed"
  exit 1
fi

# Run docker compose
echo "All images built succesfully, proceeding to docker compose..."
docker compose up -d

# Check build status
if [ $? -eq 0 ]; then
  echo "Services started succesfully"
else
  echo "Docker Compose failed to start services!"
  exit 1
fi
