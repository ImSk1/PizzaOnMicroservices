FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 50002
EXPOSE 5202
ENV PORT=50002
ENV GRPC_PORT=5202

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src


# Copy solution file
COPY "PizzaOnMicroservices-All.sln" "PizzaOnMicroservices-All.sln"

# Copy all project files
COPY "Clients/Web/WebMVC/WebMVC/WebMVC.csproj" "Clients/Web/WebMVC/WebMVC/WebMVC.csproj"
COPY "ApiGateways/Web.BFF/Web.BFF/Web.BFF.csproj" "ApiGateways/Web.BFF/Web.BFF/Web.BFF.csproj"
COPY "Services/Identity/Identity.API/Identity.API/Identity.API.csproj" "Services/Identity/Identity.API/Identity.API/Identity.API.csproj"
COPY "Services/Basket/Basket.API/Basket.API/Basket.API.csproj" "Services/Basket/Basket.API/Basket.API/Basket.API.csproj"
COPY "Services/Menu/Menu.API/Menu.API/Menu.API.csproj" "Services/Menu/Menu.API/Menu.API/Menu.API.csproj"

COPY "NuGet.config" "NuGet.config"

# Restore packages
RUN dotnet restore "PizzaOnMicroservices-All.sln"

# Copy all other files
COPY . .

# Publish Basket.API project
WORKDIR /src/Services/Basket/Basket.API/Basket.API
RUN dotnet publish --no-restore --no-dependencies -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Basket.API.dll"]
