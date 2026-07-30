# Automated Test Results

## Command

Run from `ShijianZhu_S394861/Week_1`:

```powershell
dotnet test ShijianZhu.Week1.slnx --configuration Release --no-restore
```

## Result

```text
Passed! - Failed: 0, Passed: 9, Skipped: 0, Total: 9
```

The solution build completed with zero warnings and zero errors.

## Validation coverage

`MovieValidationTests` verifies:

1. A complete valid Movie passes validation.
2. A missing title is rejected.
3. A title shorter than three characters is rejected.
4. A future release date is rejected.
5. An unsupported Rating is rejected.
6. A price outside the allowed range is rejected.

## Controller coverage

`MoviesControllerTests` uses a separate EF Core InMemory database for each test
and verifies:

1. Text search returns a matching Movie.
2. Rating filtering returns the correct Movie.
3. Text and Rating filters can be combined.

## Git evidence

The final verification commit brings the personal directory history to ten
meaningful commits. No empty commits or fabricated meeting records were used.

Verify the count from the repository root:

```powershell
git log --oneline --all -- ShijianZhu_S394861/
```
