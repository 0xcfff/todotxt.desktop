using NUnit.Framework;
using ToDoLib;
using TodoTxt.Avalonia.Controls;
using Task = ToDoLib.Task;

namespace TodoTxt.Avalonia.Tests;

[TestFixture]
public class IntellisenseTextBoxUnitTests
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

    [Test]
    public void Should_Initialize_With_Default_Values()
    {
        var textBox = new IntellisenseTextBox();
        
        Assert.That(textBox.TaskList, Is.Null);
        Assert.That(textBox.CaseSensitive, Is.False);
    }

    [Test]
    public void Should_Set_TaskList_Property()
    {
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        Assert.That(textBox.TaskList, Is.EqualTo(_taskList));
    }

    [Test]
    public void Should_Set_CaseSensitive_Property()
    {
        var textBox = new IntellisenseTextBox();
        textBox.CaseSensitive = true;
        
        Assert.That(textBox.CaseSensitive, Is.True);
    }

    [Test]
    public void Should_Handle_Null_TaskList()
    {
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = null;
        
        Assert.That(textBox.TaskList, Is.Null);
    }

    [Test]
    public void Should_Handle_Empty_TaskList()
    {
        var emptyTaskList = new TaskList();
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = emptyTaskList;
        
        Assert.That(textBox.TaskList, Is.EqualTo(emptyTaskList));
        Assert.That(textBox.TaskList.Tasks.Count, Is.EqualTo(0));
    }

    [Test]
    public void Should_Extract_Projects_From_TaskList()
    {
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // The TaskList should have extracted projects from the tasks
        Assert.That(_taskList.Projects, Contains.Item("+shopping"));
        Assert.That(_taskList.Projects, Contains.Item("+work"));
        Assert.That(_taskList.Projects, Contains.Item("+important"));
    }

    [Test]
    public void Should_Extract_Contexts_From_TaskList()
    {
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // The TaskList should have extracted contexts from the tasks
        Assert.That(_taskList.Contexts, Contains.Item("@home"));
        Assert.That(_taskList.Contexts, Contains.Item("@phone"));
        Assert.That(_taskList.Contexts, Contains.Item("@office"));
    }

    [Test]
    public void Should_Extract_Priorities_From_TaskList()
    {
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // The TaskList should have extracted priorities from the tasks
        Assert.That(_taskList.Priorities, Contains.Item("(A)"));
        Assert.That(_taskList.Priorities, Contains.Item("(B)"));
    }

    [Test]
    public void Should_Handle_Text_Input()
    {
        var textBox = new IntellisenseTextBox();
        textBox.Text = "Test input";
        
        Assert.That(textBox.Text, Is.EqualTo("Test input"));
    }

    [Test]
    public void Should_Handle_Caret_Position()
    {
        var textBox = new IntellisenseTextBox();
        textBox.Text = "Hello World";
        textBox.CaretIndex = 5;
        
        Assert.That(textBox.CaretIndex, Is.EqualTo(5));
    }

    [Test]
    public void Should_Handle_Project_Trigger_Character()
    {
        var textBox = new IntellisenseTextBox();
        textBox.Text = "Test +";
        
        Assert.That(textBox.Text, Does.Contain("+"));
    }

    [Test]
    public void Should_Handle_Context_Trigger_Character()
    {
        var textBox = new IntellisenseTextBox();
        textBox.Text = "Test @";
        
        Assert.That(textBox.Text, Does.Contain("@"));
    }

    [Test]
    public void Should_Handle_Priority_Trigger_Character()
    {
        var textBox = new IntellisenseTextBox();
        textBox.Text = "Test (";
        
        Assert.That(textBox.Text, Does.Contain("("));
    }

    [Test]
    public void Should_Handle_Multiple_Projects_And_Contexts()
    {
        var complexTaskList = new TaskList();
        complexTaskList.Tasks.Add(new Task("Task with +project1 +project2 @context1 @context2"));
        complexTaskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = complexTaskList;
        
        Assert.That(complexTaskList.Projects, Contains.Item("+project1"));
        Assert.That(complexTaskList.Projects, Contains.Item("+project2"));
        Assert.That(complexTaskList.Contexts, Contains.Item("@context1"));
        Assert.That(complexTaskList.Contexts, Contains.Item("@context2"));
    }
}
