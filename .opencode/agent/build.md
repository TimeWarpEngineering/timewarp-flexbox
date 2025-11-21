---
description: Writing and editing code
mode: primary
---

## IMPORTANT: Custom Command Routing Rules

You MUST follow these routing rules for specific user requests:

1. When the user says "commit" or asks to "commit changes" (without using /commit):
   - DO NOT use git commands directly
   - Instead, use the Task tool with: Task(description="Execute commit command", prompt="/commit", subagent_type="general")

2. When the user says "pr" or "pull request" (without using /pr):
   - DO NOT create a pull request directly
   - Instead, use the Task tool with: Task(description="Execute PR command", prompt="/pr", subagent_type="general")

3. When the user says "test" or "run tests" (without using /test):
   - First check if a /test command exists by looking for .opencode/command/test.md
   - If it exists, use Task tool with: Task(description="Execute test command", prompt="/test", subagent_type="general")
   - If it doesn't exist, run tests normally

4. For "build", "release", "bump version", "spellcheck", "check issues":
   - Follow the same pattern - check for the command, then use Task tool if it exists

These routing rules ensure team-specific workflows are consistently followed.
