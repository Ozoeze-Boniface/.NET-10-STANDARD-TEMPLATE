FROM ghcr.io/sterling-retailcore-team/dotnet-base-image:8.0 AS base
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base

# Expose ports
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM ghcr.io/sterling-retailcore-team/dotnet-base-image:8.0 AS build
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ["src/CityCode.MandateSystem.Api/CityCode.MandateSystem.Api.csproj", "src/CityCode.MandateSystem.Api/"]
COPY ["src/CityCode.MandateSystem.Application/CityCode.MandateSystem.Application.csproj", "src/CityCode.MandateSystem.Application/"]
COPY ["src/CityCode.MandateSystem.Domain/CityCode.MandateSystem.Domain.csproj", "src/CityCode.MandateSystem.Domain/"]
COPY ["src/CityCode.MandateSystem.Infrastructure/CityCode.MandateSystem.Infrastructure.csproj", "src/CityCode.MandateSystem.Infrastructure/"]
COPY ["src/CityCode.MandateSystem.WorkerService/CityCode.MandateSystem.WorkerService.csproj", "src/CityCode.MandateSystem.WorkerService/"]
COPY Directory.Build.props ./
COPY Directory.Packages.props ./
RUN dotnet restore "src/CityCode.MandateSystem.Api/CityCode.MandateSystem.Api.csproj"
RUN dotnet restore "src/CityCode.MandateSystem.Infrastructure/CityCode.MandateSystem.Infrastructure.csproj"


COPY . .
WORKDIR "/app/src/CityCode.MandateSystem.Api"
RUN dotnet build "CityCode.MandateSystem.Api.csproj" -c Release -o /app/build

FROM build AS publish

# Publish the application
RUN dotnet publish "CityCode.MandateSystem.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

# Copy the published app from build stage
COPY --from=publish /app/publish .

# Command to run the application
ENTRYPOINT ["dotnet", "CityCode.MandateSystem.Api.dll"]
