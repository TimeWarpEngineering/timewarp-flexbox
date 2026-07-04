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
- [x] Decide on 145/146: both landed before release land first, or list as known gaps in release notes
- [x] Bump `<Version>` in `source/Directory.Build.props` to `1.0.0`
- [x] Update readme + skill install snippets (drop `--prerelease`; version 1.0.0)
- [x] PR dev -> master, green CI, merge (PR #11, efb7150)
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

## Results (2026-07-04)

RELEASED. https://github.com/TimeWarpEngineering/timewarp-flexbox/releases/tag/v1.0.0

- PR #11 merged green (efb7150); release pipeline: version check, pack,
  "Your package was pushed" to nuget.org (first STABLE version), rebuild
  dispatched.
- The timewarp-software search-index wait-gate (PR #23) worked on its first
  stable exercise: the rebuild completed after 1.0.0 became searchable, and
  the package page at https://timewarp.software/packages/timewarp.flexbox/
  shows 1.0.0; the flexbox skill remains live.
- Consumer install verified from nuget.org with no prerelease flag; the
  readme example produces the documented output.
- Release notes carry the stable-API commitment (39 public types under
  semver), breaking changes vs beta.4 (internals no longer public,
  GetChild returns Node, Clone detaches), the five since-beta.4 engine
  fixes, and the documented exclusions (FixFlexBasisFitContent - task 148,
  auto-min-size extension - task 149).

Suite at release: 1468 passed / 0 failed / 0 skipped.
