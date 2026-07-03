# Task 142 - Create Flexbox Skill

## Summary

Author `skills/flexbox/SKILL.md` teaching AI agents how to use TimeWarp.Flexbox,
following the sibling-repo convention (timewarp-terminal `skills/terminal/SKILL.md`,
timewarp-nuru `skills/nuru/SKILL.md`, timewarp-amuru `skills/amuru/SKILL.md`):
YAML frontmatter (`name`, `description` tuned for auto-triggering), repository and
package links, a "When to Use What" table, then task-oriented guidance with
compiling code examples. Target length in line with siblings (~350-500 lines).

## Target Files

| Type  | Path                        |
| ----- | --------------------------- |
| Skill | `skills/flexbox/SKILL.md`   |

## Todo List

- [x] Frontmatter: `name: flexbox`; description that triggers on "flexbox layout in
      C#", "compute layout without a UI framework", "Yoga layout for .NET",
      "position/size a tree of boxes", etc.
- [x] Core model section: build a `Node` tree (`InsertChild`), set styles via
      `Node.Style`, run `CalculateLayout.Calculate(root, availW, availH, Direction)`,
      read results from `Node.Layout` (`GetPosition(PhysicalEdge)`,
      `GetDimension(Dimension)`); `float.NaN` = undefined/unconstrained
- [x] CSS-to-C# mapping table: every CSS flexbox property to its C# call
      (`width: 100px` -> `SetDimension(Dimension.Width, StyleSizeLength.Points(100))`,
      `margin: 10px` -> `SetMargin(Edge.All, StyleLength.Points(10))`,
      `gap` -> `SetGap(Gutter.All, ...)`, enums for direction/justify/align/wrap/
      position-type/overflow/display/box-sizing)
- [x] Gotchas section (the things an agent will get wrong):
      - Defaults are Yoga's, not web CSS: `flex-direction: column`, `flex-shrink: 0`,
        `align-content: flex-start`; `Config.UseWebDefaults` for web behavior
      - `StyleLength` vs `StyleSizeLength` (edges/gaps vs dimensions/flex-basis)
      - Owner semantics: a child belongs to one parent; re-inserting an owned child
        throws (`YogaAssertException`) - `RemoveChild` first
      - Style mutations auto-dirty the tree; re-call `Calculate` to get fresh results
      - Positions are relative to the parent; accumulate for absolute coordinates
- [x] Recipes: sidebar+content shell, wrapping card grid with gap, centered overlay
      via absolute insets, RTL, custom measured leaf (SetMeasureFunc with
      MeasureMode semantics), incremental re-layout loop
- [x] Verify every code example compiles and produces the outputs shown (reuse the
      readme-example verification approach; `samples/layout-demo/layout-demo.cs` is
      a source of known-good snippets)
- [x] Point to the visual demo and the Generated conformance tests as further
      reference material
- [ ] (deferred to task 143) Publication to https://timewarp.software/ happens automatically once the repo
      qualifies for the site's catalog: the site aggregates `skills/*/SKILL.md` from
      family repos' default branches at build time. Repo qualification and the
      release-triggered rebuild are task 143's scope; verify the skill appears on the
      site's skills index after both land.

## Notes

- The audit scaffold already created `skills/` (currently only `.gitkeep`).
- Keep the skill self-contained: agents may load it without repo access, so inline
  the essential API surface rather than deferring to source files.
- The `description` frontmatter drives triggering accuracy - study the sibling
  descriptions (terminal, nuru, amuru) for phrasing that names both the library and
  the generic tasks it solves.

## Results (2026-07-04)

`skills/flexbox/SKILL.md` created (~270 lines), following the sibling
convention (terminal/nuru/amuru): trigger-tuned frontmatter, When to Use What
table, core five-step model, full CSS-to-C# mapping table (both length types),
Yoga-vs-web defaults table with the UseWebDefaults escape hatch, reading
results + AbsolutePosition helper, six recipes (app shell, wrap grid, absolute
overlay, RTL, measure function with MeasureMode semantics, incremental
re-layout), pitfalls (owner semantics, NaN, length types, one-config-per-tree),
installation, and further-reference pointers.

Every code snippet was compiled and executed against the library; all output
comments in the skill are actual engine output (verified via a scratch console
consumer). Site publication happens automatically once task 143 lands (the
site aggregates skills/*/SKILL.md from qualifying repos at build time).
