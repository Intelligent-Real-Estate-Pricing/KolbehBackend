# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
# حذف خطوط مربوط به ایجاد پوشه logs و تغییر کاربر
# RUN mkdir -p /app/logs && chmod 777 /app/logs
# USER app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["MyApi/Kolbeh.Api.csproj", "MyApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["WebFramework/WebFramework.csproj", "WebFramework/"]
RUN dotnet restore "./MyApi/Kolbeh.Api.csproj"
COPY . .
WORKDIR "/src/MyApi"
RUN dotnet build "./Kolbeh.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Kolbeh.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kolbeh.Api.dll"]