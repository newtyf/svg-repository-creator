FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install -y \
    libglib2.0-0 \
    libnss3 \
    libgconf-2-4 \
    libasound2 \
    libatk1.0-0 \
    libgtk-3-0 \
    libgbm-dev \
    libx11-xcb1 \
    libxcomposite1 \
    libxcursor1 \
    libxdamage1 \
    libxi6 \
    libxtst6 \
    libxrandr2 \
    libxss1 \
    libxkbcommon0 \
    libxslt1.1 \
    libxkbfile1 \
    libevent-2.1-6 \
    libfontconfig1 \
    libpango-1.0-0 \
    libcairo2 \
    libatspi2.0-0 \
    libuuid1 \
    libgdk-pixbuf2.0-0 \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EmbedRepoGithub.csproj", "./"]
RUN dotnet restore "EmbedRepoGithub.csproj"
COPY . .
WORKDIR "/src/"

RUN dotnet build "EmbedRepoGithub.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "EmbedRepoGithub.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

ARG TOKEN
ENV OAUTH_TOKEN_GITHUB = $TOKEN

RUN mkdir -p "storage"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmbedRepoGithub.dll"]
