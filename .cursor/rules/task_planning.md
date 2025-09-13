---
description: Task Planning
globs:
alwaysApply: false
---

# Task Planning Rule

## Overview
When a user describes a task, agents should **plan first, implement later**. This rule establishes a structured planning process that produces documentation before any code changes.

## Planning Process

### 1. Create Task Directory
- Create `tasks/{task_name}/` directory where `task_name` is a short, descriptive name derived from user's request
- Task name should be kebab-case (e.g., `user-authentication`, `data-export`, `ui-refactor`)

### 2. Research Phase
- Use read-only tools to understand the codebase
- Analyze existing patterns, architecture, and dependencies
- Document findings in `tasks/{task_name}/research.md`
- **No code changes during research**

### 3. Planning Documentation
Create these files in the task directory:

#### `tasks/{task_name}/plan.md`
- **Objective**: Clear task description
- **Scope**: What's included/excluded
- **Approach**: High-level strategy
- **Components**: Files/classes to modify
- **Dependencies**: External libraries, APIs, or features needed
- **Risks**: Potential challenges or blockers

#### `tasks/{task_name}/implementation.md`
- **Steps**: Ordered list of implementation steps with checkmarks
- **Testing Strategy**: How changes will be tested
- **Rollback Plan**: How to undo changes if needed

### 4. Interactive Planning
- Ask clarifying questions about requirements
- Present multiple approaches when applicable
- Request user input on architectural decisions
- Validate assumptions with user before proceeding
- Balance user involvement with task complexity

### 5. Supporting Documentation
As needed, create:
- `api-docs/`: External API documentation
- `samples/`: Example requests/responses
- `references/`: Relevant code samples or patterns
- `mockups/`: UI sketches or wireframes

## Implementation Trigger
Do not perform any implementation, the only purpose at this point is to make the plan and adjust it based on user asks.

## Guidelines
- Keep planning proportional to task complexity
- Focus on understanding
- Document decisions and rationale
- Use the task folder for future reference and documentation
