# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files first so `dotnet restore` is cached unless a .csproj actually changes.
COPY ["src/CleanApp.WebApi/CleanApp.WebApi.csproj", "src/CleanApp.WebApi/"]
COPY ["src/CleanApp.Domain/CleanApp.Domain.csproj", "src/CleanApp.Domain/"]
COPY ["src/CleanApp.Application/CleanApp.Application.csproj", "src/CleanApp.Application/"]
COPY ["src/CleanApp.Persistence/CleanApp.Persistence.csproj", "src/CleanApp.Persistence/"]
COPY ["src/CleanApp.Infrastructure/CleanApp.Infrastructure.csproj", "src/CleanApp.Infrastructure/"]
COPY ["src/CleanApp.Presentation/CleanApp.Presentation.csproj", "src/CleanApp.Presentation/"]
RUN dotnet restore "src/CleanApp.WebApi/CleanApp.WebApi.csproj"

COPY src/ src/
WORKDIR "/src/src/CleanApp.WebApi"
RUN dotnet build "CleanApp.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CleanApp.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanApp.WebApi.dll"]
