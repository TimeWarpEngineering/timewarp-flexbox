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

- [ ] Frontmatter: `name: flexbox`; description that triggers on "flexbox layout in
      C#", "compute layout without a UI framework", "Yoga layout for .NET",
      "position/size a tree of boxes", etc.
- [ ] Core model section: build a `Node` tree (`InsertChild`), set styles via
      `Node.Style`, run `CalculateLayout.Calculate(root, availW, availH, Direction)`,
      read results from `Node.Layout` (`GetPosition(PhysicalEdge)`,
      `GetDimension(Dimension)`); `float.NaN` = undefined/unconstrained
- [ ] CSS-to-C# mapping table: every CSS flexbox property to its C# call
      (`width: 100px` -> `SetDimension(Dimension.Width, StyleSizeLength.Points(100))`,
      `margin: 10px` -> `SetMargin(Edge.All, StyleLength.Points(10))`,
      `gap` -> `SetGap(Gutter.All, ...)`, enums for direction/justify/align/wrap/
      position-type/overflow/display/box-sizing)
- [ ] Gotchas section (the things an agent will get wrong):
      - Defaults are Yoga's, not web CSS: `flex-direction: column`, `flex-shrink: 0`,
        `align-content: flex-start`; `Config.UseWebDefaults` for web behavior
      - `StyleLength` vs `StyleSizeLength` (edges/gaps vs dimensions/flex-basis)
      - Owner semantics: a child belongs to one parent; re-inserting an owned child
        throws (`YogaAssertException`) - `RemoveChild` first
      - Style mutations auto-dirty the tree; re-call `Calculate` to get fresh results
      - Positions are relative to the parent; accumulate for absolute coordinates
- [ ] Recipes: sidebar+content shell, wrapping card grid with gap, centered overlay
      via absolute insets, RTL, custom measured leaf (SetMeasureFunc with
      MeasureMode semantics), incremental re-layout loop
- [ ] Verify every code example compiles and produces the outputs shown (reuse the
      readme-example verification approach; `samples/layout-demo/layout-demo.cs` is
      a source of known-good snippets)
- [ ] Point to the visual demo and the Generated conformance tests as further
      reference material
- [ ] Check how sibling repos register/distribute their skills beyond the repo
      `skills/` folder (e.g. netclaw-skill-server) and hook flexbox in the same way
      if applicable

## Notes

- The audit scaffold already created `skills/` (currently only `.gitkeep`).
- Keep the skill self-contained: agents may load it without repo access, so inline
  the essential API surface rather than deferring to source files.
- The `description` frontmatter drives triggering accuracy - study the sibling
  descriptions (terminal, nuru, amuru) for phrasing that names both the library and
  the generic tasks it solves.

## Results

(Add after completion)
