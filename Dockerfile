FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Panpipe.Domain/*.csproj ./Panpipe.Domain/
COPY src/Panpipe.Application/*.csproj ./Panpipe.Application/
COPY src/Panpipe.Common/*.csproj ./Panpipe.Common/
COPY src/Panpipe.Persistence/*.csproj ./Panpipe.Persistence/
COPY src/Panpipe.Presentation/*.csproj ./Panpipe.Presentation/
RUN dotnet restore ./Panpipe.Presentation/

COPY src/ ./
RUN dotnet publish -c Release -o /app ./Panpipe.Presentation/


FROM mcr.microsoft.com/dotnet/aspnet:8.0  AS final
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT [ "dotnet", "Panpipe.Presentation.dll" ]
