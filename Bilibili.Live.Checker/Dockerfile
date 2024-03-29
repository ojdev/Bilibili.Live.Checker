#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

ENV wxpusher_APPTOKEN=

RUN apt-get update && apt-get install -y wget unzip libgdiplus libfontconfig gnupg gnupg2 gnupg1

RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add -

RUN echo "deb http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google-chrome.list
RUN apt-get update && apt-get install -y google-chrome-stable
#RUN wget https://chromedriver.storage.googleapis.com/114.0.5735.90/chromedriver_linux64.zip
RUN wget https://edgedl.me.gvt1.com/edgedl/chrome/chrome-for-testing/121.0.6167.85/win64/chrome-headless-shell-win64.zip
RUN unzip chrome-headless-shell-win64.zip -d /usr/local/bin
RUN rm chrome-headless-shell-win64.zip

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Bilibili.Live.Checker/Bilibili.Live.Checker.csproj", "Bilibili.Live.Checker/"]
RUN dotnet --info
RUN dotnet nuget locals all --clear
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