# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Restaurants.slnx", "./"]
COPY ["src/Restaurants.Domain/Restaurants.Domain.csproj", "src/Restaurants.Domain/"]
COPY ["src/Restaurants.Application/Restaurants.Application.csproj", "src/Restaurants.Application/"]
COPY ["src/Restaurants.Infrastructure/Restaurants.Infrastructure.csproj", "src/Restaurants.Infrastructure/"]
COPY ["src/Restaurants.API/Restaurants.API.csproj", "src/Restaurants.API/"]

# Restore NuGet packages
RUN dotnet restore "src/Restaurants.API/Restaurants.API.csproj"

# Copy the remaining source code
COPY . .

# Build and publish the API project
WORKDIR "/src/src/Restaurants.API"
RUN dotnet publish "Restaurants.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Restaurants.API.dll"]