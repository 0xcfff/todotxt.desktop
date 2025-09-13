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

*   **`src/TodoTxt.Lib`:** Core business logic (Model).
*   **`src/TodoTxt.UI`:** WPF user interface (View and ViewModel). Follows the MVVM pattern.
*   **`src/TodoTxt.Shared`:** Shared utility code.
*   **`src/TodoTxt.Lib.Tests`:** Unit tests for the core library.

**Important:** The `src/TodoTxt.UI` project is a WPF application and cannot be built on macOS and Linux. Do not attempt to build or run this project.

## Task Workflow

When starting a new task that requires code modifications, follow these steps:

1.  **Branching:**
    *   Ensure you are not on the `main` branch. If you are, suggest creating a new branch for the task.

2.  **Testing and Coverage:**
    *   Before making any changes, run the tests with coverage and save the report:
        ```sh
        dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=coverage/baseline.xml
        ```
    *   After completing the task, run the coverage report again:
        ```sh
        dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=coverage/current.xml
        ```
    *   Compare `coverage/current.xml` with `coverage/baseline.xml` to ensure that test coverage has not decreased. If it has, notify the user and suggest which parts of the code are easiest to add tests for.

3.  **Project Constraints:**
    *   Ignore projects that cannot be built, such as the UI project.