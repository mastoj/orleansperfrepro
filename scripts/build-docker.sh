#!/bin/bash

# Check if a label argument was provided
if [ -z "$1" ]; then
    echo "Error: Please provide a label for the Docker images"
    echo "Usage: ./build-docker.sh <label>"
    exit 1
fi

LABEL=$1
REPO="orleansdemo"
ROOT_DIR=$(dirname "$(dirname "$(realpath "$0")")")

echo "Building Docker images with label: $LABEL"

# Build the client image
echo "Building client image..."
docker build -t $REPO/client:$LABEL -f $ROOT_DIR/orleansdemo.client/Dockerfile $ROOT_DIR

# Build the server image
echo "Building server image..."
docker build -t $REPO/server:$LABEL -f $ROOT_DIR/orleansdemo.server/Dockerfile $ROOT_DIR

echo "Build completed successfully!"
echo "Created images:"
echo "- $REPO/client:$LABEL"
echo "- $REPO/server:$LABEL"