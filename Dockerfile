FROM mcr.microsoft.com/dotnet/sdk:9.0 AS test
COPY .  /src
WORKDIR /src
RUN dotnet restore 
RUN dotnet build --no-restore
RUN dotnet test  --no-build --no-restore


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
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
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "DevOpsQuickScan.Web.dll"]

#FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
#ARG TARGETARCH
#COPY . /source
#WORKDIR /source/src
#RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
#    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app
#RUN dotnet test /source/tests
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS development
#COPY . /source
#WORKDIR /source/src
#CMD dotnet run --no-launch-profile
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
#WORKDIR /app
#COPY --from=build /app .
#ARG UID=10001
#RUN adduser \
#    --disabled-password \
#    --gecos "" \
#    --home "/nonexistent" \
#    --shell "/sbin/nologin" \
#    --no-create-home \
#    --uid "${UID}" \
#    appuser
#USER appuser
#ENTRYPOINT ["dotnet", "myWebApp.dll"]
