FROM microsoft/dotnet:2.0.0-sdk AS build-env

WORKDIR /src
COPY TcpGameServer tcpserver

WORKDIR /src/tcpserver
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM microsoft/dotnet:2.0-runtime
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 4445

ENTRYPOINT ["dotnet", "TcpGameServer.dll"]