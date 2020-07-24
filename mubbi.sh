#!/bin/bash
# Author: felipe-allmeida
# ----------------------------
$Operation = $1

echo "Operation: $Operation"
if [ $1 -eq "start" ];
then
    docker-compose up -d
elif [ $1 -eq "stop"];
then
    docker-compose down
elif [ $1 -eq "bootstrap" ];
then
    docker-compose build --no-cache
    docker-compose up -d
fi  