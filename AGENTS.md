This is a C#/.NET project.

**Build:**
```sh
dotnet build ToDo.Net.sln
```

**Test:**
```sh
dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj
```

**Note:** The project currently has build errors. These will be addressed separately.

## Code Style

*   **Casing:**
    *   Classes, methods, properties, and enums: `PascalCase`.
    *   Local variables and private fields: `camelCase`. Private fields are prefixed with an underscore (e.g., `_filePath`).
*   **Braces:** Braces go on a new line (Allman style).
*   **`using` directives:** Place `using` directives inside the namespace declaration.
*   **`var` keyword:** Use `var` when the type is obvious.

## Code Structure

*   **`src/TodoTxt.Core`:** Core business logic (Model).
*   **`src/TodoTxt.UI`:** WPF user interface (View and ViewModel). Follows the MVVM pattern.
*   **`src/TodoTxt.Shared`:** Shared utility code.
*   **`src/TodoTxt.Lib.Tests`:** Unit tests for the core library.

**Important:** The `src/TodoTxt.UI` project is a WPF application and cannot be built on macOS and Linux. Do not attempt to build or run this project.

## Terminal and Development Environment Handling

### Terminal State Management
- Always verify terminal responsiveness before proceeding with critical operations
- Use non-interactive commands when possible to avoid blocking states
- Implement timeout mechanisms for long-running operations

### .NET Command Best Practices  
- Use `dotnet build --verbosity minimal --no-restore` to avoid verbose output
- Add `--no-restore` flag when appropriate to speed up builds
- Use `dotnet test --logger "console;verbosity=normal"` for controlled test output
- Avoid commands that require user input in automated contexts

## Task Workflow

Important! If a task changes any of GEMINI.md or AGENTS.md, make synchronous changes in other files in this list. 

When starting a new task that requires code modifications, follow these steps:

1.  **Branching:**
    *   Ensure you are not on the `main` branch. If you are, suggest creating a new branch for the task.

2.  **Testing and Coverage:**
    *   Before making any changes, run the tests with coverage and save the report:
        ```sh
        dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=reports/coverage/baseline.xml
        ```
    *   After completing the task, run the coverage report again:
        ```sh
        dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=reports/coverage/current.xml
        ```
    *   Compare `reports/coverage/current.xml` with `reports/coverage/baseline.xml` to ensure that test coverage has not decreased. If it has, notify the user and suggest which parts of the code are easiest to add tests for.

3.  **Project Constraints:**
    *   Ignore projects that cannot be built, such as the UI project.
