# CareerTrack NT API

Base URL while running locally: `http://127.0.0.1:3000`

## Error shape

```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Check the highlighted fields.",
    "details": {
      "company": "Company is required."
    }
  }
}
```

Unexpected errors never return a stack trace or SQL detail.

## Health

`GET /api/health` → `200`

```json
{ "status": "ok" }
```

## List applications

`GET /api/applications?page=1&pageSize=20&status=Applied&search=engineer`

- `page` defaults to 1.
- `pageSize` defaults to 20 and is capped at 100.
- `status` must match an approved stage when supplied.
- `search` is trimmed, capped at 120 characters and matches company or role.

Response:

```json
{
  "items": [],
  "total": 0,
  "page": 1,
  "pageSize": 20,
  "pageCount": 1
}
```

## Retrieve application

`GET /api/applications/:id` → `200` or `404`

## Create application

`POST /api/applications` with `Content-Type: application/json` → `201`

```json
{
  "company": "Territory Tech",
  "role": "Full-Stack Developer",
  "status": "Applied",
  "applicationDate": "2026-08-19",
  "followUpDate": "2026-08-26",
  "jobUrl": "https://example.com/jobs/123",
  "notes": "Follow up after one week."
}
```

Validation failure returns `422`; malformed JSON returns `400`; wrong media type returns `415`; body over 64 KB returns `413`.

## Update application

`PUT /api/applications/:id` uses the complete representation shown above → `200`, `404` or `422`.

## Delete application

`DELETE /api/applications/:id` → `204` or `404`.

The browser asks for confirmation; the API itself treats an authorised request as intentional. This local POC has no authentication and must not be exposed publicly.

## Summary

`GET /api/summary` → `200`

```json
{
  "total": 3,
  "byStatus": {
    "Wishlist": 0,
    "Applied": 1,
    "Interview": 2,
    "Offer": 0,
    "Rejected": 0,
    "Withdrawn": 0
  }
}
```
