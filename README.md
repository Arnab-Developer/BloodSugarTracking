# Blood sugar tracking app

This web app is to track blood sugar data for 'fasting' and 'PP'.

- Register with email and password.
- Login with the email and password.
- Create an user for which you want to enter blood sugar data.
- Add blood sugar data for that user.

The app shows the time difference between last meal and test time. If the data is higher than normal
then it shows that data as red. The normal range of blood sugar is mentioned in the appsettings.json file.

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

It is an ASP.NET 6 MVC app with Entity Framework for data access with SQL Server database. xUnit has been used for 
unit testing. ASP.NET Identity has been used for authentication.

## Docker image

This app is in a docker image and stored in dockerhub.

https://hub.docker.com/r/45862391/bloodsugartracking

## How to run from local machine

- Clone the repo
- Create two databases (one for the application and another for identity) with the below script before run this app.

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

CREATE DATABASE [BloodSugarIdentityDb]
ON PRIMARY
( 
    NAME = N'BloodSugarIdentityDb', 
    FILENAME = N'[local path]\BloodSugarIdentityDb.mdf' , 
    SIZE = 8192KB , 
    MAXSIZE = UNLIMITED, 
    FILEGROWTH = 65536KB 
)
LOG ON 
( 
    NAME = N'BloodSugarIdentityDb_log', 
    FILENAME = N'[local path]\BloodSugarIdentityDb.ldf' , 
    SIZE = 8192KB , 
    MAXSIZE = 2048GB , 
    FILEGROWTH = 65536KB 
)
GO
```

- Update the connection strings in appsettings.json file if applicable.

- Open terminal and navigate to the project folder and execute below command. Copy the generated 
sql script and execute in the `BloodSugarDb` database.

```
dotnet ef migrations script -c BloodSugarContext
```

- Now execute below command and copy the generated sql script and execute in the `BloodSugarIdentityDb` database.

```
dotnet ef migrations script -c ApplicationDbContext
```

Open the solution in Visual Studio and press F5.

## Hosting

When a new release is created then this app is stored inside a docker image and push to docker hub through GitHub action. 
Hosting in Azure Web App is not done as a part of GitHub action. That needs to be done manually.

- Create a new resource group in Azure.
- In that resource group create the following resources
  - App Service Plan for Linux
  - Web App with docker hub image `45862391/bloodsugartracking:[use the latest tag]`
  - Create two SQL Servers and databases (one for the application and another for identity)
- Update the connection strings with the database details in Web App configuration
- Clone the repo in your local machine
- Open terminal and navigate to the project folder and execute below command. Copy the generated 
sql script and execute in the application database in Azure.

```
dotnet ef migrations script -c BloodSugarContext
```

- Now execute below command and copy the generated sql script and execute in the identity database in Azure.

```
dotnet ef migrations script -c ApplicationDbContext
```

Open the Web App URL in web browser and you should access the app.
