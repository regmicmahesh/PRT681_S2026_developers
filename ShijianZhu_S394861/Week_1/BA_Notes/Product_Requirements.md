# Product Requirements — Week 1 Movie Catalogue

## Document control

- Owner: Shijian Zhu (Developer primary; BA secondary)
- Status: Week 1 baseline
- Product: `week1HelloWorldMVC`

## Problem statement

A user needs a simple browser-based way to maintain and find Movie information.
Without the application, records would need to be managed directly in a database
or an unstructured document, making validation and retrieval inconsistent.

## Stakeholders

| Stakeholder | Need |
|---|---|
| Catalogue user | Quickly view, search, add, update, and remove Movie records |
| Developer | Clear, testable requirements and a maintainable MVC structure |
| Unit lecturer/assessor | Evidence of Week 1 MVC, database, CRUD, search, and validation skills |

## Scope

### In scope

- HelloWorld demonstration pages.
- A Movie list stored in SQL Server Express LocalDB.
- Create, list, search, view details, edit, and delete Movie records.
- Movie fields: title, release date, genre, price, and classification rating.
- Server-side and client-side validation.

### Out of scope for Week 1

- User accounts and role-based access.
- Image uploads, reviews, payments, or external movie APIs.
- Production deployment and production database administration.

## Functional requirements

| ID | Requirement | Acceptance criteria |
|---|---|---|
| FR-01 | The system shall list stored Movies. | Opening `/Movies` shows every stored Movie with its main fields. |
| FR-02 | The user shall search by title or genre. | A case-insensitive partial search limits the displayed list; clearing it restores all records. |
| FR-03 | The user shall create a Movie. | Valid data is stored and the user returns to the Movie list. |
| FR-04 | The user shall view one Movie. | Details displays all fields for an existing identifier and returns Not Found for an invalid identifier. |
| FR-05 | The user shall edit a Movie. | Valid changes are saved; invalid values display messages and are not saved. |
| FR-06 | The user shall delete a Movie. | The user sees a confirmation page before the selected record is removed. |
| FR-07 | The system shall validate Movie data. | Title and genre are required; title length, price, date, and rating rules are enforced. |
| FR-08 | The system shall provide a HelloWorld demonstration. | `/HelloWorld` and `/HelloWorld/Welcome` render MVC views using controller data. |

## Business rules

- BR-01: Title must contain 3–60 characters.
- BR-02: Genre is required and cannot exceed 30 characters.
- BR-03: Price must be between 0.01 and 1000.00.
- BR-04: Release date cannot be in the future.
- BR-05: Rating must be one of `G`, `PG`, `PG-13`, `M`, `MA15+`, or `R18+`.
- BR-06: Delete requires an explicit confirmation action.

## Non-functional requirements

- NFR-01: The application shall use ASP.NET Core MVC and Entity Framework Core.
- NFR-02: Local development data shall use SQL Server Express LocalDB.
- NFR-03: Database access shall use asynchronous EF Core operations.
- NFR-04: Forms shall display field-level validation messages.
- NFR-05: Navigation shall expose Home, HelloWorld, and Movies.
- NFR-06: The project shall build successfully with the documented .NET SDK.

## Assumptions and risks

- LocalDB is available only on Windows and may need separate installation.
- The Week 1 application is a learning artefact and is not production hardened.
- Authentication is out of scope, so anyone who can run the local app can change data.
