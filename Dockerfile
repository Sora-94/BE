# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official build image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebAPI.Server.csproj", "./"]
RUN dotnet restore "WebAPI.Server.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "WebAPI.Server.csproj" -c Release -o /app/build

# Use the build image to publish the app
FROM build AS publish
RUN dotnet publish "WebAPI.Server.csproj" -c Release -o /app/publish

# Use the runtime image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.Server.dll"]
