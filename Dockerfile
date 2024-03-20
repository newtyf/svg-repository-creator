FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

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
