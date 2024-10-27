FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Panpipe.WebApi/*.csproj ./Panpipe.WebApi/
RUN dotnet restore ./Panpipe.WebApi/

COPY src/Panpipe.WebApi/ ./Panpipe.WebApi/
RUN dotnet publish -c Release -o /app ./Panpipe.WebApi/


FROM mcr.microsoft.com/dotnet/aspnet:8.0  AS final
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT [ "dotnet", "Panpipe.WebApi.dll" ]
