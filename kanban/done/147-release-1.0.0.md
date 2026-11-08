# Task 147 - Release 1.0.0 (Stable)

## Summary

Publish the first stable release. The engine quality is release-ready
(Yoga-conformance-verified, AOT-guaranteed, benchmarked, zero-dependency,
documented, pipeline proven with beta.3/beta.4). The single hard blocker is
task 144 (API surface tightening) because 1.0.0 freezes the public API under
semver. Tasks 145/146 are recommended-but-optional coverage work — either land
them first or note the gaps in the release notes.

## Dependencies

- **Task 144 (BLOCKER)** — API surface tightening; do not tag 1.0.0 before it
- Task 145 (recommended) — remaining Yoga unit tests
- Task 146 (recommended) — intrinsic text-measurement coverage
- Some beta soak time is prudent: beta.4 shipped 2026-07-04 with zero external
  usage feedback so far

## Todo List

- [x] Confirm task 144 merged; public API reviewed and final
- [x] Decide on 145/146: land first, or list as known gaps in release notes
- [x] Bump `<Version>` in `source/Directory.Build.props` to `1.0.0`
- [x] Update readme + skill install snippets (drop `--prerelease`; version 1.0.0)
- [x] PR dev -> master, green CI, merge
- [x] `gh release create v1.0.0` (NOT --prerelease). Release notes: stable API
      commitment statement; conformance/AOT/perf highlights; anything excluded
      (FixFlexBasisFitContent, intrinsic text if 146 not done); breaking changes
      vs beta.4 from the 144 internalization (beta consumers using internals)
- [x] Verify the chain: nuget.org (stable channel now — the site's
      `prerelease=false` owner search picks it up), site rebuild (the PR #23
      wait-gate handles search-index lag), package page shows stable version
- [x] Consumer install check without `--prerelease`

## Notes

- First STABLE version: the site's stable-channel search
  (`prerelease=false`) will list it — until now flexbox only existed in the
  prerelease channel.
- Semver discipline starts here: internalization or renames after this
  require 2.0.0.

## Results

Closed out 2026-11-09 (the release itself shipped 2026-07-04; this card was never updated).

- Task 144 (API tightening), 145 and 146 are all in `kanban/done/`, so there are no
  known gaps to list
- `source/Directory.Build.props` `<Version>` is `1.0.0`
- PR #11 "feat: release 1.0.0 - stable API, full Yoga test coverage, five engine fixes" merged
- GitHub release `v1.0.0` published 2026-07-04, marked Latest, not prerelease
- nuget.org flat container lists `1.0.0` after `1.0.0-beta.4`
- readme install snippet is `dotnet add package TimeWarp.Flexbox` / `Version="1.0.0"` (no `--prerelease`)
- Consumer check 2026-11-09: a fresh net10.0 console app,
  `dotnet add package TimeWarp.Flexbox` resolved `1.0.0` from nuget.org and built

Not rechecked at close-out: the site rebuild and the nuget.org package page.

## Session

- Close-out: b3bc5dc2-e0c9-4b04-ac76-4a1bd4f7a1f3 (2026-11-09)
