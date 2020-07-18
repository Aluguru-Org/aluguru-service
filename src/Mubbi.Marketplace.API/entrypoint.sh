#!/bin/bash

set -e
run_cmd="dotnet run --server.urls http://*:5000"

>&2 echo "######## Database Update"
dotnet tool run dotnet-ef -v
until dotnet tool run dotnet-ef database update; do
>&2 echo "######## SQL Server is starting up..."
sleep 1
done

>&2 echo "######## Database Updated - Starting Server"
exec $run_cmd
>&2 echo "######## Server is up and running!"