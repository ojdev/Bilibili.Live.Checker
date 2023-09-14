#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

ENV wxpusher_APPTOKEN=

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bilibili.Live.Checker/Bilibili.Live.Checker.csproj", "Bilibili.Live.Checker/"]
RUN dotnet restore "Bilibili.Live.Checker/Bilibili.Live.Checker.csproj"
COPY . .
WORKDIR "/src/Bilibili.Live.Checker"
RUN dotnet build "Bilibili.Live.Checker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bilibili.Live.Checker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bilibili.Live.Checker.dll"]