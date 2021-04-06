FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY BloodSugarTracking/bin/publish/ ./app
WORKDIR /app
ENTRYPOINT ["dotnet", "BloodSugarTracking.dll"]