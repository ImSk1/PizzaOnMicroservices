#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.



FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY Menu.API.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 50001
EXPOSE 5101
ENV GRPC_PORT=5101
ENV PORT=50001
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Menu.API.dll"]
