# Stage 1: Use the ASP.NET runtime image as the base for the final, small image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Stage 2: Use the full .NET SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ["src/CityCode.MandateSystem.Api/CityCode.MandateSystem.Api.csproj", "src/CityCode.MandateSystem.Api/"]
COPY ["src/CityCode.MandateSystem.Application/CityCode.MandateSystem.Application.csproj", "src/CityCode.MandateSystem.Application/"]
COPY ["src/CityCode.MandateSystem.Domain/CityCode.MandateSystem.Domain.csproj", "src/CityCode.MandateSystem.Domain/"]
COPY ["src/CityCode.MandateSystem.Infrastructure/CityCode.MandateSystem.Infrastructure.csproj", "src/CityCode.MandateSystem.Infrastructure/"]
COPY ["src/CityCode.MandateSystem.WorkerService/CityCode.MandateSystem.WorkerService.csproj", "src/CityCode.MandateSystem.WorkerService/"]
COPY Directory.Build.props ./
COPY Directory.Packages.props ./
RUN dotnet restore "src/CityCode.MandateSystem.Api/CityCode.MandateSystem.Api.csproj"

# Copy the rest of the source code and build the application
COPY . .
WORKDIR "/app/src/CityCode.MandateSystem.Api"
RUN dotnet build "CityCode.MandateSystem.Api.csproj" -c Release -o /app/build

# Stage 3: Publish the application
FROM build AS publish
RUN dotnet publish "CityCode.MandateSystem.Api.csproj" -c Release -o /app/publish

# Stage 4: Create the final image from the 'base' stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CityCode.MandateSystem.Api.dll"]
