FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS stage1
WORKDIR /app
COPY ./discord-bot.csproj .
RUN dotnet restore "discord-bot.csproj"
COPY . .
RUN dotnet publish "discord-bot.csproj" -c Release -o /publish
COPY /assets /assets

FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine
# https://github.com/discord-net/Discord.Net/issues/2704 annoying
RUN apk add --no-cache icu-libs
USER 1000:1000
WORKDIR /app
COPY --from=stage1 --chown=1000:1000 /app/assets ./assets
COPY --from=stage1 --chown=1000:1000 /publish .
ENTRYPOINT [ "dotnet", "discord-bot.dll" ]