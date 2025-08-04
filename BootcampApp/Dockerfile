# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Postavi NuGet cache folder unutar kontejnera
ENV NUGET_PACKAGES=/root/.nuget/packages

# Kopiraj nuget.config iz podfoldera WebAPI
COPY WebAPI/nuget.config ./

# Kopiraj solution i projekte prije restorea
COPY BootcampApp.sln ./
COPY Bootcamp.App.Service/*.csproj ./Bootcamp.App.Service/
COPY BootcampApp.Repository/*.csproj ./BootcampApp.Repository/
COPY BootcampApp.Model/*.csproj ./BootcampApp.Model/
COPY BootcampApp.Common/*.csproj ./BootcampApp.Common/
COPY BootcampApp.SignalR/*.csproj ./BootcampApp.SignalR/
COPY BootcampApp.Tests/*.csproj ./BootcampApp.Tests/
COPY WebAPI/*.csproj ./WebAPI/

# Restore paketa
RUN dotnet restore

# Kopiraj ostatak koda
COPY . ./

# Build i publish
RUN dotnet publish WebAPI -c Release -o out


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "WebAPI.dll"]
