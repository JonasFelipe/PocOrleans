# 🏗️ poc_orleans_onion

A **.NET 8** application using **Orleans 7**, **Onion Architecture**, **SOLID Principles**, **Entity Framework Core**, **Docker**, and **Unit Testing** with **xUnit & NSubstitute**.

## 📌 Features
✅ **Orleans 7** for distributed computing  
✅ **Onion Architecture** (Domain, Application, Infrastructure, API layers)  
✅ **SOLID Principles** for maintainability and scalability  
✅ **Entity Framework Core** with **SQL Server**  
✅ **Dockerized** with `docker-compose`  
✅ **Unit tests** using **xUnit & NSubstitute**  

---

## 🚀 **Getting Started**
Follow these steps to set up the project.

### **1️⃣ Clone the Repository**
```sh
git clone https://github.com/yourusername/poc_orleans_onion.git
cd poc_orleans_onion
2️⃣ Install Dependencies
sh
Copy code
dotnet restore
3️⃣ Setup the Database
Run EF Core migrations:

sh
Copy code
dotnet ef migrations add InitialCreate --project OrleansOnionSOLID.Infrastructure
dotnet ef database update --project OrleansOnionSOLID.Infrastructure
4️⃣ Run the Application
👉 Run Locally
sh
Copy code
dotnet run --project OrleansOnionSOLID.API
👉 Run with Docker
sh
Copy code
docker-compose up --build
The API will be available at http://localhost:5000.

🔥 Testing the API
Use cURL or Postman to test the API.

Create a User
sh
Copy code
curl -X POST "http://localhost:5000/api/user?id=550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{"name": "John Doe", "email": "john@example.com"}'
Get a User
sh
Copy code
curl -X GET "http://localhost:5000/api/user/550e8400-e29b-41d4-a716-446655440000"
🧪 Running Unit Tests
The project includes unit tests for services and repositories using xUnit & NSubstitute.

Run Tests
sh
Copy code
dotnet test
Expected Output
csharp
Copy code
Passed! - 2 tests from OrleansOnionSOLID.Tests.Repositories.UserRepositoryTests
Passed! - 2 tests from OrleansOnionSOLID.Tests.Services.UserProfileServiceTests
🏗 Project Structure
bash
Copy code
poc_orleans_onion/
│-- OrleansOnionSOLID.API/            # API Layer (Presentation)
│-- OrleansOnionSOLID.Application/    # Application Layer (Business Logic)
│-- OrleansOnionSOLID.Domain/         # Domain Layer (Entities & Interfaces)
│-- OrleansOnionSOLID.Infrastructure/ # Infrastructure Layer (Persistence & Orleans)
│-- OrleansOnionSOLID.Tests/          # Unit Tests (xUnit & NSubstitute)
│-- docker-compose.yml                # Docker Configuration
│-- README.md                         # Project Documentation
🐳 Docker Setup
Build & Run Containers
sh
Copy code
docker-compose up --build
Stop Containers
sh
Copy code
docker-compose down
Check Running Containers
sh
Copy code
docker ps
🔹 Docker Configuration
The project includes a Dockerfile and docker-compose.yml file.

Dockerfile for API
dockerfile
Copy code
# Use official .NET 8 SDK as build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore dependencies
COPY *.sln .
COPY OrleansOnionSOLID.Domain/*.csproj OrleansOnionSOLID.Domain/
COPY OrleansOnionSOLID.Application/*.csproj OrleansOnionSOLID.Application/
COPY OrleansOnionSOLID.Infrastructure/*.csproj OrleansOnionSOLID.Infrastructure/
COPY OrleansOnionSOLID.API/*.csproj OrleansOnionSOLID.API/

RUN dotnet restore OrleansOnionSOLID.API/OrleansOnionSOLID.API.csproj

# Copy everything and build
COPY . .
WORKDIR /app/OrleansOnionSOLID.API
RUN dotnet publish -c Release -o /out

# Use official .NET runtime as execution environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["dotnet", "OrleansOnionSOLID.API.dll"]
🧪 Unit Tests
The project includes unit tests using xUnit & NSubstitute.

Example: UserProfileServiceTests
csharp
Copy code
using System;
using System.Threading.Tasks;
using NSubstitute;
using Orleans;
using OrleansOnionSOLID.Application.Interfaces;
using OrleansOnionSOLID.Application.Services;
using OrleansOnionSOLID.Domain.Entities;
using OrleansOnionSOLID.Domain.Interfaces;
using Xunit;

namespace OrleansOnionSOLID.Tests.Services
{
    public class UserProfileServiceTests
    {
        private readonly IClusterClient _orleansClient;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileService _service;

        public UserProfileServiceTests()
        {
            _orleansClient = Substitute.For<IClusterClient>();
            _userRepository = Substitute.For<IUserRepository>();
            _service = new UserProfileService(_orleansClient, _userRepository);
        }

        [Fact]
        public async Task GetProfileAsync_ShouldReturnUserProfile()
        {
            var userId = Guid.NewGuid();
            var mockUser = new UserProfile { Id = userId, Name = "John Doe", Email = "john@example.com" };

            var grain = Substitute.For<IUserProfileGrain>();
            grain.GetProfile().Returns(Task.FromResult(mockUser));

            _orleansClient.GetGrain<IUserProfileGrain>(userId).Returns(grain);

            var result = await _service.GetProfileAsync(userId);

            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("john@example.com", result.Email);
        }
    }
}
