These are the commands to add EFCore SQLServer and Design.

```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer // required to comms with sql server
dotnet add package Microsoft.EntityFrameworkCore.Design // required for dotnet-ef tool to work, migrations, reversing and stuffs
```

Tool for migrations.

```
dotnet tool install --global dotnet-ef
```
