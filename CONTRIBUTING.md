# Contributing

Contributions are welcome. Please create a new issue or pick an existing issue to work on. Let me 
know on which issue you are going to work before raising a pull request.

You need Visual Studio 2019, ASP.NET 5, EF and SQL Server 2016 to work on this project locally.

- If you find an existing issue then comment on that to let me know. Or if you want to work
on something else then create a new issue to let me know the details.
- If your issue is approved then fork the repo and create a new branch from `main`.
- Clone the forked repo and set the upstream to the original repo `main` branch.
- Create the databases with the below script.

```sql
USE [master]
GO

CREATE DATABASE [BloodSugarDb]
ON PRIMARY
( 
    NAME = N'BloodSugarDb', 
    FILENAME = N'D:\chowba\BloodSugarDb.mdf' , 
    SIZE = 8192KB , 
    MAXSIZE = UNLIMITED, 
    FILEGROWTH = 65536KB 
)
LOG ON 
( 
    NAME = N'BloodSugarDb_log', 
    FILENAME = N'D:\chowba\BloodSugarDb.ldf' , 
    SIZE = 8192KB , 
    MAXSIZE = 2048GB , 
    FILEGROWTH = 65536KB 
)
GO
```

- Open terminal and navigate to the project folder and execute below command.
Copy the generated sql script and execute in the database.

```
dotnet ef migrations script
```

- Open the solution in Visual Studio 2019 and start working.
- Work on the issue and commit the code on the new branch.
- Create a pull request from the new branch to upstream `main` branch.
- Your pull request will be reviewed.
- After the review, if everything is fine then it will be merged.

> Note: pull request can be rejected if the project don't need the proposed changes anymore.