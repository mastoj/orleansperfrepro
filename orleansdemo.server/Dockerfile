FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["orleansdemo.server/*.csproj", "orleansdemo.server/"]
COPY ["orleansdemo.graininterfaces/*.csproj", "orleansdemo.graininterfaces/"]
COPY ["orleansdemo.ServiceDefaults/*.csproj", "orleansdemo.ServiceDefaults/"]

RUN dotnet restore "orleansdemo.server/orleansdemo.server.csproj"

# Copy all source files and build the application
COPY . .
WORKDIR "/src/orleansdemo.server"
RUN dotnet build "orleansdemo.server.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "orleansdemo.server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
# Orleans silo needs port 11111 for silo-to-silo communication
# and port 30000 for client-to-silo communication
EXPOSE 11111
EXPOSE 30000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "orleansdemo.server.dll"]