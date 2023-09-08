FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["KafkaConsumer/KafkaConsumer.csproj", "KafkaConsumer/"]
COPY ["CoreLibrary/CoreLibrary.csproj", "CoreLibrary/"]
RUN dotnet restore "KafkaConsumer/KafkaConsumer.csproj"
COPY . .
WORKDIR "/src/KafkaConsumer"
RUN dotnet build "KafkaConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KafkaConsumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN groupadd --gid 15555 kafkaconsumer \ 
    && useradd --uid 15555 --gid 15555 -ms /bin/false kafkaconsumer\
    && chown -R kafkaconsumer:kafkaconsumer /home/kafkaconsumer

USER kafkaconsumer

# https://github.com/dotnet/docs/issues/10217#issuecomment-462323277
ENV COMPlus_EnableDiagnostics=0

ENTRYPOINT ["dotnet", "KafkaConsumer.dll"]