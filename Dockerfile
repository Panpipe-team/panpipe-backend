FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Panpipe/*.csproj ./Panpipe/
RUN dotnet restore ./Panpipe/

COPY src/ ./
RUN dotnet publish -c Release -o /app ./Panpipe/


FROM mcr.microsoft.com/dotnet/aspnet:8.0  AS final
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT [ "dotnet", "Panpipe.dll" ]
