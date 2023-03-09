FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /App
COPY . .
RUN dotnet restore
RUN dotnet publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "Api.dll" ]