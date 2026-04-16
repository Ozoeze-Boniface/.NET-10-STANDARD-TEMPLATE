# Stage 1: Use the ASP.NET runtime image as the base for the final, small image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Stage 2: Use the full .NET SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ["src/KeyRails.BankingApi.Api/KeyRails.BankingApi.Api.csproj", "src/KeyRails.BankingApi.Api/"]
COPY ["src/KeyRails.BankingApi.Application/KeyRails.BankingApi.Application.csproj", "src/KeyRails.BankingApi.Application/"]
COPY ["src/KeyRails.BankingApi.Domain/KeyRails.BankingApi.Domain.csproj", "src/KeyRails.BankingApi.Domain/"]
COPY ["src/KeyRails.BankingApi.Infrastructure/KeyRails.BankingApi.Infrastructure.csproj", "src/KeyRails.BankingApi.Infrastructure/"]
COPY ["src/KeyRails.BankingApi.WorkerService/KeyRails.BankingApi.WorkerService.csproj", "src/KeyRails.BankingApi.WorkerService/"]
COPY Directory.Build.props ./
COPY Directory.Packages.props ./
RUN dotnet restore "src/KeyRails.BankingApi.Api/KeyRails.BankingApi.Api.csproj"

# Copy the rest of the source code and build the application
COPY . .
WORKDIR "/app/src/KeyRails.BankingApi.Api"
RUN dotnet build "KeyRails.BankingApi.Api.csproj" -c Release -o /app/build

# Stage 3: Publish the application
FROM build AS publish
RUN dotnet publish "KeyRails.BankingApi.Api.csproj" -c Release -o /app/publish

# Stage 4: Create the final image from the 'base' stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KeyRails.BankingApi.Api.dll"]
