# CareerTrack NT — Four-Week Implementation Plan

## Architecture decisions

- Use a coherent Developer proof-of-concept so each technical acceptance criterion can be traced to code and tests.
- Use Node 25 and built-in SQLite because they are installed and executable in the current environment; document the intended .NET learning path separately.
- Build vertical slices in dependency order: create, list/filter, update, delete, summary.
- Keep the prototype local and unauthenticated; do not store sensitive documents or credentials.

## Week 1 — foundations and first APIs

### Task 1: Establish specification and repository evidence

Acceptance criteria:

- The objective, architecture, commands, validation, tests and boundaries are documented.
- Work that requires human participation is clearly marked.

Verification: review `PROJECT_SPEC.md`, this Developer-folder README and `CareerTrackNT/README.md`.

### Task 2: Create and validate an application

Dependencies: Task 1.

Acceptance criteria:

- Invalid applications return field-specific errors.
- A valid application can be stored and retrieved.
- Unit tests prove normalisation and validation behaviour.

Verification: targeted domain tests, then the full test command.

### Task 3: List and filter applications

Dependencies: Task 2.

Acceptance criteria:

- Results can be searched and filtered by status.
- Pagination is bounded and returns total/page metadata.
- SQL uses parameters.

Verification: repository/API integration tests.

### Checkpoint

- Application remains executable.
- Tests and syntax checks pass.
- Week 1 Developer notes describe actual environment results.

## Week 2 — connected front end and three API operations

### Task 4: Add update behaviour

Dependencies: Task 2.

Acceptance criteria: valid updates persist; missing IDs return 404; invalid updates return 422.

### Task 5: Add delete behaviour

Dependencies: Task 2.

Acceptance criteria: existing records can be deleted; missing IDs return 404; UI requires confirmation.

### Task 6: Connect accessible browser UI

Dependencies: Tasks 3–5.

Acceptance criteria:

- The interface provides create, filter, edit and delete flows.
- Loading, empty, success and error states are announced.
- It is usable by keyboard at mobile and desktop widths.

### Checkpoint

- At least three API operations are consumed by the front end.
- API contract and manual verification steps are documented.

## Week 3 — themed POC, validation and quality

### Task 7: Add summary reporting

Dependencies: Task 3.

Acceptance criteria: summary counts are returned by status and displayed without extra per-row queries.

### Task 8: Polish and test the CareerTrack NT theme

Dependencies: Task 6.

Acceptance criteria: semantic, responsive interface; visible focus; no colour-only status meaning; realistic content.

### Task 9: Complete traceability and UAT evidence

Dependencies: Tasks 4–8.

Acceptance criteria: each implemented user story maps to requirement, endpoint, UI and test case.

## Week 4 — architecture, SQL, security and handover

### Task 10: Document three-tier architecture and SQL

Acceptance criteria: notes cover OOP, MVC/MVVM/ORM, indexes, normalisation, joins, transactions and the current architecture.

### Task 11: Document identity and operational concepts

Acceptance criteria: notes distinguish authentication/authorisation and cover Active Directory, identity options, IIS, containers and message queues without claiming implementation.

### Task 12: Final quality review

Acceptance criteria:

- All tests and checks pass.
- Correctness, readability, architecture, security and performance are reviewed.
- Generated data, secrets and personal contact details are absent.
- Remaining human/group actions are listed.

## Risks and mitigations

| Risk | Impact | Mitigation |
|---|---|---|
| Group chooses another product | Individual POC may differ from shared codebase | Keep it labelled as an individual POC and adapt only after group confirmation |
| .NET SDK unavailable | Exact lecturer technology cannot be executed here | Deliver a portable API design; record .NET/EF Core port as an honest backlog task |
| Four weeks compressed into one repository update | Could look like fabricated weekly activity | Use artefact folders as curriculum organisation, not invented chronology or timesheets |
| Personal job data enters Git | Privacy exposure | Use fictional seed/example data only; ignore runtime database |
| Unauthenticated app is deployed | Unauthorised access | Keep local-only and state the boundary prominently |
