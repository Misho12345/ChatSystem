# Chat System

This project is a real-time chat application built with a microservices architecture using .NET, Docker, and SignalR. It allows users to register, manage friendships, and engage in private conversations.

## Features

* **User Authentication**: Secure JWT-based authentication with refresh tokens.
* **Friendship Management**: Send, accept, and manage friend requests.
* **Real-time Chat**: Instant messaging powered by SignalR.
* **Separate Services**: A `UserAccountService` for identity and a `ChatService` for messaging.
* **Containerized**: Fully containerized with Docker for easy setup and deployment.

## Tech Stack

* **Backend**: ASP.NET Core 9
* **Databases**: PostgreSQL & MongoDB
* **Real-time**: SignalR
* **Containerization**: Docker & Docker Compose
* **Frontend**: Vanilla JavaScript, HTML5, CSS3

-----

## Getting Started

Follow these instructions to get the project running on your local machine.

### Prerequisites

* [Docker](https://www.docker.com/products/docker-desktop/)
* [Docker Compose](https://docs.docker.com/compose/install/)

### 1\. Configure Environment Secrets

Before you start, create a `.env` file in the root directory of the project. This is crucial for storing database credentials and JWT secrets.

Create a file named `.env` and add the following content. **Remember to replace the placeholder values with your own strong secrets.**

```env
# PostgreSQL Credentials for the UserAccountService
POSTGRES_USER=my_pg_user
POSTGRES_PASSWORD=my_pg_password
POSTGRES_DB=my_pg_db

# MongoDB Credentials for the ChatService
MONGO_USER=my_mongo_admin
MONGO_PASSWORD=my_mongo_admin_password
MONGO_DB=my_mongo_db

# A long, random secret key for signing JWT tokens
JWT_SECRET=your_super_secret_and_long_jwt_key_that_is_at_least_32_chars
```

### 2\. Build and Run the Project

1.  Open a terminal in the project's root directory.

2.  Run the following command to build the Docker images and start all services:

    ```bash
    docker-compose up --build
    ```

    *You can add the `-d` flag to run the containers in detached mode (in the background).*

3.  The application is now running\!

    * The frontend is accessible at: **`http://localhost:3000`**

-----

## API Documentation (Swagger)

Each backend service exposes a Swagger UI for exploring and testing the API endpoints. Once the containers are running, you can access them at:

* **UserAccountService API**: [http://localhost:8001/swagger](https://www.google.com/search?q=http://localhost:8001/swagger)
* **ChatService API**: [http://localhost:8002/swagger](https://www.google.com/search?q=http://localhost:8002/swagger)

## Project Structure

The project is divided into several main components:

* `/UserAccountService`: Manages users, authentication, and friendships. Uses PostgreSQL.
* `/ChatService`: Manages conversations and real-time messages. Uses MongoDB.
* `/Frontend`: A simple Vanilla JS frontend served by an NGINX container.
* `docker-compose.yml`: Defines and orchestrates all the services.
* `.github/workflows/ci.yml`: GitHub Actions workflow for continuous integration (linting and testing).