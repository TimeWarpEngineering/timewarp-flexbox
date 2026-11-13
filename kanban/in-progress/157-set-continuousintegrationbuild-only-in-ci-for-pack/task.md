# Set ContinuousIntegrationBuild only in CI for pack

## Description

`dev workflow` pack currently passes `-p:ContinuousIntegrationBuild=true` unconditionally, which is right for CI packaging but not for a local or agent run.

Call site: `tools/dev-cli/endpoints/workflow-command.cs:155` (`dotnet pack source/timewarp-flexbox/timewarp-flexbox.csproj -c Release -o artifacts/packages -p:ContinuousIntegrationBuild=true`).

Use the same gate as Mediator's `Directory.Build.props:8-9`: set `ContinuousIntegrationBuild` only when `GITHUB_ACTIONS` (or an equivalent `CI` env) is true.

## Requirements

- Local pack / `dev workflow` must not force `ContinuousIntegrationBuild=true`.
- CI packs still set it so official packages keep CI deterministic / SourceLink semantics.
- Prefer an MSBuild condition over a hard-coded local `-p:` if that preserves CI behavior.

## Checklist

- [x] Gate or move the flag at `workflow-command.cs:155`
- [x] Confirm CI packaging still sets `ContinuousIntegrationBuild`
- [x] Confirm a local pack/workflow does not disable incrementality via this property

## Acceptance criteria

- A local `dev workflow` pack no longer passes `ContinuousIntegrationBuild=true` unless it is actually CI.
- CI-produced packages still set the property.

## Notes

- Sibling: timewarp-terminal 040 (`workflow.cs:124`). Pattern: Mediator `Directory.Build.props:8-9`.
- Filed by Jenzi from cross-repo build-telemetry audit 2026-11-12.

## Goal (implementer restatement)

Stop `dev workflow` from forcing `ContinuousIntegrationBuild=true` on every pack. Move the
setting into `Directory.Build.props`, gated on `GITHUB_ACTIONS == true` (the Mediator pattern).

Acceptance:
- `workflow-command.cs` pack no longer passes `-p:ContinuousIntegrationBuild=true`.
- A local build/pack evaluates `ContinuousIntegrationBuild` as empty (not true).
- On GitHub Actions (`GITHUB_ACTIONS=true`), `ContinuousIntegrationBuild` evaluates to `true`,
  so CI packages keep deterministic / SourceLink path mapping.

## Results

- `tools/dev-cli/endpoints/workflow-command.cs`: `PackAsync` no longer passes `-p:ContinuousIntegrationBuild=true`.
- `Directory.Build.props`: new `CI Build` property group sets `ContinuousIntegrationBuild=true` only when
  `'$(GITHUB_ACTIONS)' == 'true'` (Mediator pattern). The GitHub Actions `workflow.yml` job runs
  `dev workflow` on Actions, so CI packs still get the property from MSBuild.
- Evaluation check (`dotnet msbuild source/timewarp-flexbox/timewarp-flexbox.csproj -getProperty:ContinuousIntegrationBuild -p:Configuration=Release`):
  without `GITHUB_ACTIONS` → empty; with `GITHUB_ACTIONS=true` → `true`.
- Boy-scout audit fixes (`ganda repo audit --fix`): TimeWarp.Terminal 1.0.2 → 2.0.0-beta.1 (dev-cli only),
  TimeWarp.SourceGenerators 1.0.0-beta.14 → 1.0.0-beta.15, redeployed `.githooks/*.cs` (memsearch-scaffold),
  and set the VS Code window title (`.vscode/` is gitignored). The dev CLI was reinstalled and the gates rerun after these changes.

### Gates

- `./bin/dev build` — passed (0 warnings, 0 errors).
- `./bin/dev test` — passed: 1468/1468 on net10.0 and 1468/1468 on net11.0.
- `ganda repo audit` — first run failed 3 checks (package-pin-currency, memsearch-scaffold, vscode-window-icon).
  After `--fix`, build and test were rerun and passed, and the audit passed: 44 passed, 0 failed, 2 skipped (exit 0).

### How to validate

Smoke:
```bash
P=source/timewarp-flexbox/timewarp-flexbox.csproj
env -u GITHUB_ACTIONS dotnet msbuild $P -getProperty:ContinuousIntegrationBuild -p:Configuration=Release
GITHUB_ACTIONS=true dotnet msbuild $P -getProperty:ContinuousIntegrationBuild -p:Configuration=Release
grep -n ContinuousIntegrationBuild tools/dev-cli/endpoints/workflow-command.cs
```

Expect: the first command prints an empty line, the second prints `true`, and the grep finds nothing.

## Session

- Implement oracle: implemented by implementer-claude (claude, vendor anthropic, model claude-opus-5-5), session not reported, max-turns 500, Done — 2026-11-13T17:32:12Z
