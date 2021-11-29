# Blood sugar tracking app

[![CI CD](https://github.com/Arnab-Developer/BloodSugarTracking/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/Arnab-Developer/BloodSugarTracking/actions/workflows/ci-cd.yml)
![Docker Image Version (latest by date)](https://img.shields.io/docker/v/45862391/bloodsugartracking)

This web app is to track blood sugar data for fasting and PP. In the first
step, one or more users needs to be added, after that blood sugar data 
(fasting and PP) for those users can be added. The app shows the time 
difference between last meal and test time. If the data is higher than normal
then it shows that data as red. The normal range of blood sugar is mentioned 
in the appsettings.json file.

```json
{
  "FastingNormal": 100,
  "TwoHoursNormal": 140,
}
```

## Screenshots

Home page

![Home page](https://github.com/Arnab-Developer/BloodSugarTracking/blob/main/Assets/HomePage.jpg)

Users

![Users](https://github.com/Arnab-Developer/BloodSugarTracking/blob/main/Assets/Users.jpg)

Add blood sugar data

![Add blood sugar data](https://github.com/Arnab-Developer/BloodSugarTracking/blob/main/Assets/AddBloodSugar.jpg)

View blood sugar data

![View blood sugar data](https://github.com/Arnab-Developer/BloodSugarTracking/blob/main/Assets/BloodSugarData.jpg)

## Tech stack

It is an ASP.NET 6 MVC app with Entity Framework for data access with 
SQL Server database. xUnit has been used for unit testing.

## Docker image

This app is in a docker image and stored in dockerhub.

https://hub.docker.com/r/45862391/bloodsugartracking

## Hosting

When a new release is created then this app is stored inside a docker image and push to 
docker hub through CI CD. Hosting in Azure Web App is not done as a part of CI CD. That 
needs to be done manually.

- Create a new resource group in Azure.
- In that resource group create the following resources
  - App Service Plan for Linux
  - Web App with docker hub image `45862391:[use the latest tag]`
  - Create a SQL Server and database
- Update the connection string with the database details in Web App configuration
- In your local machine open terminal and navigate to the project folder and execute below 
command. Copy the generated sql script and execute in the database.

```
dotnet ef migrations script
```

Open the Web App URL in web browser and you should access the app.

## How to run

Create the databases with the below script before run this app.

```sql
USE [master]
GO

CREATE DATABASE [BloodSugarDb]
ON PRIMARY
( 
    NAME = N'BloodSugarDb', 
    FILENAME = N'[local path]\BloodSugarDb.mdf' , 
    SIZE = 8192KB , 
    MAXSIZE = UNLIMITED, 
    FILEGROWTH = 65536KB 
)
LOG ON 
( 
    NAME = N'BloodSugarDb_log', 
    FILENAME = N'[local path]\BloodSugarDb.ldf' , 
    SIZE = 8192KB , 
    MAXSIZE = 2048GB , 
    FILEGROWTH = 65536KB 
)
GO
```

Update the connection string in appsettings.json file if applicable.

Open terminal and navigate to the project folder and execute below command. Copy the generated 
sql script and execute in the database.

```
dotnet ef migrations script
```

Open the solution in Visual Studio and press F5.
