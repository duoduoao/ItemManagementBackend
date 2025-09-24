# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
USER app

# Build image with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the WebAPI csproj and restore (to optimize caching)
COPY ["ItemManagement.WebAPI/ItemManagement.WebAPI.csproj", "ItemManagement.WebAPI/"]
# Also copy other projects' csproj files (for dotnet restore)
COPY ["ItemManagement.Application/ItemManagement.Application.csproj", "ItemManagement.Application/"]
COPY ["ItemManagement.Domain/ItemManagement.Domain.csproj", "ItemManagement.Domain/"]
COPY ["ItemManagement.Infrastructure/ItemManagement.Infrastructure.csproj", "ItemManagement.Infrastructure/"]

RUN dotnet restore "ItemManagement.WebAPI/ItemManagement.WebAPI.csproj"

# Copy all source files
COPY . .

WORKDIR "/src/ItemManagement.WebAPI"

RUN dotnet build "ItemManagement.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ItemManagement.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ItemManagement.WebAPI.dll"]
