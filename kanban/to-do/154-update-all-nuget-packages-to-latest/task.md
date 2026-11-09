# Update all NuGet packages to latest

## Description

Bump every central package pin in `Directory.Packages.props` (and any `#:package` versions in
runfiles / `.githooks` that pin explicitly) to the latest available version, then make the repo
build and test green on the new versions.

Known drift at creation: `TimeWarp.Nuru` / `TimeWarp.Nuru.DevCli` are pinned at `4.0.0-beta.11`
while `4.0.0-beta.12` exists. Check the rest with `ganda nuget outdated`.

## Requirements

- `ganda nuget outdated` reports every package up to date.
- Major-version bumps are in scope (per global prefs, not an exception); fix any API breaks they cause.
- Keep the TFM split: library and tests on net10.0; dev CLI, runfiles and `.githooks` on net11.0 (Amuru 3).
- Prerelease packages stay on the latest prerelease where already on a prerelease line.
- Do not add or remove packages beyond what an upgrade requires.

## Checklist

- [ ] Run `ganda nuget outdated` and record the before/after list in Results
- [ ] Update pins in `Directory.Packages.props`
- [ ] Update any explicitly versioned `#:package` directives
- [ ] `dotnet build timewarp-flexbox.slnx` clean (no new warnings)
- [ ] `dotnet test timewarp-flexbox.slnx` green
- [ ] `./bin/dev --capabilities` works (dev CLI still builds on new Nuru/Amuru)
- [ ] `ganda repo audit` exit 0 (commit whatever `--fix` changes)
- [ ] Open PR

## Session

- Created: d7943e80-818a-43bd-a36a-6968aed6437a (2026-11-09)

## Notes

Prior bump: task 151 (`kanban/done/151-fix-ganda-repo-audit-and-update-all-nugets-to-latest/`).
TimeWarp packages resolve from nuget.org; `nuget.config` maps only `TimeWarp.Flexbox*` to the GitHub feed.
