FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5029

ENV ASPNETCORE_URLS=http://+:5029

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["CommandApi/src/CommandApi/CommandApi.csproj", "CommandApi/src/CommandApi/"]
RUN dotnet restore "CommandApi/src/CommandApi/CommandApi.csproj"
COPY . .
WORKDIR "/src/CommandApi/src/CommandApi"
RUN dotnet build "CommandApi.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "CommandApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommandApi.dll"]
