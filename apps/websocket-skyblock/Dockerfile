FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS stage1
WORKDIR /app
COPY . .
ARG $CACHEBUST
RUN cd ~
RUN ls -la
RUN cd /
RUN ls -la


# COPY certs .
# # RUN dotnet restore "websocket-skyblock.csproj"
# # RUN dotnet publish "websocket-skyblock.csproj" -c Release -o /publish

# # FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine
# # ARG $CACHEBUST
# # USER 1000:1000
# # WORKDIR /app
# # COPY --from=stage1 --chown=1000:1000 /publish .
# # COPY --from=stage1 --chown=1000:1000 /app/katbot.org.crt .
# # RUN apt-get update && apt-get install -y ca-certificates
# # COPY ./katbot.org.crt /usr/local/share/ca-certificates/
# # RUN update-ca-certificates
# # ENTRYPOINT [ "dotnet", "websocket-skyblock.dll" ]