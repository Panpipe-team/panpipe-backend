FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln .
COPY Panpipe/*.csproj ./Panpipe/
RUN dotnet restore ./Panpipe/Panpipe.csproj

COPY . .
WORKDIR /src/Panpipe
RUN dotnet publish -c Release -o /app


FROM mcr.microsoft.com/dotnet/aspnet:8.0  AS final
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT [ "dotnet", "Panpipe.dll" ]
