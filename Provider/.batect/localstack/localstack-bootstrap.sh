#!/bin/sh

aws dynamodb --endpoint-url http://localhost:4566 \
    --region ap-southeast-2 \
    create-table --table-name PactMembership \
    --attribute-definitions AttributeName=UserId,AttributeType=S \
    --key-schema AttributeName=UserId,KeyType=HASH \
    --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5

aws dynamodb --endpoint-url http://localhost:4566 \
    --region ap-southeast-2 \
    put-item --table-name PactMembership \
    --item '{"UserId":{"S":"user1"}, "Type":{"S":"fellow"}}'

aws dynamodb --endpoint-url http://localhost:4566 \
    --region ap-southeast-2 \
    scan --table-name PactMembership
