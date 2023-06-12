#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT="Development"
ENV ASPNETCORE_URLS="https://+:443;http://+:80"
ENV DOTNET_USE_POLLING_FILE_WATCHER="1"
#ENV NUGET_PACKAGES="/root/.nuget/fallbackpackages"
#ENV NUGET_FALLBACK_PACKAGES="/root/.nuget/fallbackpackages"
ENV USE_SSL="false"
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="ecc497d8-668b-4d6a-989b-0d933203c75d"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/https/aspnetapp.pfx"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TwitterETL/TwitterETL.csproj", "TwitterETL/"]
#RUN dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p "ecc497d8-668b-4d6a-989b-0d933203c75d"
#RUN dotnet dev-certs https --trust
#RUN dotnet user-secrets -p TwitterETL/TwitterETL.csproj set "Kestrel:Certificates:Development:Password" "ecc497d8-668b-4d6a-989b-0d933203c75d"
RUN dotnet restore "TwitterETL/TwitterETL.csproj"
COPY . .
WORKDIR "/src/TwitterETL"
RUN dotnet build "TwitterETL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TwitterETL.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TwitterETL.dll"]