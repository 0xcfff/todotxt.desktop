using NUnit.Framework;
using TodoTxt.Core.Tasks;
using TodoTxt.Avalonia.Controls;
using Task = TodoTxt.Core.Tasks.Task;

namespace TodoTxt.Avalonia.Tests;

/// <summary>
/// Tests for basic IntellisenseTextBox properties, constructor, and fundamental functionality.
/// </summary>
[TestFixture]
public class IntellisenseTextBoxBasicTests
{
    private TaskList _taskList = null!;

    [SetUp]
    public void Setup()
    {
        // Create TaskList in memory without file dependency
        _taskList = new TaskList();
        
        // Add test tasks directly to the TaskList
        _taskList.Tasks.Add(new Task("Buy groceries +shopping @home"));
        _taskList.Tasks.Add(new Task("Call mom @phone"));
        _taskList.Tasks.Add(new Task("Finish project report +work @office"));
        _taskList.Tasks.Add(new Task("(A) High priority task +important"));
        _taskList.Tasks.Add(new Task("(B) Medium priority task +work"));
        
        // Update metadata for autocompletion
        _taskList.UpdateTaskListMetaData();
    }

    #region Constructor and Basic Properties

    /// <summary>
    /// Verifies that the IntellisenseTextBox constructor sets default property values correctly.
    /// </summary>
    [Test]
    public void IntellisenseTextBoxCtor_WithDefaultValuesProvided_SetsCorrectProperties()
    {
        // arrange
        
        // act
        var textBox = new IntellisenseTextBox();
        
        // assert
        Assert.That(textBox.TaskList, Is.Null);
        Assert.That(textBox.CaseSensitive, Is.False);
    }

    /// <summary>
    /// Verifies that the TaskList property can be set and retrieved correctly.
    /// </summary>
    [Test]
    public void TaskList_WithValidTaskListProvided_SetsProperty()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        
        // act
        textBox.TaskList = _taskList;
        
        // assert
        Assert.That(textBox.TaskList, Is.EqualTo(_taskList));
    }

    /// <summary>
    /// Verifies that the TaskList property can be set to null.
    /// </summary>
    [Test]
    public void TaskList_WithNullValueProvided_SetsToNull()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList; // Set to a valid value first
        
        // act
        textBox.TaskList = null;
        
        // assert
        Assert.That(textBox.TaskList, Is.Null);
    }

    /// <summary>
    /// Verifies that the CaseSensitive property can be set and retrieved correctly.
    /// </summary>
    [Test]
    public void CaseSensitive_WithTrueValueSet_SetsProperty()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        
        // act
        textBox.CaseSensitive = true;
        
        // assert
        Assert.That(textBox.CaseSensitive, Is.True);
    }

    /// <summary>
    /// Verifies that the CaretIndex property can be set and retrieved correctly.
    /// </summary>
    [Test]
    public void CaretIndex_WithValidPositionSet_SetsProperty()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.Text = "Test text";
        
        // act
        textBox.CaretIndex = 5;
        
        // assert
        Assert.That(textBox.CaretIndex, Is.EqualTo(5));
    }

    /// <summary>
    /// Verifies that the Text property can be set and retrieved correctly.
    /// </summary>
    [Test]
    public void Text_WithValidInputProvided_SetsProperty()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        
        // act
        textBox.Text = "Test input";
        
        // assert
        Assert.That(textBox.Text, Is.EqualTo("Test input"));
    }

    #endregion

    #region TaskList Integration

    /// <summary>
    /// Verifies that an empty TaskList is handled correctly.
    /// </summary>
    [Test]
    public void TaskList_WithEmptyTaskListProvided_SetsPropertyAndHasZeroTasks()
    {
        // arrange
        var emptyTaskList = new TaskList();
        var textBox = new IntellisenseTextBox();
        
        // act
        textBox.TaskList = emptyTaskList;
        
        // assert
        Assert.That(textBox.TaskList, Is.EqualTo(emptyTaskList));
        Assert.That(textBox.TaskList.Tasks.Count, Is.EqualTo(0));
    }

    /// <summary>
    /// Verifies that projects are correctly extracted from the TaskList.
    /// </summary>
    [Test]
    public void TaskList_WithValidTasksProvided_ExtractsProjects()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // act
        // The TaskList should have extracted projects from the test tasks
        
        // assert
        Assert.That(_taskList.Projects.Count, Is.GreaterThan(0));
        Assert.That(_taskList.Projects, Contains.Item("+shopping"));
        Assert.That(_taskList.Projects, Contains.Item("+work"));
        Assert.That(_taskList.Projects, Contains.Item("+important"));
    }

    /// <summary>
    /// Verifies that contexts are correctly extracted from the TaskList.
    /// </summary>
    [Test]
    public void TaskList_WithValidTasksProvided_ExtractsContexts()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // act
        // The TaskList should have extracted contexts from the test tasks
        
        // assert
        Assert.That(_taskList.Contexts.Count, Is.GreaterThan(0));
        Assert.That(_taskList.Contexts, Contains.Item("@home"));
        Assert.That(_taskList.Contexts, Contains.Item("@phone"));
        Assert.That(_taskList.Contexts, Contains.Item("@office"));
    }

    /// <summary>
    /// Verifies that priorities are correctly extracted from the TaskList.
    /// </summary>
    [Test]
    public void TaskList_WithValidTasksProvided_ExtractsPriorities()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // act
        // The TaskList should have extracted priorities from the test tasks
        
        // assert
        Assert.That(_taskList.Priorities.Count, Is.GreaterThan(0));
        Assert.That(_taskList.Priorities, Contains.Item("(A)"));
        Assert.That(_taskList.Priorities, Contains.Item("(B)"));
    }

    /// <summary>
    /// Verifies that multiple projects and contexts are correctly extracted from a complex TaskList.
    /// </summary>
    [Test]
    public void TaskList_WithMultipleProjectsAndContextsProvided_ExtractsAllItems()
    {
        // arrange
        var complexTaskList = new TaskList();
        complexTaskList.Tasks.Add(new Task("Task 1 +project1 +project2 @context1 @context2"));
        complexTaskList.Tasks.Add(new Task("Task 2 +project3 @context3"));
        complexTaskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = complexTaskList;
        
        // act
        // The TaskList should have extracted all projects and contexts
        
        // assert
        Assert.That(complexTaskList.Projects.Count, Is.EqualTo(3));
        Assert.That(complexTaskList.Projects, Contains.Item("+project1"));
        Assert.That(complexTaskList.Projects, Contains.Item("+project2"));
        Assert.That(complexTaskList.Projects, Contains.Item("+project3"));
        
        Assert.That(complexTaskList.Contexts.Count, Is.EqualTo(3));
        Assert.That(complexTaskList.Contexts, Contains.Item("@context1"));
        Assert.That(complexTaskList.Contexts, Contains.Item("@context2"));
        Assert.That(complexTaskList.Contexts, Contains.Item("@context3"));
    }

    #endregion
}
