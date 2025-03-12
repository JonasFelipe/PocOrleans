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
```

### **2️⃣ Install Dependencies**
```sh
dotnet restore
```

### **3️⃣ Setup the Database**
Run EF Core migrations:
```sh
dotnet ef migrations add InitialCreate --project PocOrleans.Infrastructure
dotnet ef database update --project PocOrleans.Infrastructure
```

### **4️⃣ Run the Application**
👉 Run Locally
```sh
dotnet run --project PocOrleans.API
```

👉 Run with Docker
```sh
docker-compose up --build
```
The API will be available at [http://localhost:5000](http://localhost:5000).

---

## 🔥 Testing the API
Use cURL or Postman to test the API.

### Create a User
```sh
curl -X POST "http://localhost:5000/api/user?id=550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{"name": "John Doe", "email": "john@example.com"}'
```

### Get a User
```sh
curl -X GET "http://localhost:5000/api/user/550e8400-e29b-41d4-a716-446655440000"
```

---

## 🧪 Running Unit Tests
The project includes unit tests for services and repositories using xUnit & NSubstitute.

### Run Tests
```sh
dotnet test
```

### Expected Output
```sh
Passed! - 2 tests from PocOrleans.Tests.Repositories.UserRepositoryTests
Passed! - 2 tests from PocOrleans.Tests.Services.UserProfileServiceTests
```

---

## 🏗 Project Structure
```bash
poc_orleans_onion/
│-- PocOrleans.API/            # API Layer (Presentation)
│-- PocOrleans.Application/    # Application Layer (Business Logic)
│-- PocOrleans.Domain/         # Domain Layer (Entities & Interfaces)
│-- PocOrleans.Infrastructure/ # Infrastructure Layer (Persistence & Orleans)
│-- PocOrleans.Tests/          # Unit Tests (xUnit & NSubstitute)
│-- docker-compose.yml                # Docker Configuration
│-- README.md                         # Project Documentation
```

---

## 🐳 Docker Setup

### Build & Run Containers
```sh
docker-compose up --build
```

### Stop Containers
```sh
docker-compose down
```

### Check Running Containers
```sh
docker ps
```

---

## 🔹 Docker Configuration
The project includes a Dockerfile and docker-compose.yml file.

### Dockerfile for API
```dockerfile
# Use official .NET 8 SDK as build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore dependencies
COPY *.sln .
COPY PocOrleans.Domain/*.csproj PocOrleans.Domain/
COPY PocOrleans.Application/*.csproj PocOrleans.Application/
COPY PocOrleans.Infrastructure/*.csproj PocOrleans.Infrastructure/
COPY PocOrleans.API/*.csproj PocOrleans.API/

RUN dotnet restore PocOrleans.API/PocOrleans.API.csproj

# Copy everything and build
COPY . .
WORKDIR /app/PocOrleans.API
RUN dotnet publish -c Release -o /out

# Use official .NET runtime as execution environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["dotnet", "PocOrleans.API.dll"]
```

---

## 🧪 Unit Tests
The project includes unit tests using xUnit & NSubstitute.

### Example: UserProfileServiceTests
```csharp
using System;
using System.Threading.Tasks;
using NSubstitute;
using Orleans;
using PocOrleans.Application.Interfaces;
using PocOrleans.Application.Services;
using PocOrleans.Domain.Entities;
using PocOrleans.Domain.Interfaces;
using Xunit;

namespace PocOrleans.Tests.Services
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
```
