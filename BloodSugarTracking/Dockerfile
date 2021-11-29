FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY BloodSugarTracking/bin/publish/ ./app
WORKDIR /app
ENTRYPOINT ["dotnet", "BloodSugarTracking.dll"]