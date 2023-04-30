FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5001
ENV PORT=5001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy solution file
COPY "PizzaOnMicroservices-All.sln" "PizzaOnMicroservices-All.sln"
COPY "NuGet.config" "NuGet.config"

# Copy all project files
COPY "Services/Identity/Identity.API/Identity.API/Identity.API.csproj" "Services/Identity/Identity.API/Identity.API/Identity.API.csproj"
COPY . .

# Restore packages
RUN dotnet restore "Services/Identity/Identity.API/Identity.API/Identity.API.csproj"



# Publish Identity.API project
WORKDIR /src/Services/Identity/Identity.API/Identity.API
RUN dotnet build "Identity.API.csproj" -c Release -o /app/build
RUN dotnet publish --no-restore --no-dependencies -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Identity.API.dll"]
