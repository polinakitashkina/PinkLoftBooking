# Сборка и запуск для хостингов (Render, Fly.io и т.д.)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY PinkLoftBooking.sln ./
COPY PinkLoftBooking.Api/PinkLoftBooking.Api.csproj PinkLoftBooking.Api/
COPY PinkLoftBooking.Tests/PinkLoftBooking.Tests.csproj PinkLoftBooking.Tests/
RUN dotnet restore PinkLoftBooking.sln
COPY . .
RUN dotnet publish PinkLoftBooking.Api/PinkLoftBooking.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["dotnet", "PinkLoftBooking.Api.dll"]
