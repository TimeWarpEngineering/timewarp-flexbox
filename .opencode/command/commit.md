---
description: Git commit and push
subtask: true
---

# Commit Message Format

Follow Angular's commit convention:

```
<type>(<scope>): <subject>

<body>

<footer>
```

## Required Fields

### Type (REQUIRED)
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation only
- `style`: Formatting, no code change
- `refactor`: Code change that neither fixes bug nor adds feature
- `test`: Adding/correcting tests
- `build`: Build system or dependencies
- `ci`: CI configuration changes
- `perf`: Performance improvement

### Subject (REQUIRED)
- Imperative present tense ("add" not "added")
- No capitalization, no period at end
- Under 100 characters
- Focus on WHY from user perspective, not WHAT

## Optional Fields

### Scope
- Component/package affected in parentheses
- Special scopes: `documentation` → `docs`
- Omit for cross-cutting changes

### Body
- Explain motivation and contrast with previous behavior
- Imperative present tense

### Footer
- Reference issues: `Closes #123`
- Breaking changes: `BREAKING CHANGE: description`

## Examples
```
feat(flexbox): add support for gap property

Previously gap was ignored. This adds CSS gap support
for better spacing control without margins.

Closes #42
```

```
fix(layout): correct float precision in flex calculations
```

```
docs: update README with build instructions
```
