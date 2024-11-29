FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LunaEdge.TestAssignment.WebApi/LunaEdge.TestAssignment.WebApi.csproj", "LunaEdge.TestAssignment.WebApi/"]
COPY ["LunaEdge.TestAssignment.Application/LunaEdge.TestAssignment.Application.csproj", "LunaEdge.TestAssignment.Application/"]
COPY ["LunaEdge.TestAssignment.Domain/LunaEdge.TestAssignment.Domain.csproj", "LunaEdge.TestAssignment.Domain/"]
COPY ["LunaEdge.TestAssignment.ServiceDefaults/LunaEdge.TestAssignment.ServiceDefaults.csproj", "LunaEdge.TestAssignment.ServiceDefaults/"]
RUN dotnet restore "LunaEdge.TestAssignment.WebApi/LunaEdge.TestAssignment.WebApi.csproj"
COPY . .
WORKDIR "/src/LunaEdge.TestAssignment.WebApi"
RUN dotnet build "LunaEdge.TestAssignment.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LunaEdge.TestAssignment.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LunaEdge.TestAssignment.WebApi.dll"]
