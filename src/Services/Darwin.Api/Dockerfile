FROM microsoft/aspnetcore-build:2.0 AS build-env

WORKDIR /src
COPY Services/Darwin.Api api

WORKDIR /src/api
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Darwin.Api.dll"]