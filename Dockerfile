FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install gnupg wget -y && \
  wget --quiet --output-document=- https://dl-ssl.google.com/linux/linux_signing_key.pub | gpg --dearmor > /etc/apt/trusted.gpg.d/google-archive.gpg && \
  sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list' && \
  apt-get update && \
  apt-get install google-chrome-stable -y --no-install-recommends && \
  rm -rf /var/lib/apt/lists/*

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
COPY --from=build /src/template.html .
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmbedRepoGithub.dll"]
