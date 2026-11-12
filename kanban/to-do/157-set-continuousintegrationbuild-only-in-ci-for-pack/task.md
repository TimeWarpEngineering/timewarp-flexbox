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

- [ ] Gate or move the flag at `workflow-command.cs:155`
- [ ] Confirm CI packaging still sets `ContinuousIntegrationBuild`
- [ ] Confirm a local pack/workflow does not disable incrementality via this property

## Acceptance criteria

- A local `dev workflow` pack no longer passes `ContinuousIntegrationBuild=true` unless it is actually CI.
- CI-produced packages still set the property.

## Notes

- Sibling: timewarp-terminal 040 (`workflow.cs:124`). Pattern: Mediator `Directory.Build.props:8-9`.
- Filed by Jenzi from cross-repo build-telemetry audit 2026-11-12.
