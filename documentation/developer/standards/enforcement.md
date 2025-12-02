# Enforcement

How standards are enforced in the project.

---

## Analyzers

- Roslynator for code style and best practices
- GlobalUsingsAnalyzer for global using organization
- Microsoft.CodeAnalysis.NetAnalyzers for .NET patterns

---

## CI/CD checks

- Build must succeed
- All tests must pass
- Analyzer warnings treated as errors

---

## Editor configuration

`.editorconfig` enforces:
- Indentation (2 spaces)
- Line endings (LF)
- Encoding (UTF-8)
- Trailing whitespace removal

---

## Exceptions

Standards may be violated when:
1. Performance critical path requires optimization
2. Interoperating with third-party code
3. Maintaining backward compatibility

All exceptions must be:
- Documented with `#pragma warning disable` and justification
- Reviewed during code review
- Tracked as technical debt if temporary
