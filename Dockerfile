#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Smarkets.Client.Outbound/Smarkets.Client.Outbound.csproj", "src/Smarkets.Client.Outbound/"]
COPY ["src/Smarkets.Integration.Application/Smarkets.Integration.Application.csproj", "src/Smarkets.Integration.Application/"]
COPY ["src/Shared/src/Shared.Infra/Shared.Infra.csproj", "src/Shared/src/Shared.Infra/"]
COPY ["src/Shared/src/Shared.Util/Shared.Util.csproj", "src/Shared/src/Shared.Util/"]
COPY ["src/Shared/src/Shared.Domain/Shared.Domain.csproj", "src/Shared/src/Shared.Domain/"]
COPY ["src/Smarkets.Integration.Infra/Smarkets.Integration.Infra.csproj", "src/Smarkets.Integration.Infra/"]
RUN dotnet restore "src/Smarkets.Client.Outbound/Smarkets.Client.Outbound.csproj"
COPY . .
WORKDIR "/src/src/Smarkets.Client.Outbound"
RUN dotnet build "Smarkets.Client.Outbound.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smarkets.Client.Outbound.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Smarkets.Client.Outbound.dll"]