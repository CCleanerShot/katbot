FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS stage1
WORKDIR /app
COPY ./websocket-skyblock.csproj .
RUN dotnet restore "websocket-skyblock.csproj"
COPY . .
RUN dotnet publish "websocket-skyblock.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine
USER 1000:1000
WORKDIR /app
COPY --from=stage1 --chown=1000:1000 /publish .
ENTRYPOINT [ "dotnet", "websocket-skyblock.dll" ]