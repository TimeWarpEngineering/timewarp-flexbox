# Adopt the shared pull request build classifier and the one-workflow CI standard

## Description

Bring timewarp-flexbox to the org CI standard (ganda 476 / 479 / 565 / 587, audited by ganda 655): `workflow.yml` has
no `paths` / `paths-ignore` filter on `pull_request`, so every pull request reports `CI`, and `dev workflow`
calls the shared classifier `PullRequestBuildClassifier` from `TimeWarp.Nuru.DevCli` 4.0.0-beta.15
(timewarp-nuru 595) to skip build and test on a pull request that touches no build path (kanban, docs, ...).

Tracked by timewarp-ganda 656 (architecture reposet rollout).

Survey 2026-11-16 for this repo: has a `pull_request` paths filter; `dev workflow` does not classify. Work: remove the filter, add the classifier call.

## Requirements

1. Run `ganda repo audit --fix` first and commit what it changes.
2. Bump the `TimeWarp.Nuru.DevCli` pin to `4.0.0-beta.15` (and `TimeWarp.Nuru` to the same version if pinned).
3. In the `workflow` command's pull-request path, call `PullRequestBuildClassifier.DecideAsync` first. On skip,
   print its message and exit 0. Push and release paths are unchanged.
4. Remove `paths` / `paths-ignore` from the `pull_request` trigger of `.github/workflows/workflow.yml`. Keep any
   `push` filter and add a comment saying why (a kanban-only merge must not rebuild and upload packages).
5. If this repo builds from roots outside the classifier defaults (`source`, `tests`, `tools`, `msbuild`), declare
   them through the classifier's config (`PullRequestBuildConfig`), not by copying or editing the rule.
6. No local copy of the classification rule remains in `tools/dev-cli/`.
7. `ganda repo audit` passes, including both CI-standard checks from ganda 655.

## Goal (implementer restatement)

Every pull request on timewarp-flexbox must report a `CI` check, and a pull request that touches only
non-build paths (kanban, docs, ...) must pass it quickly without building. To get there:

- Commit whatever `ganda repo audit --fix` changes.
- Pin `TimeWarp.Nuru` and `TimeWarp.Nuru.DevCli` at `4.0.0-beta.15` (the version that ships `PullRequestBuildClassifier`).
- `dev workflow` calls `PullRequestBuildClassifier.DecideAsync` before clean/build on the non-release path; on a
  skip it prints the classifier message and exits 0. Release is untouched.
- `workflow.yml`: no `paths` on `pull_request`; the `push` filter stays with a comment saying why.
- Build roots the defaults miss (`test/`, `benchmarks/`, `samples/`, packed `readme.md` and `assets/logo.png`)
  go in `.timewarp/dev.jsonc` `pullRequestBuild.buildPaths`, not in a copied rule.
- No local copy of the rule under `tools/dev-cli/`.
- `ganda repo audit` passes, including `workflow-file` and `dev-workflow-classifier`.

Acceptance: build, test, and audit gates green; the task PR shows a `CI` run.

## Checklist

- [x] `ganda repo audit --fix` run and committed
- [x] DevCli pin at 4.0.0-beta.15
- [x] `dev workflow` calls the shared classifier on pull requests; skip exits 0 with its message
- [x] No `paths` filter on `pull_request`; `push` filter kept with a why comment
- [x] Extra build roots (if any) declared through config
- [x] `ganda repo audit` clean
- [ ] This task's pull request shows a `CI` run (record the URL in Results)

## Notes


After merge, the ganda 656 tracker proves a kanban-only pull request here reports `CI` success and merges with
plain `ganda pr merge` (no `--allow-no-ci`).

## Session

- Created: 64677b22-d705-4b25-b537-4b53732bcb92 (2026-11-16)
- 2026-11-16 implementer (claude-opus-5-5): `ganda repo audit --fix` added `.githooks/prepare-commit-msg` (committed).
  The audit also failed `package-pin-currency`; bumped Amuru/Amuru.Tools 3.0.0-beta.5 and Terminal 2.0.0-beta.2 with Nuru/DevCli.
  Removed the TWA0038 line-cap globalconfig target from `tools/dev-cli/Directory.Build.props`: every DevCli
  beta.15 content file is under 500 lines.

## Results

Changes:
- `Directory.Packages.props`: TimeWarp.Nuru and TimeWarp.Nuru.DevCli 4.0.0-beta.15; TimeWarp.Amuru and
  TimeWarp.Amuru.Tools 3.0.0-beta.5; TimeWarp.Terminal 2.0.0-beta.2 (audit `package-pin-currency`).
- `tools/dev-cli/endpoints/workflow-command.cs`: the non-release path calls `PullRequestBuildClassifier.DecideAsync`
  before clean. On a skip it prints the message, writes `requires_build=false` to `GITHUB_OUTPUT`, and exits 0.
  The release path is unchanged. The classifier itself builds on push, dispatch, and local runs.
- `.github/workflows/workflow.yml`: no `paths` on `pull_request` (comment says why); `push` filter kept with a
  comment (a kanban-only merge must not rebuild and upload packages). The run step has `id: pipeline`. Upload Coverage
  (`if-no-files-found: error`) and the AOT smoke test skip when `requires_build == 'false'`, so a skipped PR stays green.
- `.timewarp/dev.jsonc`: `pullRequestBuild.buildPaths` = `readme.md`, `assets/logo.png` (both packed into the nupkg),
  `test/**`, `benchmarks/**`, `samples/**` (build roots outside the classifier defaults, markdown included).
- `tools/dev-cli/Directory.Build.props`: removed the obsolete DevCli line-cap globalconfig target.
- No local copy of the classification rule in `tools/dev-cli/`; the rule comes from DevCli content.

Gates:
- `./bin/dev build`: succeeded, 0 warnings, 0 errors.
- `./bin/dev test`: passed, 1468/1468.
- `ganda repo audit`: "Repository passes all audit checks." (includes `workflow-file` and `dev-workflow-classifier`).

### How to validate

Smoke (skip path, kanban-only diff 227809f...e81a641):
```
T=$(mktemp -d); printf '{"pull_request":{"base":{"sha":"%s"},"head":{"sha":"%s"}}}' $(git rev-parse 227809f) $(git rev-parse e81a641) > $T/ev.json
GITHUB_EVENT_NAME=pull_request GITHUB_EVENT_PATH=$T/ev.json GITHUB_OUTPUT=$T/out ./bin/dev workflow; echo $?; cat $T/out
```
Expect: `Pull request changes only declared non-build paths. Skipping build and test. ... (2 changed paths)`, exit 0,
and `$T/out` contains `requires_build=false`.

Smoke (build path): same with base `3a0e427` and head `HEAD`.
Expect: `Build path changed: .githooks/prepare-commit-msg. Full build.` then clean/build/... run.

CI: this task's pull request must show a `CI` run (URL recorded after the host opens the PR).

