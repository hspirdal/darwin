FROM microsoft/dotnet:2.1-sdk AS build-env

WORKDIR /src
COPY TcpGameServer.Contracts TcpGameServer.Contracts
COPY GameLib GameLib
COPY WebSocketServer WebSocketServer

WORKDIR /src/WebSocketServer
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 5000

ENTRYPOINT ["dotnet", "WebSocketServer.dll"]