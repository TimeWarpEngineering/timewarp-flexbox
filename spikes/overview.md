# Spikes Overview

This folder contains experimental code, prototypes, and exploratory work by individual developers.

## Purpose

Spikes are time-boxed investigations to:
- Explore new technologies or approaches
- Prototype features before full implementation
- Validate architectural decisions
- Learn and experiment without affecting main codebase

## Structure

Each developer has their own subfolder to work independently:

```
spikes/
├── cramer/
├── [other-developer-name]/
└── overview.md
```

## Guidelines

- **Isolated**: Spike code is separate from production code
- **Experimental**: No quality guarantees, break things freely
- **Time-boxed**: Document learnings and move on
- **Results-focused**: Share findings, not necessarily code
- **No interference**: Each developer owns their subfolder

## From Spike to Production

When a spike proves valuable:

1. Document findings and recommendations
2. Create proper task in [kanban](../kanban/)
3. Implement properly following [standards](../documentation/developer/standards/)
4. Spike code may be archived or deleted

## Naming Conventions

- Developer folders: lowercase (e.g., `cramer/`)
- Inside folders: any structure works
- No strict naming - this is experimental space

## Git Policy

- Commit spikes to share learnings
- Feel free to experiment with branches
- Clean up when done or archive interesting findings
- Add `.gitignore` for large binary files or dependencies
