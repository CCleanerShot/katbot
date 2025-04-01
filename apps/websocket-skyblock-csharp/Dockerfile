FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS stage1
WORKDIR /app
COPY ./websocket-skyblock-csharp.csproj .
RUN dotnet restore "websocket-skyblock-csharp.csproj"
COPY . .
RUN dotnet publish "websocket-skyblock-csharp.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine
USER 1000:1000
WORKDIR /app
COPY --from=stage1 --chown=1000:1000 /publish .
COPY --from=stage1 --chown=1000:1000 /assets .
ENTRYPOINT [ "dotnet", "websocket-skyblock-csharp.dll" ]