FROM microsoft/dotnet:2.2-sdk AS build

WORKDIR /app

COPY ./Bot.Irc/*.csproj .
COPY ./Bot.Irc .
COPY ./Bot.Database /Bot.Database

RUN dotnet restore Bot.csproj
RUN dotnet publish Bot.csproj -c Release -o out

FROM microsoft/dotnet:2.2-runtime AS runtime

WORKDIR /app

COPY --from=build /app/out .
COPY --from=build /app/config.json .
COPY --from=build /app/streams.db .

ENTRYPOINT ["/bin/bash"]


