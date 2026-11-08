# Round 1 — merged findings
**Date:** 2026-11-09
**Sources:** general

## Counts

| Severity | open | fixed | wontfix |
|----------|------|-------|---------|
| bug | 0 | 0 | 0 |
| suggestion | 0 | 0 | 1 |
| nit | 0 | 0 | 2 |

## Issues

### M1 — Severity: suggestion — Status: wontfix
- File: tools/dev-cli/endpoints/verify-samples-discovery.cs:112
- Description: `IsUnderBuildOutput` checks the absolute path. A checkout under a `bin` or `obj` directory skips every sample and fails with a misleading "no runfiles or projects" message.
- Suggestion: Check only the path segments relative to `samples/`.
- Source: general
- Disposition notes: wontfix on this task (review oracle). It fails closed: the command exits 1, so a broken sample can never pass, and the gate the task asked for still holds. CI paths (`/home/runner/work/...`) and the standard ganda worktree layout do not contain those segments. A fix pass costs a full implementer round for a cosmetic false failure on unusual checkouts. Candidate follow-up if anyone hits it.

### M2 — Severity: nit — Status: wontfix
- File: tools/dev-cli/endpoints/verify-samples-command.cs:115
- Description: The `{out}` file is always named `output.html`.
- Suggestion: Let the args file choose the name, or document the fixed `.html` name.
- Source: general
- Disposition notes: wontfix (review oracle). The only sample is the HTML layout demo. Generalize when a second sample with file output appears.

### M3 — Severity: nit — Status: wontfix
- File: tools/dev-cli/endpoints/verify-samples-discovery.cs:158
- Description: The args file splits on spaces and supports no quoting.
- Suggestion: Document this, or read one argument per line.
- Source: general
- Disposition notes: wontfix (review oracle). The current template has no argument with a space, and temp paths are substituted after the split. Revisit with M2.

## Duplicates / conflicts

- None. There was a single reviewer.
