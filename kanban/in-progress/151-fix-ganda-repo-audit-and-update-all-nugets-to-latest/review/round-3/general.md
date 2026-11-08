# Round 3 — general
**Date:** 2026-11-09
**Scope reviewed:** `9ac7d1f..8d57233` product paths (commit 1e77e8a), plus a re-check of M1–M5 at HEAD.

## Summary

Commit 1e77e8a sets `GenerateDocumentationFile` to false in the AOT smoke project. On SDK 11.0.100-rc.3 the Native AOT publish looks for `bin/Release/net10.0/linux-x64/TimeWarp.Flexbox.AotSmoke.xml`, which is never written, and fails with MSB3030. The smoke binary ships no docs, and `tools/dev-cli/Directory.Build.props` already turns doc generation off for the same reason. The library project still generates `TimeWarp.Flexbox.xml`, and the publish output still includes it. Risk is low. No findings.

## Verification (this machine, SDK 11.0.100-rc.3.26522.148 default, SDK 10.0.403 installed)

- Repro: `dotnet publish tests/timewarp-flexbox-aot-smoke/... -c Release -r linux-x64 -p:GenerateDocumentationFile=true` fails with the same MSB3030 that PR #12 run 42503142992 hit.
- As committed: the same publish exits 0. `TimeWarp.Flexbox.AotSmoke` prints `AOT smoke: PASS (all layout checks)` and exits 0. The output holds the native binary, `.dbg`, and `TimeWarp.Flexbox.xml`.
- `dotnet build timewarp-flexbox.slnx -c Release`: 0 warnings, 0 errors.
- `dotnet test timewarp-flexbox.slnx`: 1468 passed, 0 failed, 0 skipped.
- `ganda repo audit`: exit 0, "Repository passes all audit checks."
- `git status` after these runs shows no product-path changes. Build outputs are ignored.
- M1–M5 (rounds 1–2) are unchanged by this delta and stay fixed.

## Considered, not raised

- The root `Directory.Build.props` turns on doc generation so IDE0005 runs in build. With docs off, the smoke project loses IDE0005 for its single `program.cs`. Scoping the property to RID publishes would keep it. The dev CLI already accepts the same tradeoff, the file is small, and the gate this commit restores (a Native AOT publish that runs) is intact. Not a suppression that skips the work the check measures.
- With no `global.json`, CI now builds the net10.0 product and release packages on the SDK 11 preview, because M1 added a `11.0.x` preview step. Local development uses the same SDK, and rounds 1–2 accepted M1. This is an operator follow-up (for example, pin the product build to SDK 10 once the dev CLI can select SDK 11 on its own), not a defect in this delta.
- The task asks for TimeWarp packages to come from the GitHub feed. Results record that the feed returned NAME_UNKNOWN for TimeWarp.Nuru and TimeWarp.Amuru, so those resolve from nuget.org. Rounds 1–2 accepted this. It was not re-checked here, because this review runs without network access.

## Issues

None.
