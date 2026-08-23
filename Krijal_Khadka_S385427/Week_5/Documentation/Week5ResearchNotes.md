# Week 5 Research Notes

## Project

BookStore Authentication and Authorization Proof of Concept

## Student

Krijal Khadka — S385427

---

## 1. Authentication

Authentication is the process of verifying the identity of a user. In the BookStore application, ASP.NET Core Identity allows users to register, log in and log out securely.

Identity stores user information in the `AspNetUsers` table. Passwords are not stored as plain text; they are converted into secure password hashes.

After a successful login, ASP.NET Core creates an authentication cookie. This cookie allows the application to recognise the user during later requests.

### Authentication implemented in the POC

- User registration
- User login
- User logout
- Secure password hashing
- Authentication cookies
- Displaying the logged-in username
- Conditional navigation controls

---

## 2. Authorization

Authorization determines which application functions a user is permitted to access. It occurs after authentication has established the identity of the user.

The `[Authorize]` attribute protects controllers or actions from anonymous users. The `[AllowAnonymous]` attribute allows selected actions to remain publicly accessible.

In the BookStore POC, guests can view Book and Printer records. Only authenticated users can create, edit and delete records.

### Access-control rules

| Function | Guest | Authenticated user |
|---|---|---|
| View Books | Allowed | Allowed |
| View Book Details | Allowed | Allowed |
| Create Book | Denied | Allowed |
| Edit Book | Denied | Allowed |
| Delete Book | Denied | Allowed |
| View Printers | Allowed | Allowed |
| View Printer Details | Allowed | Allowed |
| Create Printer | Denied | Allowed |
| Edit Printer | Denied | Allowed |
| Delete Printer | Denied | Allowed |

---

## 3. Bootstrap Theme

Bootstrap is a frontend framework that provides responsive components and utility classes. It helps developers build interfaces that adapt to desktop, tablet and mobile screens.

The BookStore application uses Bootstrap navigation, buttons, cards, alerts, forms and tables. Bootstrap classes provide consistent spacing, colours and responsive behaviour.

The interface also changes according to authentication status. Guests see Login and Register controls, while authenticated users see their username and Logout.

### Bootstrap components used

- Responsive navigation bar
- Buttons
- Cards
- Alerts
- Forms
- Tables
- Responsive grid
- Spacing utilities
- Text and background colours

---

## 4. Three-Tier Architecture

Three-tier architecture separates an application into presentation, business-logic and data-access responsibilities. This separation makes the application easier to understand, test and maintain.

The presentation layer displays Razor Views and receives user input. The application layer uses MVC controllers to process requests and apply access-control rules.

The data layer uses Entity Framework Core and SQL Server to store Books, Printers, Scanners and Identity user information.

### BookStore architecture

#### Presentation layer

- Razor Views
- Bootstrap user interface
- Login and registration interface
- Forms, tables, alerts and navigation

#### Application and business-logic layer

- HomeController
- BooksController
- PrintersController
- Authentication configuration
- Authorization attributes
- Model validation

#### Data-access layer

- EfBookStoreContext
- Entity Framework Core
- SQL Server Express
- Database migrations
- DbInitializer
- Book, Printer and Scanner models
- ASP.NET Core Identity tables

### Request flow

```text
User
→ Razor View
→ MVC Controller
→ Entity Framework Core
→ SQL Server
→ MVC Controller
→ Razor View
→ User