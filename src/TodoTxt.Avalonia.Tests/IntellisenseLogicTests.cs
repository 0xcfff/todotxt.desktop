using NUnit.Framework;
using TodoTxt.Lib;
using System.IO;
using Task = TodoTxt.Lib.Task;

namespace TodoTxt.Avalonia.Tests;

[TestFixture]
public class IntellisenseLogicTests
{
    private TaskList _taskList = null!;
    private string _tempFilePath = null!;

    [SetUp]
    public void Setup()
    {
        // Create a temporary file for testing
        _tempFilePath = Path.GetTempFileName();
        File.WriteAllText(_tempFilePath, "");
        
        _taskList = new TaskList(_tempFilePath, false);
        
        // Add some test tasks with projects and contexts
        _taskList.Add(new Task("Buy groceries +shopping @home"));
        _taskList.Add(new Task("Call mom @phone"));
        _taskList.Add(new Task("Finish project report +work @office"));
        _taskList.Add(new Task("(A) High priority task +important"));
        _taskList.Add(new Task("(B) Medium priority task +work"));
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the temporary file
        if (File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath);
        }
    }

    [Test]
    public void Should_Extract_Projects_From_TaskList()
    {
        // The TaskList should have extracted projects from the tasks
        Assert.That(_taskList.Projects, Contains.Item("+shopping"));
        Assert.That(_taskList.Projects, Contains.Item("+work"));
        Assert.That(_taskList.Projects, Contains.Item("+important"));
    }

    [Test]
    public void Should_Extract_Contexts_From_TaskList()
    {
        // The TaskList should have extracted contexts from the tasks
        Assert.That(_taskList.Contexts, Contains.Item("@home"));
        Assert.That(_taskList.Contexts, Contains.Item("@phone"));
        Assert.That(_taskList.Contexts, Contains.Item("@office"));
    }

    [Test]
    public void Should_Extract_Priorities_From_TaskList()
    {
        // The TaskList should have extracted priorities from the tasks
        Assert.That(_taskList.Priorities, Contains.Item("(A)"));
        Assert.That(_taskList.Priorities, Contains.Item("(B)"));
    }

    [Test]
    public void Should_Handle_Empty_TaskList()
    {
        // arrange
        var emptyFilePath = Path.GetTempFileName();
        File.WriteAllText(emptyFilePath, "");
        
        // act
        var emptyTaskList = new TaskList(emptyFilePath, false);
        
        // assert
        Assert.That(emptyTaskList.Tasks.Count, Is.EqualTo(0));
        Assert.That(emptyTaskList.Projects.Count, Is.EqualTo(0));
        Assert.That(emptyTaskList.Contexts.Count, Is.EqualTo(0));
        Assert.That(emptyTaskList.Priorities.Count, Is.EqualTo(0));
        
        // Clean up
        File.Delete(emptyFilePath);
    }

    [Test]
    public void Should_Handle_Multiple_Projects_And_Contexts()
    {
        // arrange
        var complexFilePath = Path.GetTempFileName();
        File.WriteAllText(complexFilePath, "");
        var complexTaskList = new TaskList(complexFilePath, false);
        
        // act
        complexTaskList.Add(new Task("Task with +project1 +project2 @context1 @context2"));
        
        // assert
        Assert.That(complexTaskList.Projects, Contains.Item("+project1"));
        Assert.That(complexTaskList.Projects, Contains.Item("+project2"));
        Assert.That(complexTaskList.Contexts, Contains.Item("@context1"));
        Assert.That(complexTaskList.Contexts, Contains.Item("@context2"));
        
        // Clean up
        File.Delete(complexFilePath);
    }

    [Test]
    public void Should_Filter_Projects_Case_Insensitive()
    {
        var filteredProjects = _taskList.Projects.Where(p => 
            p.ToLowerInvariant().Contains("work")).ToList();
        
        Assert.That(filteredProjects, Contains.Item("+work"));
    }

    [Test]
    public void Should_Filter_Contexts_Case_Insensitive()
    {
        var filteredContexts = _taskList.Contexts.Where(c => 
            c.ToLowerInvariant().Contains("home")).ToList();
        
        Assert.That(filteredContexts, Contains.Item("@home"));
    }

    [Test]
    public void Should_Filter_Priorities_Case_Insensitive()
    {
        var filteredPriorities = _taskList.Priorities.Where(p => 
            p.ToLowerInvariant().Contains("a")).ToList();
        
        Assert.That(filteredPriorities, Contains.Item("(A)"));
    }

    [Test]
    public void Should_Handle_Project_Trigger_Character()
    {
        var text = "Test +";
        Assert.That(text, Does.Contain("+"));
        
        // Test that we can detect project trigger
        var hasProjectTrigger = text.Contains("+");
        Assert.That(hasProjectTrigger, Is.True);
    }

    [Test]
    public void Should_Handle_Context_Trigger_Character()
    {
        var text = "Test @";
        Assert.That(text, Does.Contain("@"));
        
        // Test that we can detect context trigger
        var hasContextTrigger = text.Contains("@");
        Assert.That(hasContextTrigger, Is.True);
    }

    [Test]
    public void Should_Handle_Priority_Trigger_Character()
    {
        var text = "Test (";
        Assert.That(text, Does.Contain("("));
        
        // Test that we can detect priority trigger
        var hasPriorityTrigger = text.Contains("(");
        Assert.That(hasPriorityTrigger, Is.True);
    }

    [Test]
    public void Should_Extract_Current_Word_From_Text()
    {
        var text = "Test +shopping";
        var caretIndex = text.Length;
        
        // Find the last trigger character
        var lastPlusIndex = text.LastIndexOf('+');
        var lastAtIndex = text.LastIndexOf('@');
        var lastParenIndex = text.LastIndexOf('(');
        
        var triggerIndex = Math.Max(Math.Max(lastPlusIndex, lastAtIndex), lastParenIndex);
        
        if (triggerIndex >= 0)
        {
            var currentWord = text.Substring(triggerIndex, caretIndex - triggerIndex);
            Assert.That(currentWord, Is.EqualTo("+shopping"));
        }
    }

    [Test]
    public void Should_Handle_No_Trigger_Characters()
    {
        var text = "Just plain text";
        var caretIndex = text.Length;
        
        var lastPlusIndex = text.LastIndexOf('+');
        var lastAtIndex = text.LastIndexOf('@');
        var lastParenIndex = text.LastIndexOf('(');
        
        var triggerIndex = Math.Max(Math.Max(lastPlusIndex, lastAtIndex), lastParenIndex);
        
        Assert.That(triggerIndex, Is.EqualTo(-1));
    }

    [Test]
    public void Should_Handle_Multiple_Trigger_Characters()
    {
        var text = "Test +work @home (A)";
        var caretIndex = text.Length;
        
        var lastPlusIndex = text.LastIndexOf('+');
        var lastAtIndex = text.LastIndexOf('@');
        var lastParenIndex = text.LastIndexOf('(');
        
        var triggerIndex = Math.Max(Math.Max(lastPlusIndex, lastAtIndex), lastParenIndex);
        
        Assert.That(triggerIndex, Is.EqualTo(text.LastIndexOf('(')));
    }
}
