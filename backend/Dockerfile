# 1. Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY Chatapi/*.csproj ./Chatapi/
RUN dotnet restore Chatapi/Chatapi.csproj

COPY . .
RUN dotnet publish Chatapi/Chatapi.csproj -c Release -o out

# 2. Çalıştırma aşaması
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Chatapi.dll"]
