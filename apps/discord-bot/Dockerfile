FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS stage1
WORKDIR /app
COPY ./skyblock-bot.csproj .
RUN dotnet restore "skyblock-bot.csproj"
COPY . .
RUN dotnet publish "skyblock-bot.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine
USER 1000:1000
WORKDIR /app
COPY --from=stage1 --chown=1000:1000 /publish .
COPY --from=stage1 --chown=1000:1000 /assets .
ENTRYPOINT [ "dotnet", "skyblock-bot.dll" ]