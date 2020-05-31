FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /app
COPY . .
WORKDIR /app/Gateway
RUN dotnet publish -o out -r linux-x64 --self-contained

FROM mcr.microsoft.com/dotnet/runtime:5.0 as prod
WORKDIR /app
COPY --from=build /app/Gateway/out /app
RUN ls -l
CMD [ "./Gateway"]