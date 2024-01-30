# MicroServicesRedis

A couple of microservices implementing e-commerce modules over Catalog, Shopping Cart, Discount and Ordering microservices with NoSQL (MongoDB, Redis) and Relational databases (PostgreSQL, Sql Server) with communicating over RabbitMQ Event Driven Communication and using Ocelot API Gateway.

Installing
Follow these steps to get your development environment set up: (Before Run Start the Docker Desktop)

Once Docker for Windows is installed, go to the Settings > Advanced option, from the Docker icon in the system tray, to configure the minimum amount of memory and CPU like so:
Memory: 4 GB
CPU: 2
At the root directory which include docker-compose.yml files, run below command:
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
Wait for docker compose all microservices. That’s it! (some microservices need extra time to work so please wait if not worked in first shut)
