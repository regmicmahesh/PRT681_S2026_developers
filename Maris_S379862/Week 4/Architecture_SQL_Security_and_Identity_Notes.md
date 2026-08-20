# Week 4 — Architecture, SQL, Security and Identity Notes

## 1. Object-oriented programming concepts

- **Class:** a definition combining data and behaviour; an `ApplicationService` class could coordinate validated operations.
- **Object:** a runtime instance of a class with current state.
- **Constructor:** establishes valid dependencies/initial state when an object is created.
- **Encapsulation:** protects internal state behind a deliberate interface.
- **Abstraction:** exposes essential behaviour while hiding implementation detail; a repository hides SQL from application logic.
- **Inheritance:** derives a specialised type from a base type; use sparingly because composition is often clearer.
- **Polymorphism:** different implementations satisfy one contract, such as SQLite and SQL Server repositories.
- **Interface:** defines required behaviour without a concrete implementation, improving substitution and testing.

### Memory management

.NET uses managed memory and garbage collection. Reachable objects remain alive; unreachable managed objects are eventually reclaimed by generations. Garbage collection does not automatically release every unmanaged resource promptly, so disposable resources such as streams, connections and handles require `using`/`IDisposable` patterns. Memory leaks can still occur when references, event handlers or caches keep objects reachable unnecessarily.

Node also uses garbage collection for JavaScript objects, while explicit cleanup is still required for servers, database connections, streams, timers and listeners.

## 2. MVC, MVVM, ORM and application types

| Concept | Purpose |
|---|---|
| MVC | Separates model/data, controller request orchestration and view rendering |
| MVVM | Separates view from view-model state/commands; common in rich client UI frameworks |
| ORM | Maps application objects/queries to relational data; EF Core is the target .NET ORM |
| Razor Pages | Page-focused server-rendered ASP.NET model |
| Web API | HTTP endpoints returning data, commonly JSON |
| Blazor | .NET component UI model that can run server-interactively or through WebAssembly modes |

Web Forms is legacy .NET Framework technology. Modern ASP.NET Core choices should be based on interaction, deployment, team skill, accessibility and operational needs.

## 3. Three-tier architecture

```text
Presentation tier: browser HTML/CSS/JavaScript
       |
Application tier: HTTP routes -> application service -> validation
       |
Data tier: repository -> parameterised SQLite statements
```

Benefits include focused testing, replaceable persistence and reduced coupling. A layer should add a clear responsibility; excessive pass-through layers only add complexity.

## 4. SQL and database design

CareerTrack NT uses one `applications` table in the current scope. It is in first/second/third normal form for its atomic attributes and direct dependency on the record key. New repeating concepts (contacts, interviews, documents) should become related tables rather than comma-separated fields.

### Example queries

```sql
SELECT id, company, role, status, follow_up_date
FROM applications
WHERE status = ?
ORDER BY follow_up_date
LIMIT ? OFFSET ?;

SELECT status, COUNT(*) AS count
FROM applications
GROUP BY status;
```

Placeholders are bound parameters; input is not concatenated into SQL.

### Indexes

- `idx_applications_status` supports status filters/aggregates.
- `idx_applications_follow_up_date` supports follow-up ordering/search patterns.
- Every index increases storage and write work, so new indexes require query evidence.
- Search using `%term%` does not efficiently use a normal leading-key index; larger data may require full-text search or a changed query.

### Joins

- `INNER JOIN`: only matching rows.
- `LEFT JOIN`: every left row plus matching right data.
- `RIGHT/FULL JOIN`: availability/behaviour varies by database; choose from required result semantics.
- `CROSS JOIN`: Cartesian product; useful rarely and dangerous accidentally.

### Transactions and concurrency

A transaction groups related changes atomically. Use it when a business operation spans multiple writes that must all succeed or fail. Concurrency strategy depends on database/workload; optimistic concurrency commonly uses a version/timestamp and rejects stale updates.

### Backup and restore

A copied database file is not automatically a verified backup. Define schedule, retention, encryption, access, restore procedure and recovery objectives. Test restores; a backup that has never restored successfully is only an assumption.

## 5. Security and key concepts

Public/private key cryptography supports identity, signatures and key exchange. The public key may be shared; the private key must be protected. TLS uses certificates and asymmetric operations to establish trust/key material, then efficient symmetric encryption protects the session.

CareerTrack NT controls:

- server-side allowlist/length/date validation;
- parameterised SQL and schema constraints;
- `textContent` rendering rather than untrusted HTML;
- 64 KB request-body and 100-row page-size limits;
- CSP, frame, MIME, referrer and permissions headers;
- generic internal errors and ignored runtime database files.

Current risk: no authentication/authorisation. Therefore it is local-only and unsuitable for real personal data/public hosting.

## 6. Active Directory and identity management

Active Directory Domain Services stores organisation identities, groups, computers and policies. Domain controllers authenticate users, normally using Kerberos in domain environments, and systems use groups/access-control rules for authorisation.

Authentication establishes identity; authorisation decides allowed actions/resources. Identity management includes onboarding, provisioning, role changes, access review, federation, recovery and de-provisioning.

Common ASP.NET project choices can include no authentication, individual accounts, organisation/work accounts and integrated Windows/domain authentication. Exact template labels vary by version. Selection depends on users, deployment, data sensitivity, SSO needs and support responsibility.

## 7. Infrastructure topics

### IIS

Internet Information Services is Microsoft's Windows web server. It can reverse-proxy/host ASP.NET applications, terminate TLS and integrate with Windows administration. Production operation requires app-pool identity, certificates, logs, patching and least privilege.

### Containers

Containers package an application and dependencies while sharing the host kernel. They improve repeatability, but teams still manage images, secrets, networking, persistent data, health checks and vulnerability updates.

### Message queues

A queue decouples producers from consumers for asynchronous work such as email or import processing. Messages require durable delivery decisions, idempotent consumers, retry/backoff, poison-message handling, observability and clear ordering guarantees.

### LDAP queries

LDAP is a protocol for querying/updating directory services. Queries should use least-privilege service identities, safe filter construction and bounded attributes/results. Never build an LDAP filter by concatenating untrusted user input.

## 8. Review questions

1. Which layer owns each validation rule, and why?
2. Which endpoint/query would degrade first as records grow?
3. What identity option fits a local single-user tool versus an enterprise intranet?
4. When would a queue add value rather than unnecessary complexity?
5. How would the SQLite repository be replaced without changing the API contract?
