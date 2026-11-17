# Bump coverlet.collector to 10.1.1

## Description

`ganda nuget outdated` reports one outdated package: `coverlet.collector` 10.1.0 → 10.1.1
(patch, dev-only, used by the test project). All other packages are current.

## Checklist

- [ ] Update `coverlet.collector` to `10.1.1` in `Directory.Packages.props`
- [ ] `./bin/dev build` and `./bin/dev test` pass
- [ ] `ganda nuget outdated` reports no outdated packages

## Session

- Created: 344df8f6-db0a-4171-88ee-c067d2de3f47 (2026-11-17)

## Notes

- Only reference: `Directory.Packages.props:19` (central package management); consumed by
  `test/timewarp-flexbox-tests/timewarp-flexbox-tests.csproj`.
- Patch bump of a dev-only package — no product version change needed.
