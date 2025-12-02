# Documentation Standards

Rules for writing and maintaining documentation.

---

## Temporal references

Never use temporal language in code or documentation:

- ❌ "currently", "now", "at this time", "for now"
- ✅ Describe what IS, not when it was written
- ✅ Use versions for historical context: "As of v0.2.0..."

---

## File naming

See [file-naming.md](file-naming.md) for complete details.

- Use kebab-case for all files and folders
- Exception: MSBuild files require specific casing (`Directory.Build.props`)

---

## Markdown structure

```markdown
# Title (H1 - Once per document)

Brief description.

## Section (H2)
Content...

### Subsection (H3)
Content...

#### Details (H4)
Content...
```

---

## Code blocks

Always specify the language:

````markdown
```csharp
public class Example { }
```
````
