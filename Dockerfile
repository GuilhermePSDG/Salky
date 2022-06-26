FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV ASPNETCORE_ENVIRONMENT Development
WORKDIR /app 
#
# copy csproj and restore as distinct layers
COPY Salky.API/*.csproj ./SalkyAPI/
COPY Salky.App/*.csproj ./Salky.App/
COPY Salky.Domain/*.csproj ./Salky.Domain/
COPY Salky.WebSocket/*.csproj ./Salky.WebSocket/
#
WORKDIR /app/SalkyAPI
RUN dotnet restore
#
# copy everything else and build app

COPY Salky.API/. ./SalkyAPI/
COPY Salky.App/. ./Salky.App/
COPY Salky.Domain/. ./Salky.Domain/
COPY Salky.WebSocket/. ./Salky.WebSocket/
#   
WORKDIR /app/SalkyAPI
RUN dotnet publish -c Release -o out 
#
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
#
COPY --from=build /app/SalkyAPI/out ./

CMD ASPNETCORE_URLS=http://*:$PORT dotnet Salky.API.dll

