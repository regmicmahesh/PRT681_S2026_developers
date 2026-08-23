# BookStore Authentication and Authorization POC

## Student Information

- Name: Krijal Khadka
- Student ID: S385427
- Unit: PRT681
- Week: Week 5
- Primary role: Developer
- Secondary role: Business Analyst

## Project Overview

This project is a Week 5 proof of concept that extends the previous ASP.NET Core MVC BookStore application.

The POC demonstrates how ASP.NET Core Identity can be used to implement authentication and authorization.

Guests can view Books, Printers and their details. Registered and authenticated users can create, edit and delete records.

## Week 5 Objectives

The project researches and demonstrates:

- Authentication
- Authorization
- Bootstrap theming
- Three-tier architecture
- Data models
- Entity Framework Core
- SQL Server
- Database migrations
- Secure user management

## Technologies

- C#
- .NET 10
- ASP.NET Core MVC
- ASP.NET Core Identity
- Entity Framework Core
- SQL Server Express
- Razor Views
- Bootstrap
- HTML and CSS
- Git and GitHub

## Features

### Public features

Users who are not logged in can:

- View the homepage
- View all Books
- View Book details
- View all Printers
- View Printer details
- Register a new account
- Log in

### Authenticated features

Registered and logged-in users can:

- Create Books
- Edit Books
- Delete Books
- Create Printers
- Edit Printers
- Delete Printers
- Manage their account
- Log out

## Authentication

Authentication verifies the identity of the user.

ASP.NET Core Identity provides:

- Registration
- Login
- Logout
- Password hashing
- Authentication cookies
- User-account storage

User information is stored in the SQL Server `AspNetUsers` table.

## Authorization

Authorization controls which resources a user can access.

The Book and Printer controllers use:

```csharp
[Authorize]