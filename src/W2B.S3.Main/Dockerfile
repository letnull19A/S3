FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["W2B.S3.Main/W2B.S3.Main.csproj", "W2B.S3.Main/"]
RUN dotnet restore "W2B.S3.Main/W2B.S3.Main.csproj"
COPY . .
WORKDIR "/src/W2B.S3.Main"
RUN dotnet build "W2B.S3.Main.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "W2B.S3.Main.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "W2B.S3.Main.dll"]
