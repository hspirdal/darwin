FROM microsoft/dotnet:2.0.0-sdk AS build-env

WORKDIR /src
COPY TcpGameClient tcpclient

WORKDIR /src/tcpclient
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM microsoft/dotnet:2.0-runtime
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "TcpGameClient.dll"]