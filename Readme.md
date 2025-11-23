# F1 Telemetry Dashboard

A C# + React project that streams simulated Formula 1 telemetry through RabbitMQ using the pub/sub pattern and visualises it on a real-time dashboard. The backend publishes telemetry events, and a WebSocket gateway relays them to the frontend for live updates.

## Tech Stack

![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/rabbitmq-%23FF6600.svg?&style=for-the-badge&logo=rabbitmq&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

## High Level Architecture

- **Telemetry API (C#):** Generates simulated telemetry and publishes messages to RabbitMQ exchanges.
- **WebSocket Gateway (C#):** Subscribes to RabbitMQ queues and streams received telemetry to the frontend.
- **React Dashboard:** Receives real-time updates via WebSockets and renders telemetry components.
- **RabbitMQ:** Acts as the message broker implementing pub/sub pattern.
- **Docker:** Provides a consistent, containerised environment for the backend, frontend, and RabbitMQ.

## Features

- Real-time telemetry streaming delivered via RabbitMQ pub/sub.
- Dashboard cards displaying driver/session information at a glance.
- Durable and messaging setup (exchanges, queues, routing keys, acknowledgements).
- Fully containerised system using Docker

## Goals & Requirements

The primary goal of this project was to gain practical experience with messaging queues and specifically the pub/sub pattern by integrating RabbitMQ into a C# backend and connecting it to a real-time React dashboard.

Key objectives / learnings included:

- Understanding core messaging concepts: producers, consumers, exchanges, routing keys, bindings, message persistence, and queue durability.
- Implement a pub/sub workflow where multiple consumers can receive the same telemetry messages simultaneously.
- Simulate a real-world scenario (F1 telemetry streaming) to better understand how distributed systems handle continuous, high-frequency data.
- Build a resilient messaging setup, including handling reconnection, message acknowledgement, and fault-tolerant configuration.
- Connecting an asynchronous data stream to a React UI and ensuring smooth, real-time rendering.
- Containerising the full system to provide a reproducible environment for development and testing.
- Exploring trade-offs between messaging systems and REST APIs, and understanding when asynchronous communication is beneficial.

## Project Thinking

### Why RabbitMQ over Kafka or Azure Service Bus?

- Lightweight and simple to run locally.
- Easy to follow documentation
- Not tied to a specific cloud provider.
- More approachable than Kafka for small-scale pub/sub projects.

### Why pub/sub instead of a single queue?

- Allows multiple consumers to receive the same data stream.
- Matches real-time telemetry broadcasting scenarios.
- Enables easy addition of future consumers (analytics, logging, event storage).

### Why React instead of Blazor?

- Familiarity allowed faster iteration and focus on backend/messaging concepts.
- Strong ecosystem for real-time UI patterns.
- Easy to prototype dashboard components.

### Why Dockerised?

- Ensures consistent environments across all services.
- Simplifies startup, teardown, and sharing.
- Reflects real-world distributed system deployment practices.

## Key Learnings

This project was my introduction into using RabbitMQ with C# and React and learning about messaging queues and the pub/sub pattern.

- Designing RabbitMQ queues for pub/sub workloads.
- The benefits of messaging queus and webhooks over RESTful API's in specific circumstances
- Coordinating producer/consumer logic in C# and handling retries/dead-lettering.
- Streaming data to a React UI via WebSockets while keeping the dashboard performant.
- Building distributed systems using asynchronous communication patterns.

Overall, this project expanded my understanding of Client/Server communication beyond REST APIs and into message-driven architecture, real-time communication, and distributed systems.

## For the future

- **Abstract messaging logic**  
  Create a messaging interface to support swapping RabbitMQ for Kafka, Azure Service Bus, etc.

- **Add more telemetry variables**  
  Increase realism by including RPM, gear shifts, ERS deployment, and more.

- **Enhance UI**  
  Introduce multiple queues for different categories of telemetry and visualise them with dedicated dashboard components. e.g. Telemetry with concerning data and render these within the UI

## Setup

1. Install Docker
2. Run `docker-compose up --build`
3. Open http://localhost:3000 in your browser
