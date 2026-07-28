# Solution-Level Configuration Files

This solution follows the standard .NET convention of putting shared, cross-project
configuration in a small set of well-known files at the **solution root** (the same
folder as `todo.slnx`, one level above every project folder). MSBuild and the .NET
CLI auto-discover these files by walking *up* the directory tree from each `.csproj`
— nothing needs to reference them explicitly.

**Where to put them:** solution root, not inside any individual project folder, and
not further up outside the solution (e.g. not in the repo root if the repo contains
multiple unrelated solutions). Auto-discovery works by searching upward from each
project until it finds the file, so placing them any higher would risk a different
`Directory.Build.props`/`Directory.Packages.props` bleeding into unrelated solutions
that happen to live in a parent folder; placing them lower (inside a project folder)
means other projects in the solution won't pick them up at all.

## Files in this solution

### `Directory.Build.props`
General MSBuild **properties** shared by every project in the solution — things like
`TargetFramework`, `Nullable`, `ImplicitUsings`, language version, or analyzer
settings. Imported very early in the build, before the SDK sets its own defaults, so
anything set here behaves like a default that an individual `.csproj` can still
override locally if it needs to.

In this solution it hoists `TargetFramework`, `ImplicitUsings`, and `Nullable` out of
Domain, Application, Infrastructure, Presentation, and WebAPI so they're declared
once instead of five times.

### `Directory.Build.targets`
The sibling of `Directory.Build.props`, using the same auto-discovery, but imported
at the very **end** of the build instead of the start — after the SDK has already
populated its own items and properties. Use it when you need to react to or extend
something the SDK sets up (e.g. appending to an SDK-populated item group, adding a
post-build step). Not present in this solution yet — most small-to-mid solutions
never need one.

### `Directory.Packages.props`
Enables **Central Package Management (CPM)** for NuGet. Sets
`ManagePackageVersionsCentrally=true` and lists every package's version once via
`<PackageVersion Include="..." Version="..." />`. Individual `.csproj` files then
reference packages **without** a version:
`<PackageReference Include="MediatR" />`. This guarantees every project in the
solution uses the exact same version of a given package and makes version bumps a
one-line change instead of a find-and-replace across every `.csproj`.

> Note the file name is `Directory.Packages.props`, distinct from
> `Directory.Build.props` — MSBuild only recognizes each by its exact name. A typo
> here (e.g. `DirectoryPackages.props`) fails silently: the file is just never
> imported, `ManagePackageVersionsCentrally` never gets set, and every
> version-less `PackageReference` in the solution fails to restore with `NU1015`.

### `global.json` *(not yet added)*
Pins the exact .NET SDK version the solution builds with, so every developer
machine and CI agent uses the same SDK regardless of what else is installed
globally. Prevents "works on my machine" issues caused by SDK version drift.

### `.editorconfig` *(not yet added)*
Repo-wide code style and analyzer severity rules enforced by the compiler/IDE —
indentation, naming conventions, `dotnet_diagnostic.*` warning levels. Keeps
formatting and analyzer behavior consistent across every editor/IDE a contributor
might use.

### `nuget.config` *(not yet added)*
Repo-level NuGet settings — which package feeds to use, credentials for private
feeds, whether to fall back to nuget.org. Only needed once the solution consumes
packages from somewhere other than the public NuGet feed.

### `packages.lock.json` *(not yet added)*
Opt-in via `RestorePackagesWithLockFile=true`. Locks the exact resolved version of
every **transitive** dependency (not just the ones you reference directly), so a
`dotnet restore` produces byte-identical dependency graphs across machines and over
time — the NuGet equivalent of npm's `package-lock.json`.

## Recommended baseline for a new clean-architecture solution

```
Directory.Build.props     ← shared TargetFramework / Nullable / LangVersion / analyzers
Directory.Packages.props  ← central NuGet versions (CPM)
.editorconfig             ← style + analyzer rules
global.json                ← SDK version pin
```

All four live at the solution root, next to the `.slnx`/`.sln` file. This is the
combination most .NET teams treat as baseline hygiene for any multi-project
solution — it removes duplicated settings from every `.csproj` and makes the whole
solution build the same way on every machine.
