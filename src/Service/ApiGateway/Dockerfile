ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", ".editorconfig", "./"]
COPY ["src/Service/ApiGateway/Orchid.ApiGateway.csproj", "src/Service/ApiGateway/"]
RUN dotnet restore "src/Service/ApiGateway/Orchid.ApiGateway.csproj"

COPY ["src/Service/ApiGateway/", "src/Service/ApiGateway/"]
RUN dotnet publish "src/Service/ApiGateway/Orchid.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final
USER $APP_UID
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Orchid.ApiGateway.dll"]
