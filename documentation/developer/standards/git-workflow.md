# Git Workflow

Rules for version control and collaboration.

---

## Branching

Use the following branch naming pattern:

```
UserName/YYYY-MM-DD/feature-description
Cramer/2025-10-14/component-lifecycle
```

---

## Commits

- Never squash or rebase
- Write descriptive commit messages following [conventional commits](git-commit-message-format.md)
- Reference issues in commit messages: `Fixes #123`

---

## Pull requests

- Always use `--head` flag with `gh pr create`
- All code requires review before merge
- Address all comments before merge
- No force-push after review starts

---

## Code review

- Reviewers check standards compliance
- Focus on logic, not style (analyzers handle style)
- Approve only when all concerns are addressed
