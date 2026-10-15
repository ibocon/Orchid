ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", ".editorconfig", "./"]
COPY ["src/Shared/Contract/Orchid.Contract.csproj", "src/Shared/Contract/"]
COPY ["src/Service/Clipboard/Orchid.Service.Clipboard.csproj", "src/Service/Clipboard/"]
RUN dotnet restore "src/Service/Clipboard/Orchid.Service.Clipboard.csproj"

COPY ["src/Shared/Contract/", "src/Shared/Contract/"]
COPY ["src/Service/Clipboard/", "src/Service/Clipboard/"]
RUN dotnet publish "src/Service/Clipboard/Orchid.Service.Clipboard.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final
USER $APP_UID
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Orchid.Service.Clipboard.dll"]
