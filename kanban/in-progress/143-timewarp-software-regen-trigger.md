# Task 143 - Trigger timewarp-software Regen on Release

## Summary

Adopt the timewarp-terminal release pipeline so publishing a release (a) pushes the
package where https://timewarp.software/ can see it and (b) dispatches an immediate
site rebuild, making the package page and the flexbox skill (task 142) appear on the
site without waiting for the nightly cron backstop.

## How the working pattern operates (from timewarp-terminal)

1. `workflow.yml` has a single `ci` job that delegates the pipeline to the dev CLI:
   `dotnet run tools/dev-cli/dev.cs -- workflow [--api-key ...]`.
2. On `release`, `nuget/login@v1` (OIDC Trusted Publishing, user
   `TimeWarp.Enterprises`) mints the NuGet API key — no stored secret.
3. A short-lived installation token is minted from the org's "TimeWarp Rebuild
   Dispatcher" GitHub App (`vars.REBUILD_APP_ID` + `secrets.REBUILD_APP_PRIVATE_KEY`,
   both org-level) because the default `GITHUB_TOKEN` cannot reach other repos. The
   step is skipped if the app is not configured.
4. After pushing packages, the dev CLI's workflow command runs a best-effort
   `gh api repos/TimeWarpEngineering/timewarp-software/dispatches -f event_type=rebuild
   -f client_payload[package]=... -f client_payload[version]=...` — a failure warns
   but never fails a release that already pushed (the site rebuilds nightly anyway).

## How timewarp.software picks things up

- The catalog is driven by a **nuget.org owner search** — packages published to
  nuget.org under TimeWarp.Enterprises get pages automatically; GitHub Packages-only
  packages are invisible to it.
- Skills are aggregated at site build time from family repos' `skills/*/SKILL.md`
  on the default branch (plus `extraSkillRepos` in
  `timewarp-software/source/data/catalog-overrides.json` for repos with no NuGet
  package). Once this repo qualifies and task 142's skill exists, no site-side
  change is needed.

## Prerequisites (decisions needed)

- [ ] **Make the repository public** — it is currently PRIVATE; the site's privacy
      check and raw.githubusercontent skill fetches require a public repo
- [ ] **Publish to nuget.org instead of (or in addition to) GitHub Packages** —
      register TimeWarp.Flexbox for Trusted Publishing under TimeWarp.Enterprises;
      once done, rewrite the readme installation section (the PAT/GitHub-Packages
      dance becomes a plain `dotnet add package`)

## Todo List

- [x] Extend `tools/dev-cli/endpoints/workflow-command.cs` (currently the ganda
      scaffold: clean/build/test) with the release path from terminal's
      `tools/dev-cli/endpoints/workflow.cs`: verify samples, pack, push with
      `--api-key`, then `NotifySoftwareSiteAsync` repository_dispatch (best-effort,
      non-fatal)
- [x] Rewrite `.github/workflows/workflow.yml` to the terminal shape: dev-cli-driven
      `ci` job, `nuget/login@v1` OIDC on release, conditional
      `actions/create-github-app-token@v2` rebuild-token mint
      (`vars.REBUILD_APP_ID` / `secrets.REBUILD_APP_PRIVATE_KEY`, owner
      TimeWarpEngineering, repositories timewarp-software), `GH_TOKEN` passed to the
      pipeline, artifact upload
- [ ] Confirm the org App/vars are visible to this repo (they are org-level for
      terminal; private repos may need enabling)
- [x] Dry-run: `workflow_dispatch` the pipeline without a release; then cut the next
      release and verify the dispatch lands (timewarp-software Actions shows a
      `rebuild` repository_dispatch run) and the package page appears
- [ ] After task 142 lands, verify the flexbox skill shows on
      https://timewarp.software/ skills index (or add this repo to
      `extraSkillRepos` if it remains off nuget.org)

## Dependencies

- Task 142 (flexbox skill) — content this pipeline publishes
- Task 140 released beta.3 to GitHub Packages; the next release should exercise the
  new pipeline

## Results

(Add after completion)

## Progress (2026-07-04)

Implemented and verified everything not gated on the two prerequisites:

- workflow-command.cs extended to terminal parity: PR path (clean -> build ->
  verify-samples -> test) and release path (+ check-version -> pack -> push ->
  notify). Release detected via --api-key or GITHUB_EVENT_NAME=release. Falls
  back to running dev.cs as a runfile when bin/dev (uncommitted self-install
  artifact) is absent, so CI needs no bootstrap step.
- .timewarp/dev.jsonc added (checkVersionConfig -> TimeWarp.Flexbox) so the
  packaged check-version works unconfigured.
- workflow.yml rewritten to the terminal shape: dev-cli-driven ci job,
  nuget/login OIDC on release, conditional Rebuild Dispatcher app-token mint,
  GH_TOKEN passed to the pipeline, AOT smoke step retained, artifact upload.
- Verified locally: PR workflow green end-to-end; release dry-run green
  (version check "safe to release", pack succeeds, push/notify skipped without
  key); repository_dispatch tested for real with local gh auth - timewarp-
  software started a "rebuild" repository_dispatch run from our payload.

Remaining items require owner action: make the repo public, register
TimeWarp.Flexbox for nuget.org Trusted Publishing under TimeWarp.Enterprises,
then cut the next release and verify the package page + flexbox skill appear
on https://timewarp.software/.
