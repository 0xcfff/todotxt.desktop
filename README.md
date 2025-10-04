# todotxt.net

This is an implemention of [todo.txt](http://todotxt.org/) using the .NET framework. As far as I am aware, it is fully compliant with [Gina's spec](https://github.com/todotxt/todo.txt/blob/master/README.md).

**Note:** This project is based on the work of [benrhughes/todotxt.net](https://github.com/benrhughes/todotxt.net). 

There is installer for the latest version available from the [releases page](https://github.com/benrhughes/todotxt.net/releases).

## Maintenence mode

Please note that todotxt.net is now in maintenence mode. I am happy to receive bug reports and bug fixes, but no new features will be added at this stage.

## Building and Testing

This project includes a comprehensive Makefile for building and testing. The Makefile provides cross-platform support and follows .NET best practices.

### Quick Start

```bash
# Show all available targets
make help

# Build entire cross-platform solution (recommended)
make build

# Run all tests (automatically builds first)
make test

# Build and test release version
make release

# Clean build artifacts
make clean
```

### Available Targets

- **Build targets**: `build` (builds entire cross-platform solution)
- **Test targets**: `test`, `test-lib`, `test-avalonia`, `test-coverage`, `test-coverage-cobertura`
- **Development targets**: `watch`, `watch-tests`, `format`, `check-format`
- **CI targets**: `ci`, `ci-build`, `ci-test`
- **Utility targets**: `clean`, `clean-all`, `info`, `help`

### Configuration

You can customize the build using variables:

```bash
# Build in Release mode
make build CONFIGURATION=Release

# Use detailed verbosity
make build VERBOSITY=detailed

# Run tests with coverage
make test-coverage-cobertura
```

### Cross-Platform Testing

The project includes cross-platform testing capabilities:

```bash
# Run cross-platform tests
make test-cross-platform
```

## Note for contributors

If you change any of the files in the [`ToDoLib`](ToDoLib) folder, please make sure the current unit tests pass, and add new tests where appropriate.

Please send your pull requests to the `dev` branch. 

## Goals

 - menu driven interface for novices
 - minimalist, keyboard-driven UI for expert users
 - vim/gmail/twitter-like keyboard nav (single key, easily accessible)
 - re-usable library that other projects can use as a todo.txt API
 - API (but not UI) runs under Mono
 - full compliance with Gina's specs


## Features

 - Sorting by completed status, priority, project, context, alphabetically due date or the order in the file
 - Sorting respects multiple projects and contexts
 - Remembers preferences for the todo.txt file, sort order, window size and position
 - Manual or automatic moving of completed tasks into an archive (done.txt) file
 - Free text filtering/search
 - Intellisense for projects and contexts
 - Minimize to tray icon - double-click the icon or Ctrl+Alt+M to hide or show the app
 - Keyboard shortcuts:
	- O or Ctrl+O: open todo.txt file
	- C or Ctrl+N: new todo.txt file
	- N: new task
	- J: next task
	- K: prev task
	- X: toggle task completion
	- A: archive tasks
	- D or Del or Backspace: delete task (with confirmation)
	- U or F2: update task
    - T: append text to selected tasks
	- F: filter tasks (free-text, one filter condition per line)
	- I: set priority
    - 0: clear filter
    - 1-9: apply numbered filter preset
	- . or F5: reload tasks from file
	- ?: show help
	- Alt+Up: increase priority
	- Alt+Down: decrease priority
	- Alt+Left/Right: clear priority
 	- Ctrl+Alt+Up: increase due date by 1 day
	- Ctrl+Alt+Down: decrease due date by 1 day
    - Ctrl+Alt+Left/Right: remove due date 
    - Ctrl+Up: increase threshold date by 1 day
	- Ctrl+Down: decrease threshold date by 1 day
    - Ctrl+Left/Right: remove threshold date 
	- Ctrl+S: set threshold date 
	- Ctrl+Alt+P: add days to threshold date 
	- Ctrl+C: copy task to clipboard
	- Ctrl+Shift+C: copy task to edit field
	- Ctrl+Alt+M: hide/unhide windows
