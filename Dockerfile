
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1809 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1809 AS build
WORKDIR /src
COPY ["Sistema001.Web/Sistema001.Web.csproj", "Sistema001.Web/"]
COPY ["Sistema001.Entidades/Sistema001.Entidades.csproj", "Sistema001.Entidades/"]
COPY ["Sistema001.Datos/Sistema001.Datos.csproj", "Sistema001.Datos/"]
RUN dotnet restore "Sistema001.Web/Sistema001.Web.csproj"
COPY . .
WORKDIR "/src/Sistema001.Web"
RUN dotnet build "Sistema001.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sistema001.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sistema001.Web.dll"]