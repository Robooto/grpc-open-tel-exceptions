FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["grpc-open-tel.csproj", "./"]
RUN dotnet restore "grpc-open-tel.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "grpc-open-tel.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "grpc-open-tel.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "grpc-open-tel.dll"]
