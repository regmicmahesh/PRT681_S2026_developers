# Week 2 — Software Developer Interview Preparation

These are concise practice drafts based on known evidence. Maris should correct details from personal memory and practise aloud rather than memorise the wording.

## 20 behavioural questions with answer drafts

### 1. Tell me about yourself

I am a software-engineering postgraduate with experience in Python automation, Flutter, Angular and workflow applications. At Camera House I built automation that reduced price-tag generation time by about 90%, and at DT Produce I contributed to OCR, timesheet and Angular solutions. I am now completing a full-stack internship and strengthening API, relational-data, validation and automated-testing practice through CareerTrack NT.

### 2. Describe something you built that created value

Camera House used a manual Excel-to-Photoshop workflow for bulk price tags. I analysed the repeated steps and built a Python application to automate generation. The result was an approximately 90% reduction in preparation time.

### 3. Tell me about a difficult technical problem

The DT salary workflow needed cross-platform timesheet capture and direct synchronisation. I built a Flutter application integrated with Google Sheets API, focusing on reliable field mapping and workflow needs. It reduced payroll-processing errors; I would describe exact implementation constraints only from code or records I can verify.

### 4. How do you approach unclear requirements?

I write assumptions and acceptance criteria before coding, identify the riskiest unknown and create the smallest testable slice. For CareerTrack NT, I explicitly excluded authentication and notifications rather than quietly designing them without user/security decisions.

### 5. Describe teamwork

At DT Produce I worked in an Agile team of six contributing Angular 15 modules. I used shared process/requirements understanding to keep implementation aligned and made questions visible instead of solving them privately. The experience taught me that integration quality depends on communication as much as individual code.

### 6. How do you handle code review feedback?

I separate required correctness/security issues from optional style preferences, reproduce concerns where possible and update tests with behaviour changes. I explain the reasoning in the change rather than defending the first implementation. If evidence disproves my approach, I change it.

### 7. Tell me about learning a technology quickly

My work has moved among Python, Flutter, Angular, Google APIs and different workflow domains. I begin with an authoritative tutorial, produce the smallest working example and then apply it to one real requirement. I keep “planned” and “demonstrated” skills separate.

### 8. Describe a mistake or failed assumption

I have learned that a working screen is not proof that the underlying rule is correct. My current practice starts with tests for edge cases and error paths, then implements the smallest behaviour. In a workplace interview I would provide a specific incident only after accurately reconstructing its context and result.

### 9. How do you prioritise technical work?

I prioritise user value, risk and dependency. CareerTrack NT started with validation and persistence, then API operations, then the interface; authentication and notifications remained out of scope. This avoids polishing a UI on top of uncertain behaviour.

### 10. How do you debug a problem?

I reproduce it, identify the failing layer, reduce it to a minimal case and fix the root cause. I add a regression test, run the full suite and verify the original scenario. I avoid making unrelated changes while evidence is unclear.

### 11. How do you manage deadlines?

I define the minimum complete vertical slice and its verification, then communicate what is done, at risk or blocked. I do not call unfinished validation or testing “done” to protect a date because that creates hidden downstream cost.

### 12. Describe communicating technical concepts

At Camera House I translate camera and imaging specifications into practical outcomes for customers. In software work I use the same approach: start with the goal, show a small example and explain the trade-off in plain language.

### 13. How do you protect quality?

I use explicit acceptance criteria, tests, small changes, syntax/build checks and a five-axis review covering correctness, readability, architecture, security and performance. I also document how to run the project so verification is repeatable.

### 14. How do you handle disagreement?

I restate the shared objective, identify the disputed assumption and compare options using evidence such as tests, complexity or operational risk. Once the accountable decision is made, I document it and support the team outcome.

### 15. Tell me about taking initiative

The Camera House automation is a clear example: I identified a repetitive manual workflow and built a targeted Python tool. I connected the technical work to a measurable time reduction rather than adding technology without a business outcome.

### 16. How do you work with legacy or unfamiliar code?

I first learn how to build and test it, locate the relevant behaviour and make the smallest focused change. I preserve existing conventions unless there is evidence they are unsafe or harmful. Tests protect against accidental changes outside the task.

### 17. What motivates you?

I enjoy turning repetitive or confusing work into dependable software. The most satisfying result is not code volume; it is a workflow that becomes faster, clearer or less error-prone and can be explained and maintained by others.

### 18. How do you consider security?

I identify trust boundaries and assets before controls. I validate server input, parameterise queries, avoid rendering untrusted HTML, keep secrets out of Git and return generic errors. I also avoid deploying an unauthenticated personal-data prototype publicly.

### 19. How do you respond when blocked?

I preserve the exact failure, check environment and dependencies, find a safe alternative and make the limitation visible. When .NET was absent, the portfolio used installed Node/SQLite to prove the architecture while keeping the .NET port explicitly pending.

### 20. Why should we hire you?

I combine practical automation and full-stack experience with disciplined testing and communication. I can demonstrate measurable Python improvement, Flutter/API and Angular work, a current internship and a tested end-to-end portfolio. I am honest about gaps and close them through working evidence.

## 20 technical questions and concise answers

1. **What happens in an HTTP request?** A client resolves/connects, sends method/path/headers/body; the server routes, validates and returns status/headers/body.
2. **POST vs PUT?** POST commonly creates under a collection; PUT replaces the representation at a known resource URL and should be idempotent.
3. **400 vs 404 vs 422?** Malformed/invalid request shape or identifier; resource absent; syntactically valid body failing domain validation.
4. **What is REST?** An architectural style using resources, representations, uniform interfaces, stateless communication and cache semantics—not merely JSON.
5. **Why three-tier architecture?** It separates presentation, application rules and data access so each can change/test with less coupling.
6. **What is dependency injection?** Supplying a dependency from outside rather than constructing it internally; this improves substitution and testing.
7. **What is an ORM?** A mapping/query layer between objects and relational storage; it helps productivity but does not replace SQL understanding.
8. **Why parameterise SQL?** It separates code from values, preventing injection and handling escaping/types correctly.
9. **What is an index?** An additional ordered structure that speeds matching/sorting at storage/write cost; choose it from query patterns.
10. **What is normalisation?** Structuring relational data to reduce duplication and update anomalies; denormalise only for measured reasons.
11. **Unit vs integration test?** Unit tests isolate focused logic; integration tests exercise real component boundaries such as HTTP/database.
12. **Explain TDD.** Write a meaningful failing test, implement the minimum passing behaviour, then refactor while tests remain green.
13. **Client vs server validation?** Client validation improves feedback; server validation is the security/data-integrity boundary.
14. **How do you prevent XSS?** Render untrusted values as text/framework-escaped content, use CSP and avoid `innerHTML` with user data.
15. **How do you prevent SQL injection?** Parameterised queries/ORM binding, allowlisted structure and least-privilege database access.
16. **What is async I/O?** Work that yields while waiting for network/file operations so the runtime can handle other tasks.
17. **What is CI/CD?** Automated integration checks and controlled delivery/deployment; tests, builds and security checks create gates.
18. **Container vs virtual machine?** Containers share the host kernel and package processes; VMs include a guest OS and stronger isolation boundary.
19. **Authentication vs authorisation?** Authentication establishes identity; authorisation decides permitted actions/resources.
20. **How would you port CareerTrack NT to ASP.NET Core?** Preserve the API contract, model/validation tests and schema; implement controllers/minimal endpoints, application service, EF Core context/migrations, integration tests and the existing same-origin UI, then compare behaviour before switching.

## Questions to ask the interviewer

- What production outcomes would I own in the first three months?
- How are design decisions, testing and operational responsibility shared?
- What does the review/deployment pipeline enforce today?
- Which parts of the stack are changing, and why?
