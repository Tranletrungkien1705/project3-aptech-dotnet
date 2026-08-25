# Student/Art Institute Mgmt (net6.0) — SQLite, build-from-source cho cloud 24/7.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY eProject3/eProject3.csproj eProject3/
RUN dotnet restore eProject3/eProject3.csproj
COPY . .
RUN dotnet publish eProject3/eProject3.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/publish ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "eProject3.dll"]
