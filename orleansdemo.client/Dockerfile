FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["orleansdemo.client/*.csproj", "orleansdemo.client/"]
COPY ["orleansdemo.graininterfaces/*.csproj", "orleansdemo.graininterfaces/"]
COPY ["orleansdemo.ServiceDefaults/*.csproj", "orleansdemo.ServiceDefaults/"]

RUN dotnet restore "orleansdemo.client/orleansdemo.client.csproj"

# Copy all source files and build the application
COPY . .
WORKDIR "/src/orleansdemo.client"
RUN dotnet build "orleansdemo.client.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "orleansdemo.client.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "orleansdemo.client.dll"]