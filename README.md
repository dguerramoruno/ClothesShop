# ClothesShop
Project Overview
This project is a RESTful API for managing products in a store, built with .NET Core. The application allows performing the following operations:


Create products.
Edit products.
Delete products.
List products.
User authentication with JWT tokens.
API security through token-based authorization.
Project Architecture
The project follows Clean Architecture principles and is divided into the following layers:

1. Domain
This layer contains:

Entities: Represent the domain data model, such as Product.
Interfaces: Contracts for repositories or services (e.g., IProductRepository).
2. Application
This layer contains:

DTOs: Data Transfer Objects used for communication between layers.
Services: Implementations containing business logic (e.g., ProductService).
3. Infrastructure
This layer includes:

Database Context: Entity Framework Core configuration (e.g., ProductDbContext).
Repositories: Implementations of repository interfaces defined in the domain layer.
4. API
The presentation layer includes:

Controllers: Expose endpoints for CRUD operations (e.g., ProductsController).
Error Handling: Manages exceptions to provide appropriate responses to the client.
JWT Authentication: Protects endpoints by requiring a valid JWT token for specific operations.
Docker Setup
The project includes a Docker container to simplify configuration and deployment. The container includes:

# Azure Deployment
The API and the database are deployed in Azure. You can access the deployed API at the following URL:

https://clotheshop-dme6f9h9a3c8h2ca.spaincentral-01.azurewebsites.net


A .NET-based API image.
A SQL Server database.
Steps to Run with Docker
Install Docker: Make sure Docker is installed on your machine.

Create the image: In the root of the project, run:


docker build -t clothesshop-api .
Start the container: Use the included docker-compose.yml file to bring up the API and the database:


docker-compose up
Verify: The API will be available at http://localhost:5000 (or the port configured in the docker-compose.yml file).

Relevant Files
Dockerfile: Contains instructions to build the Docker image for the API.
docker-compose.yml: Configures the API and database services.
Tests
The project includes unit and integration tests using xUnit. These tests validate the functionality of the various layers and ensure that services and controllers work as expected.

Running Tests
Make sure you are in the root directory of the project.

Run the following command:

# dotnet test

Included Tests

Services: Verify the business logic implemented in the services.
Controllers: Ensure that the endpoints respond as expected.
How to Use the API
Main Endpoints
GET /api/products

Retrieves all products.
GET /api/products/{id}

Retrieves a product by its ID.
POST /api/products

Creates a new product.
Body (JSON):

json
Copiar
Editar
{
  "size": "M",
  "color": "Red",
  "price": 25.99,
  "description": "Red cotton t-shirt"
}
PUT /api/products/{id}

Updates an existing product.
Body (JSON): Same as POST.

DELETE /api/products/{id}

Deletes a product by its ID.
JWT Authentication
To interact with certain protected endpoints, you need to authenticate using a JWT token.

Login to obtain a token:

POST to /api/auth/login with your username and password to receive a JWT token.
Use the token:

Include the token in the Authorization header of your request, like this:

Authorization: Bearer <your_jwt_token>
Final Notes
This project is designed to be extensible and follows best development practices. If you encounter any issues or have suggestions, feel free to create an issue in the repository or contribute with a pull request.

