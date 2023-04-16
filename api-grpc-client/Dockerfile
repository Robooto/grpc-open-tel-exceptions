FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["api-grpc-client.csproj", "api-grpc-client/"]
RUN dotnet restore "api-grpc-client/api-grpc-client.csproj"
COPY . "api-grpc-client/"
WORKDIR "/src/api-grpc-client"
RUN dotnet build "api-grpc-client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api-grpc-client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api-grpc-client.dll"]
