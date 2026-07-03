# Task 140 - Release 1.0.0-beta.3

## Summary

Publish the first release of the rewritten (Yoga-port) engine. The three release
blockers were cleared on 2026-07-03: MIT LICENSE added (with Yoga attribution note),
readme rewritten against the current `Node`/`Style`/`CalculateLayout` API (example
verified compiling and producing the documented output), and version bumped to
1.0.0-beta.3 with `PackageLicenseExpression` set to MIT. Local `dotnet pack`
verified: dll + readme + logo, MIT license, version 1.0.0-beta.3.

## Todo List

- [x] Merge dev to master via PR (green CI) — PR #9 merged 2026-07-03 (1dfad96)
- [ ] Create GitHub release `v1.0.0-beta.3` — the release event triggers the
      workflow's publish step to GitHub Packages
- [ ] Release notes must state clearly: the API is entirely new; beta.2 and earlier
      were a different implementation (`FlexNode`/`FlexLayoutEngine`) that no longer
      exists. Known gaps: intrinsic text measurement untested,
      FixFlexBasisFitContent experimental feature unimplemented.
- [ ] Verify the package appears on the GitHub Packages feed and installs into a
      consumer project

## Toward stable 1.0 (follow-ups, not blockers for beta.3)

- [ ] Task 139: restore style/analyzer gates (naming decision + hand fixes)
- [ ] Port remaining hand-written Yoga unit tests (Baseline, AspectRatio, Edge,
      HadOverflow, Dirtied, Persistence, FlexGap, ...)
- [ ] Text-measurement test helper to unlock skipped IntrinsicSize tests
- [ ] Enable GenerateDocumentationFile so the package ships XML docs

## Results

(Add after completion)
