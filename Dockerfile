FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Panpipe.Domain/*.csproj ./Panpipe.Domain/
COPY src/Panpipe.Services/*.csproj ./Panpipe.Services/
COPY src/Panpipe.Controllers/*.csproj ./Panpipe.Controllers/
COPY src/Panpipe.Infrastructure/*.csproj ./Panpipe.Infrastructure/
COPY src/Panpipe.WebApi/*.csproj ./Panpipe.WebApi/
RUN dotnet restore ./Panpipe.WebApi/

COPY src/ ./
RUN dotnet publish -c Release -o /app ./Panpipe.WebApi/


FROM mcr.microsoft.com/dotnet/aspnet:8.0  AS final
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT [ "dotnet", "Panpipe.WebApi.dll" ]
