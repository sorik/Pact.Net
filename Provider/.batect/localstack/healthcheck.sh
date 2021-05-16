#!/bin/sh

aws --endpoint-url http://localhost:4566 \
    --region ap-southeast-2 \
    dynamodb describe-table --table-name PactMembership || exit 1