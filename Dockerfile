# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar
COPY . ./
RUN dotnet restore

# Publicar en modo Release
RUN dotnet publish -c Release -o /app/publish

# Etapa de runtime final (usando .NET 8)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Comando de inicio
ENTRYPOINT ["dotnet", "WarApi.dll"]
