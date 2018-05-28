FROM microsoft/aspnetcore-build:2.0.8-2.1.200 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/aspnetcore:2.0.8
WORKDIR /app
COPY --from=build-env /app/out .
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
ENTRYPOINT [ "dotnet", "PFSign.dll" ]