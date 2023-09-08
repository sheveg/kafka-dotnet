FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["KafkaProducer/KafkaProducer.csproj", "KafkaProducer/"]
COPY ["CoreLibrary/CoreLibrary.csproj", "CoreLibrary/"]
RUN dotnet restore "KafkaProducer/KafkaProducer.csproj"
COPY . .
WORKDIR "/src/KafkaProducer"
RUN dotnet build "KafkaProducer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KafkaProducer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN groupadd --gid 15555 kafkaproducer \ 
    && useradd --uid 15555 --gid 15555 -ms /bin/false kafkaproducer\
    && chown -R kafkaproducer:kafkaproducer /home/kafkaproducer

USER kafkaproducer

# https://github.com/dotnet/docs/issues/10217#issuecomment-462323277
ENV COMPlus_EnableDiagnostics=0

ENTRYPOINT ["dotnet", "KafkaProducer.dll"]