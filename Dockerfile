# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and all projects
COPY *.sln ./
COPY src/ ./src/

# Restore using the solution file
RUN dotnet restore

# Publish the main web app project
WORKDIR /src/src/DevOpsQuickScan.Web
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "DevOpsQuickScan.Web.dll"]