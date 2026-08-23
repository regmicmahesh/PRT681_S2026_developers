# Week 5 — a UI in front of the API

Backend-only is fine until you want to click something. This week I made a Blazor WebAssembly app that talks to the week 4 API.

## What I built

`ExpenseTrackerClient`

- register / login
- add expense
- list + filter by amount
- delete
- summary (count / total / average)
- token kept in `sessionStorage` so a refresh doesn't kick you out immediately

The client does not have its own database. It just calls `http://localhost:5041`.

## Things I learned

**Blazor WASM vs MVC.** The app is downloaded to the browser and runs C# there (via wasm). That's why the API url is hardcoded in `Program.cs` — the browser is the one making the HTTP calls, not the server.

**CORS again.** If the API isn't listing `http://localhost:5051`, the browser console just says "blocked by CORS" and nothing shows up. The week 4 API already has that origin.

**HttpClient + JWT.** I made `ExpenseApi` slap `Authorization: Bearer …` on every request after login. If the token is missing you get 401 and the pages send you back to login.

**IJSRuntime.** C# can't touch `sessionStorage` directly, so you ask JS. First time I used interop.

## How to run

Terminal 1:

```bash
cd Week_4
dotnet run --project ExpenseTrackerAuth
```

Terminal 2:

```bash
cd Week_5
dotnet run --project ExpenseTrackerClient
```

Open http://localhost:5051 — register, then add a couple of expenses.

## Note on the last few weeks

I was working through the API / auth / blazor stuff locally and was still shaky on git, so I didn't snapshot it as I went. The folders are here now: week 3 is the open API + tests, week 4 is login, week 5 is the UI. Same expense tracker thread from week 1, just not a console menu anymore.
