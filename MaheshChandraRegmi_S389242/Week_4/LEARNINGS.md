# Week 4 — putting a lock on the API

Week 3 API is open. Anyone who can hit the port can add/delete expenses. That's fine for learning CRUD, not fine if two people use it.

This week I took the same expense API and added login.

## What I built

`ExpenseTrackerAuth`

- `POST /api/auth/register`
- `POST /api/auth/login` → JWT
- same expense endpoints as week 3, but they now need `Authorization: Bearer <token>`
- expenses are filtered by `UserId` — you only see your own rows
- CORS opened for the week 5 blazor client

I used `AddIdentityCore` instead of full cookie Identity because this is an API, not a razor app. Password rules are relaxed (6 chars, no uppercase required) so I could actually test it.

## Things that took a while

**JWT vs cookies.** Cookie auth wants redirects. APIs want 401. JwtBearer is the one that reads the Authorization header.

**Claims.** The user id has to go into the token (`ClaimTypes.NameIdentifier` / `sub`) or the expense controller has no idea who is calling.

**Swagger authorize button.** Without the Bearer security definition you keep pasting tokens into headers by hand. Once swagger knows about bearer you can login, copy the token, click Authorize, and the rest of the requests just work.

**CORS.** I hit this while poking at a local html file. Browser blocks the API unless the API lists the frontend origin. Added it now so week 5 is not a surprise.

## How to run

```bash
cd Week_4
dotnet run --project ExpenseTrackerAuth
```

Swagger: http://localhost:5041/swagger

Register → copy `token` → Authorize → hit `/api/expenses`.
