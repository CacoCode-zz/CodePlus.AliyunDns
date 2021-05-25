FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR "/DNSR"
COPY ["DNSR/DNSR.csproj", "DNSR/"]

RUN dotnet restore "DNSR/DNSR.csproj"
COPY . .
WORKDIR "/DNSR"
RUN dotnet build "DNSR/DNSR.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DNSR/DNSR.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DNSR.dll"]
