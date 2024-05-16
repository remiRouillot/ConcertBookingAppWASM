#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim-arm64v8 AS base
#FROM quay.io/paulchapmanibm/ppc64le/dotnet-70
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM  mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ConcertScannerAppWASM.csproj", "."]
RUN dotnet restore "./ConcertScannerAppWASM.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ConcertScannerAppWASM.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConcertScannerAppWASM.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
#FROM quay.io/paulchapmanibm/ppc64le/dotnet-70
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConcertScannerAppWASM.dll"]