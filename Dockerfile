# syntax=docker/dockerfile:1

# ---- Etapa de build ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY NewLife.csproj ./
RUN dotnet restore NewLife.csproj

COPY . .
RUN dotnet publish NewLife.csproj -c Release -o /app/publish --no-restore

# ---- Etapa de runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Render inyecta PORT en runtime; Program.cs lee esta variable y bindea a 0.0.0.0.
# El valor de EXPOSE es solo documentación de la imagen (10000 = default si Render no la setea).
EXPOSE 10000

ENTRYPOINT ["dotnet", "NewLife.dll"]
