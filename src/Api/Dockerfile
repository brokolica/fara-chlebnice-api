FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY bin/Release/net6.0/ .
CMD dotnet Api.dll --urls "http://*:8080"
