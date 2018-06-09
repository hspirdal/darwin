FROM microsoft/dotnet:2.0.0-sdk AS build-env

WORKDIR /src
COPY TcpGameServer.Contracts TcpGameServer.Contracts
COPY TcpGameServer TcpGameServer

WORKDIR /src/TcpGameServer
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM microsoft/dotnet:2.0-runtime
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 4445

ENTRYPOINT ["dotnet", "TcpGameServer.dll"]