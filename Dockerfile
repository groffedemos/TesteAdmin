FROM mcr.microsoft.com/dotnet/sdk:3.1.412 AS build-env
WORKDIR /app

# Copiar arquivos .csproj e restaurar dependencias
COPY ANPAdmin.Data/ANPAdmin.Data.csproj ./ANPAdmin.Data/
COPY ANPAdmin.Business/ANPAdmin.Business.csproj ./ANPAdmin.Business/
COPY ANPAdmin.UI/ANPAdmin.UI.csproj ./ANPAdmin.UI/
RUN dotnet restore ANPAdmin.UI/ANPAdmin.UI.csproj

# Build da aplicacao
COPY . ./
RUN dotnet publish ANPAdmin.UI/ANPAdmin.UI.csproj -c Release -o out

# Build da imagem
FROM mcr.microsoft.com/dotnet/aspnet:3.1.18
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "ANPAdmin.UI.dll"]