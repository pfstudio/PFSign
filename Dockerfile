FROM microsoft/aspnetcore-build AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/aspnetcore
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "PFSign.dll" ]