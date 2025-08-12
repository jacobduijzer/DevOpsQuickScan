FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
COPY .  /src
WORKDIR /src
RUN dotnet restore && \
    dotnet build --no-restore && \
    dotnet test --no-build --no-restore 
    
WORKDIR /src/src/DevOpsQuickScan.BlazorApp
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "DevOpsQuickScan.BlazorApp.dll"]
