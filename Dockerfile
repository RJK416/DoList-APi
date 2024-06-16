# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FinalToDoAPI.csproj", "./"]
RUN dotnet restore "./FinalToDoAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "FinalToDoAPI.csproj" -c Release -o /app/build

# Publish the application to the /app/publish directory
FROM build AS publish
RUN dotnet publish "FinalToDoAPI.csproj" -c Release -o /app/publish

# Use the base image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinalToDoAPI.dll"]
