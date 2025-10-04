# Makefile for TodoTxt.Desktop
# Cross-platform build and test automation

# Variables
DOTNET := dotnet
SOLUTION := ToDo.Net.sln
CONFIGURATION ?= Debug
VERBOSITY ?= minimal
COVERAGE_DIR := reports/coverage
TEST_REPORTS_DIR := reports/test
REPORTS_DIR := reports

# OS detection
UNAME_S := $(shell uname -s)
ifeq ($(UNAME_S),Darwin)
    OS := macOS
else ifeq ($(UNAME_S),Linux)
    OS := Linux
else ifeq ($(UNAME_S),CYGWIN_NT-10.0)
    OS := Windows
else ifeq ($(UNAME_S),MINGW32_NT-6.1)
    OS := Windows
else ifeq ($(UNAME_S),MINGW32_NT-6.2)
    OS := Windows
else ifeq ($(UNAME_S),MINGW32_NT-6.3)
    OS := Windows
else ifeq ($(UNAME_S),MINGW32_NT-10.0)
    OS := Windows
else ifeq ($(UNAME_S),MSYS_NT-10.0)
    OS := Windows
else
    OS := Unknown
endif

# Project paths (for reference in info target)
LIB_PROJECT := src/TodoTxt.Lib/TodoTxt.Lib.csproj
SHARED_PROJECT := src/TodoTxt.Shared/TodoTxt.Shared.csproj
PLATFORM_PROJECT := src/TodoTxt.Platform/TodoTxt.Platform.csproj
AVALONIA_PROJECT := src/TodoTxt.Avalonia/TodoTxt.Avalonia.csproj
LIB_TESTS_PROJECT := src/TodoTxt.Lib.Tests/ToDoTests.csproj
AVALONIA_TESTS_PROJECT := src/TodoTxt.Avalonia.Tests/TodoTxt.Avalonia.Tests.csproj

# Colors for output
RED := \033[0;31m
GREEN := \033[0;32m
YELLOW := \033[1;33m
BLUE := \033[0;34m
NC := \033[0m # No Color

# Default target
.PHONY: help
help: ## Show this help message
	@echo "TodoTxt.Desktop Makefile"
	@echo "========================"
	@echo ""
	@echo "Available targets:"
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z_-]+:.*?## / {printf "  $(BLUE)%-20s$(NC) %s\n", $$1, $$2}' $(MAKEFILE_LIST)
	@echo ""
	@echo "Variables:"
	@echo "  CONFIGURATION    Build configuration (Debug|Release) [default: Debug]"
	@echo "  VERBOSITY        Build verbosity (quiet|minimal|normal|detailed|diagnostic) [default: minimal]"
	@echo ""
	@echo "Examples:"
	@echo "  make build                    # Build entire cross-platform solution"
	@echo "  make build CONFIGURATION=Release  # Build in Release mode"
	@echo "  make test                     # Build and run all tests"
	@echo "  make clean                    # Clean build artifacts"

# Check prerequisites
.PHONY: check-dotnet
check-dotnet:
	@echo "$(BLUE)[INFO]$(NC) Checking .NET SDK..."
	@which $(DOTNET) > /dev/null || (echo "$(RED)[ERROR]$(NC) .NET SDK not found. Please install .NET 9.0 SDK" && exit 1)
	@echo "$(GREEN)[SUCCESS]$(NC) .NET SDK found: $$($(DOTNET) --version)"

# Restore dependencies
.PHONY: restore
restore: check-dotnet ## Restore NuGet packages
	@echo "$(BLUE)[INFO]$(NC) Restoring dependencies..."
	@$(DOTNET) restore $(SOLUTION) --verbosity $(VERBOSITY)
	@echo "$(GREEN)[SUCCESS]$(NC) Dependencies restored"

# Build targets
.PHONY: build
build: restore ## Build entire cross-platform solution
	@echo "$(BLUE)[INFO]$(NC) Building entire cross-platform solution..."
	@$(DOTNET) build $(SOLUTION) --configuration $(CONFIGURATION) --verbosity $(VERBOSITY) --no-restore
	@echo "$(GREEN)[SUCCESS]$(NC) Cross-platform solution built successfully"


# Test targets
.PHONY: test
test: build ## Run all tests using cross-platform solution
	@echo "$(BLUE)[INFO]$(NC) Running all tests using cross-platform solution..."
	@mkdir -p $(TEST_REPORTS_DIR)
	@mkdir -p $(REPORTS_DIR)
	@echo "$(BLUE)[INFO]$(NC) Capturing test results for $(OS) ($(CONFIGURATION))..."
	@echo "TodoTxt.Desktop Test Report" > $(TEST_REPORTS_DIR)/test-report-$(OS)-$(CONFIGURATION).txt
	@echo "===========================" >> $(TEST_REPORTS_DIR)/test-report-$(OS)-$(CONFIGURATION).txt
	@echo "" >> $(TEST_REPORTS_DIR)/test-report-$(OS)-$(CONFIGURATION).txt
	@echo "Platform: $(OS)" >> $(TEST_REPORTS_DIR)/test-report-$(OS)-$(CONFIGURATION).txt
	@echo "Configuration: $(CONFIGURATION)" >> $(TEST_REPORTS_DIR)/test-report-$(OS)-$(CONFIGURATION).txt
	@echo "Timestamp: $$(date '+%Y-%m-%d %H:%M:%S')" >> $(TEST_REPORTS_DIR)/test-report-$(OS)-$(CONFIGURATION).txt
	@echo "" >> $(TEST_REPORTS_DIR)/test-report-$(OS)-$(CONFIGURATION).txt
	@echo "Test Results:" >> $(TEST_REPORTS_DIR)/test-report-$(OS)-$(CONFIGURATION).txt
	@echo "=============" >> $(TEST_REPORTS_DIR)/test-report-$(OS)-$(CONFIGURATION).txt
	@echo "" >> $(TEST_REPORTS_DIR)/test-report-$(OS)-$(CONFIGURATION).txt
	@$(DOTNET) test $(SOLUTION) --configuration $(CONFIGURATION) --no-build --results-directory $(TEST_REPORTS_DIR) --logger "console;verbosity=normal" >> $(TEST_REPORTS_DIR)/test-report-$(OS)-$(CONFIGURATION).txt 2>&1
	@echo "$(GREEN)[SUCCESS]$(NC) All tests completed"
	@echo "$(GREEN)[SUCCESS]$(NC) Test report generated: $(TEST_REPORTS_DIR)/test-report-$(OS)-$(CONFIGURATION).txt"

# Coverage targets
.PHONY: coverage
coverage: build ## Run tests with Cobertura coverage format using cross-platform solution
	@echo "$(BLUE)[INFO]$(NC) Running tests with Cobertura coverage using cross-platform solution..."
	@mkdir -p $(COVERAGE_DIR)
	@mkdir -p $(TEST_REPORTS_DIR)
	@$(DOTNET) test $(SOLUTION) --configuration $(CONFIGURATION) --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=$(PWD)/$(COVERAGE_DIR)/coverage.xml --results-directory $(TEST_REPORTS_DIR) --logger "console;verbosity=normal"
	@echo "$(GREEN)[SUCCESS]$(NC) Cobertura coverage tests completed"

# Clean targets
.PHONY: clean
clean: ## Clean build artifacts
	@echo "$(BLUE)[INFO]$(NC) Cleaning build artifacts..."
	@$(DOTNET) clean $(SOLUTION) --configuration $(CONFIGURATION) --verbosity $(VERBOSITY)
	@echo "$(GREEN)[SUCCESS]$(NC) Build artifacts cleaned"

.PHONY: clean-all
clean-all: ## Clean all build artifacts and temporary files
	@echo "$(BLUE)[INFO]$(NC) Cleaning all artifacts..."
	@rm -rf $(REPORTS_DIR)
	@find . -name "bin" -type d -exec rm -rf {} + 2>/dev/null || true
	@find . -name "obj" -type d -exec rm -rf {} + 2>/dev/null || true
	@echo "$(GREEN)[SUCCESS]$(NC) All artifacts cleaned"

# Development targets
.PHONY: watch
watch: watch-test ## Watch and rerun tests on changes (default watch behavior)

.PHONY: watch-build
watch-build: ## Watch and rebuild entire solution on changes
	@echo "$(BLUE)[INFO]$(NC) Watching entire solution for changes..."
	@$(DOTNET) watch --project $(SOLUTION) build

.PHONY: watch-test
watch-test: ## Watch and rerun tests on changes using cross-platform solution
	@echo "$(BLUE)[INFO]$(NC) Watching tests for changes using cross-platform solution..."
	@$(DOTNET) watch --project $(SOLUTION) test


# Release targets
.PHONY: release
release: CONFIGURATION=Release
release: clean build test ## Build and test release version
	@echo "$(GREEN)[SUCCESS]$(NC) Release build completed"

.PHONY: release-coverage
release-coverage: CONFIGURATION=Release
release-coverage: clean coverage ## Build and test release version with coverage
	@echo "$(GREEN)[SUCCESS]$(NC) Release build with coverage completed"

# Utility targets
.PHONY: format
format: ## Format code
	@echo "$(BLUE)[INFO]$(NC) Formatting code..."
	@$(DOTNET) format $(SOLUTION)
	@echo "$(GREEN)[SUCCESS]$(NC) Code formatted"

.PHONY: check-format
check-format: ## Check code formatting
	@echo "$(BLUE)[INFO]$(NC) Checking code formatting..."
	@$(DOTNET) format $(SOLUTION) --verify-no-changes
	@echo "$(GREEN)[SUCCESS]$(NC) Code formatting is correct"

.PHONY: info
info: ## Show project information
	@echo "$(BLUE)[INFO]$(NC) Project Information"
	@echo "=========================="
	@echo "Solution: $(SOLUTION)"
	@echo "Configuration: $(CONFIGURATION)"
	@echo "Verbosity: $(VERBOSITY)"
	@echo "Coverage Directory: $(COVERAGE_DIR)"
	@echo "Test Reports Directory: $(TEST_REPORTS_DIR)"
	@echo "Reports Directory: $(REPORTS_DIR)"
	@echo ""
	@echo "Projects:"
	@echo "  Core Library: $(LIB_PROJECT)"
	@echo "  Shared Library: $(SHARED_PROJECT)"
	@echo "  Platform Library: $(PLATFORM_PROJECT)"
	@echo "  Avalonia App: $(AVALONIA_PROJECT)"
	@echo "  Core Tests: $(LIB_TESTS_PROJECT)"
	@echo "  Avalonia Tests: $(AVALONIA_TESTS_PROJECT)"
	@echo ""
	@echo "Current .NET Version: $$($(DOTNET) --version)"
	@echo "Current Platform: $$(uname -s)"

# Default target when no target is specified
.DEFAULT_GOAL := help
