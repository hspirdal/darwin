FROM microsoft/dotnet:2.1-sdk AS build-env

WORKDIR /src
COPY Services/Darwin.Api services/api
COPY GameServer.Contracts GameServer.Contracts
COPY GameLib GameLib

WORKDIR /src/services/api
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS http://+:80
ENV ASPNETCORE_ENVIRONMENT "Development"
EXPOSE 5000

ENTRYPOINT ["dotnet", "Darwin.Api.dll"]