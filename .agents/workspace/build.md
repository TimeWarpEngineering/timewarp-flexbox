---
description: Writing and editing code
mode: primary
---

## IMPORTANT: Custom Command Routing Rules

You MUST follow these routing rules for specific user requests:

1. When the user says "commit" or asks to "commit changes":
   - First check if the command exists: `[ -f .opencode/command/commit.md ]`
   - If it exists, DO NOT use git commands directly
   - Instead, use the Task tool with: Task(description="Execute commit command", prompt="/commit", subagent_type="general")
   - If it doesn't exist, proceed with normal git operations

2. When the user says "pr" or "pull request":
   - First check if the command exists: `[ -f .opencode/command/pr.md ] || [ -f .opencode/command/pull-request.md ]`
   - If either exists, DO NOT create a pull request directly
   - Instead, use the Task tool with: Task(description="Execute PR command", prompt="/pr", subagent_type="general")
   - If neither exists, proceed with normal PR creation

3. When the user says "test" or "run tests":
   - First check if the command exists: `[ -f .opencode/command/test.md ]`
   - If it exists, use Task tool with: Task(description="Execute test command", prompt="/test", subagent_type="general")
   - If it doesn't exist, run tests normally

4. When the user says "build":
   - First check if the command exists: `[ -f .opencode/command/build.md ]`
   - If it exists, use Task tool with: Task(description="Execute build command", prompt="/build", subagent_type="general")
   - If it doesn't exist, run build normally

5. When the user says "release":
   - First check if the command exists: `[ -f .opencode/command/release.md ]`
   - If it exists, use Task tool with: Task(description="Execute release command", prompt="/release", subagent_type="general")
   - If it doesn't exist, proceed with normal release process

6. When the user says "bump" or "bump version":
   - First check if the command exists: `[ -f .opencode/command/bump.md ]`
   - If it exists, use Task tool with: Task(description="Execute bump command", prompt="/bump", subagent_type="general")
   - If it doesn't exist, proceed with normal version bumping

7. When the user says "spellcheck" or "spell check":
   - First check if the command exists: `[ -f .opencode/command/spellcheck.md ]`
   - If it exists, use Task tool with: Task(description="Execute spellcheck command", prompt="/spellcheck", subagent_type="general")
   - If it doesn't exist, run spellcheck normally

8. When the user says "check issues" or "list issues":
   - First check if the command exists: `[ -f .opencode/command/issues.md ]`
   - If it exists, use Task tool with: Task(description="Execute issues command", prompt="/issues", subagent_type="general")
   - If it doesn't exist, proceed with normal issue checking

These routing rules ensure team-specific workflows are consistently followed when custom commands are available.
