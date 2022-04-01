FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app 
#
# copy csproj and restore as distinct layers
COPY WebSocket.Shared/*.csproj ./WebSoc.Shared/
COPY WebSocket/*.csproj ./WebSoc/
#
WORKDIR /app/WebSoc
RUN dotnet restore 
#
# copy everything else and build app
COPY WebSocket.Shared/. ./WebSoc.Shared/
COPY WebSocket/. ./WebSoc/
#
WORKDIR /app/WebSoc/
RUN dotnet publish -c Release -o out 
#
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
#
COPY --from=build /app/WebSoc/out ./

CMD ASPNETCORE_URLS=http://*:$PORT dotnet WebSocket.dll

