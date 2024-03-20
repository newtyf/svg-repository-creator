FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get -f install && apt-get -y install wget gnupg2 apt-utils
RUN wget -q -O - https://dl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN echo 'deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main' >> /etc/apt/sources.list
RUN apt-get update \
&& apt-get install -y google-chrome-stable --no-install-recommends --allow-downgrades fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf

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
