FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine3.17 AS publish
WORKDIR /src
COPY . .
RUN dotnet publish "ShortenUrl.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.18 
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80

ENTRYPOINT ["dotnet", "ShortenUrl.dll"]
 