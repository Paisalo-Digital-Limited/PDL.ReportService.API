# ----------- Build Stage -----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy everything and restore
COPY . .
RUN dotnet restore PDL.ReportService.API/PDL.ReportService.API.csproj

# Publish
RUN dotnet publish PDL.ReportService.API/PDL.ReportService.API.csproj \
    -c Release -o /app/publish --no-restore -r linux-x64 --self-contained false

# ----------- Runtime Stage -----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PDL.ReportService.API.dll"]
