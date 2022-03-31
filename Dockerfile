FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:5.0-alpine AS initial

COPY test-api /src

FROM initial AS build

WORKDIR /src

RUN dotnet restore ./test-api.csproj

FROM build AS publish

RUN dotnet build test-api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final

WORKDIR /app

COPY --from=publish /app .

RUN apt update && apt install -y curl

RUN apt update \
    && apt install -y apt-transport-https dirmngr gnupg ca-certificates #\
    && apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys MAGICKEY

RUN apt update \
    && apt install -y \
        libc6-dev \
        libgdiplus \
        mono-devel

RUN apt-get autoclean -y
RUN apt-get install fontconfig \
    && fc-cache -f -v

ENTRYPOINT ["dotnet", "test-api.dll"]

