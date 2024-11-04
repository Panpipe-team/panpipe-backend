FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Panpipe.WebApi/Panpipe.WebApi.csproj", "src/Panpipe.WebApi/"]
COPY ["src/Panpipe.Infrastructure/Panpipe.Infrastructure.csproj", "src/Panpipe.Infrastructure/"]
COPY ["src/Panpipe.Domain/Panpipe.Domain.csproj", "src/Panpipe.Domain/"]
RUN dotnet restore "src/Panpipe.WebApi/Panpipe.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Panpipe.WebApi"
RUN dotnet build "Panpipe.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Panpipe.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Panpipe.WebApi.dll"]
