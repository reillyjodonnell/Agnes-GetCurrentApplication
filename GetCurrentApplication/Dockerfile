#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GetCurrentApplication/GetCurrentApplication.csproj", "GetCurrentApplication/"]
RUN dotnet restore "GetCurrentApplication/GetCurrentApplication.csproj"
COPY . .
WORKDIR "/src/GetCurrentApplication"
RUN dotnet build "GetCurrentApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GetCurrentApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GetCurrentApplication.dll"]