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

- [x] **Make the repository public** (done 2026-07-04, verified PUBLIC) — it is currently PRIVATE; the site's privacy
      check and raw.githubusercontent skill fetches require a public repo
- [x] **Publish to nuget.org instead of (or in addition to) GitHub Packages** (Trusted Publishing registered 2026-07-04; readme install section rewritten) —
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
- [x] Confirm the org App/vars are visible to this repo (they are org-level for
      terminal; private repos may need enabling)
- [x] Dry-run: `workflow_dispatch` the pipeline without a release; then cut the next
      release and verify the dispatch lands (timewarp-software Actions shows a
      `rebuild` repository_dispatch run) and the package page appears
- [x] After task 142 lands, verify the flexbox skill shows on
      https://timewarp.software/ skills index — VERIFIED 2026-07-04: skill live at
      /skills/flexbox/SKILL.md and in the skills index; package page live at
      /packages/timewarp.flexbox/

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

## Release-chain verification (2026-07-04, v1.0.0-beta.4)

- Release pipeline green end-to-end in Actions: OIDC login, version check,
  pack, "Your package was pushed" to nuget.org, "timewarp-software rebuild
  dispatched" (org App token mint worked).
- Package live on nuget.org: listed=true, gallery page 200, consumer install
  verified (plain dotnet add package; readme example output exact).
- Site rebuild runs complete successfully on each dispatch.
- REMAINING: nuget.org's SEARCH index (which drives the site's owner-search
  catalog) lags for brand-new package ids - still 0 hits ~2h after push.
  Once it indexes, any rebuild (manual dispatch or the nightly cron) picks up
  the package page and the flexbox skill automatically. Verify then check the
  final box below.

## Results (2026-07-04)

Complete and verified end-to-end with v1.0.0-beta.4:
release event -> OIDC Trusted Publishing -> push to nuget.org -> rebuild
dispatch -> site rebuild -> https://timewarp.software/ shows the
timewarp.flexbox package page and the flexbox skill.

Follow-up discovered in the process: the dispatch races NuGet's SEARCH index
(the site catalog's data source) — a brand-new package id took ~3.5h to become
searchable, so the release-triggered rebuilds ran too early and a manual
re-dispatch was needed. Fix proposed on the receiver side (poll the search
index for the payload's exact version before building, 30-min bound, nightly
backstop unchanged): timewarp-software PR #23.
