# File Naming

Use kebab-case for all file and folder names in the repository.

---

## Apply to

- Source files (`.cs`)
- Project files (`.csproj`)
- Solution files (`.sln`)
- Documentation (`.md`)
- Configuration files
- Folder names

---

## Examples

✓ Correct:
```
timewarp-flexbox.sln
timewarp-flexbox.csproj
global-usings.cs
flex-node.cs
layout-algorithm.cs
source/timewarp-flexbox/
documentation/developer/standards/
```

✗ Incorrect:
```
TimeWarp.Flexbox.sln
TimeWarp.Flexbox.csproj
GlobalUsings.cs
FlexNode.cs
LayoutAlgorithm.cs
source/TimeWarp.Flexbox/
Documentation/Developer/Standards/
```

---

## Exceptions

C# namespaces and type names remain PascalCase as required by the language:

```csharp
// File: source/timewarp-flexbox/nodes/flex-node.cs
namespace TimeWarp.Flexbox;

public class FlexNode
{
  // ...
}
```

---

## Rationale

- Consistent with web and modern tooling conventions
- Avoids case-sensitivity issues across operating systems
- Matches URL-friendly naming patterns
- Enforced by analyzers in the build process
