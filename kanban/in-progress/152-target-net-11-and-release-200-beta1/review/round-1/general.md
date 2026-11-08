# Round 1 — general
**Date:** 2026-11-09
**Scope reviewed:** `master...HEAD`, product commit 14a7714, 11 product files. The scope covers the workflow, the root and source `Directory.Build.props`, the three `.csproj` files, the layout demo, the readme, `skills/flexbox/SKILL.md`, and the comment-only edits in the runfiles and dev-cli props.

## Summary

The change multi-targets the library, the unit tests, and the AOT smoke for
`net10.0;net11.0`. Each of those projects clears the root `TargetFramework` pin
with an empty `<TargetFramework></TargetFramework>`. The other projects keep
the root `net10.0` pin: benchmarks inherit it, and the layout demo sets it with
`#:property TargetFramework=net10.0`. The version moves to `2.0.0-beta.1` and
`PackageReleaseNotes` carries the net10.0 sunset sentence. The readme and skill
install snippets use `--prerelease` and pin `2.0.0-beta.1`. CI publishes and runs
the AOT smoke once per TFM.

The risk is low: the change is build configuration and docs only, with no code
changes. I found no defects.

## Checks against the task

| Requirement | Evidence | Result |
|-------------|----------|--------|
| Library `TargetFrameworks` net10.0;net11.0, nupkg has both lib folders | `source/timewarp-flexbox/timewarp-flexbox.csproj:19-20`. `unzip -l artifacts/packages/TimeWarp.Flexbox.2.0.0-beta.1.nupkg` lists `lib/net10.0/` and `lib/net11.0/` (dll + xml) | met |
| Tests on both TFMs | `test/timewarp-flexbox-tests/timewarp-flexbox-tests.csproj:8-9`. Results report 1468 passing on each TFM, locally and in CI run 42508073157 | met |
| AOT smoke covers stated TFMs, XML-doc skip kept | `tests/timewarp-flexbox-aot-smoke/...csproj:8-10` sets both TFMs, and `GenerateDocumentationFile` false is still there. `.github/workflows/workflow.yml:91-96` loops over both TFMs with `--framework` and separate `-o` folders | met |
| Samples on one TFM, stated | Root `Directory.Build.props:16-20` comment. `samples/layout-demo/layout-demo.cs:4` pins net10.0. Benchmarks inherit the root pin | met |
| `<Version>` 2.0.0-beta.1 | `source/Directory.Build.props:8` | met |
| Readme/skill `--prerelease` / `2.0.0-beta.1`; 1.0.0 stays stable | `readme.md:17-28`, `skills/flexbox/SKILL.md:281-292` | met |
| Release notes: net10.0 ends at 2.0.0 GA, stay on 1.x | `source/Directory.Build.props:9` (`PackageReleaseNotes`), plus readme and skill prose | met |
| `ganda repo audit` exits 0 | Re-run by the reviewer on this worktree: "Repository passes all audit checks.", exit 0 | met |
| CI green on both TFMs | Results cite workflow_dispatch run 42508073157 (both TFMs, both AOT binaries PASS). The reviewer did not re-fetch it, because this oracle runs offline | met (as reported) |
| Do not drop net10.0, do not cut release | No release tag or publish step in the diff. The checklist leaves the release open | met |

I checked for leftover single-TFM assumptions. `git grep` for `bin/Release`,
`bin/Debug`, and `-f net` in `tools/`, `.github/`, and `runfiles/` finds only the
new AOT loop. No dev-cli path is tied to one TFM.

The goal restatement matches the task. I found no misread.

## Issues

None.
