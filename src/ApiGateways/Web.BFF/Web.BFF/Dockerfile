FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5038

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

# Publish Web.BFF project
WORKDIR /src/ApiGateways/Web.BFF/Web.BFF
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
ENV PORT=5038
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Web.BFF.dll"]
