# AI Agents Overview

This folder contains workspaces for AI agents working on the project.

## Purpose

AI agents use these folders for:
- Exploratory code generation and prototypes
- Experimental implementations
- Scratch work and iterations
- Multi-step task breakdowns
- Isolated experimentation without affecting main codebase

## Structure

Each AI agent has its own workspace subfolder:

```
.agents/
├── workspace/
│   ├── claude/
│   ├── grok/
│   └── [other-agent-name]/
└── overview.md
```

## Guidelines

- **Agent-specific**: Each agent owns its subfolder
- **Experimental**: No quality guarantees, iterate freely
- **Temporary**: Code here is draft/exploration, not production
- **No interference**: Agents don't modify each other's workspaces
- **Human review required**: All production code must be reviewed and properly committed

## From Agent Workspace to Production

When agent work is ready for production:

1. Human reviews the generated code
2. Code is moved/refactored into proper location following [standards](../documentation/developer/standards/)
3. Proper commit messages and documentation added
4. Agent workspace may be cleaned up or archived

## Naming Conventions

- Agent folders: lowercase (e.g., `claude/`, `grok/`)
- Inside folders: any structure works
- Similar to human [spikes](../spikes/) folder but for AI agents

## Git Policy

- Typically excluded from commits (see `.gitignore`)
- May commit significant prototypes for reference
- Clean up regularly to avoid clutter
- Archive interesting experiments if valuable

## Relationship to Human Spikes

This is the AI equivalent of the [spikes](../spikes/) folder:
- **spikes/**: Human developers experiment here
- **.agents/workspace/**: AI agents experiment here
- Both provide isolated sandbox for exploration
- Both feed into proper implementation via [kanban](../kanban/) tasks
