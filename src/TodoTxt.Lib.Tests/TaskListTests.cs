using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TodoTxt.Core;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Task = TodoTxt.Core.Task;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace ToDoTests
{
    [TestFixture]
    class TaskListTests
    {

		[OneTimeSetUp]
		public void TFSetup()
		{
			if (!File.Exists(Data.TestDataPath))
				File.WriteAllText(Data.TestDataPath, "");
		}

		[OneTimeTearDown]
		public void TearDown()
		{
			if (File.Exists(Data.TestDataPath))
				File.Delete(Data.TestDataPath);
		}
        
		[Test]
        public void TaskListCtor_WithValidFilePathProvided_CreatesTaskListInstance()
        {
            // arrange
            var filePath = Data.TestDataPath;
            
            // act
            var actual = new TaskList(filePath);
            
            // assert
            Assert.IsNotNull(actual);
        }


        [Test]
        public void Tasks_WithTaskListCreatedFromFileProvided_ReturnsTaskCollection()
        {
            // arrange
            var filePath = Data.TestDataPath;
            
            // act
            var taskList = new TaskList(filePath);
            var actual = taskList.Tasks;
            
            // assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public void Add_WithValidTaskProvided_AddsTaskToCollection()
        {
            // arrange
            var taskString = "(B) Add_ToCollection +test @task";
            var task = new Task(taskString);
            var taskList = new TaskList(Data.TestDataPath);
            var initialCount = taskList.Tasks.Count();
            
            // act
            taskList.Add(task);
            
            // assert
            Assert.AreEqual(initialCount + 1, taskList.Tasks.Count());
            Assert.That(taskList.Tasks.ToList(), Contains.Item(task));
        }

        [Test]
        public void Add_WithValidTaskProvided_WritesTaskToFile()
        {
            // arrange
            var fileContents = File.ReadAllLines(Data.TestDataPath).ToList();
            var taskString = "(B) Add_ToFile +test @task";
            fileContents.Add(taskString);
            var task = new Task(taskString);
            var taskList = new TaskList(Data.TestDataPath);
            
            // act
            taskList.Add(task);
            
            // assert
            var newFileContents = File.ReadAllLines(Data.TestDataPath);
            Assert.That(newFileContents, Is.EquivalentTo(fileContents));
        }

        [Test]
        public void Add_WithTaskAddedToEmptyFileProvided_AddsTaskSuccessfully()
        {
            // arrange
            // v0.3 and earlier contained a bug where a blank task was added
            File.WriteAllLines(Data.TestDataPath, new string[] { }); // empties the file
            var taskString = "A task";
            var task = new Task(taskString);
            var taskList = new TaskList(Data.TestDataPath);
            
            // act
            taskList.Add(task);
            
            // assert
            Assert.AreEqual(1, taskList.Tasks.Count());
        }

        [Test]
        public void Add_WithMultipleTasksProvided_AddsAllTasksToCollection()
        {
            // arrange
            var taskList = new TaskList(Data.TestDataPath);
            var initialCount = taskList.Tasks.Count();
            var task1 = new Task("Add_Multiple task one");
            var task2 = new Task("Add_Multiple task two");
            
            // act
            taskList.Add(task1);
            taskList.Add(task2);
            
            // assert
            Assert.AreEqual(initialCount + 2, taskList.Tasks.Count());
        }

        [Test]
        public void Delete_WithExistingTaskProvided_RemovesTaskFromCollection()
        {
            // arrange
            var taskString = "(B) Delete_InCollection +test @task";
            var task = new Task(taskString);
            var taskList = new TaskList(Data.TestDataPath);
            taskList.Add(task);
            var countAfterAdd = taskList.Tasks.Count();
            
            // act
            taskList.Delete(task);
            
            // assert
            Assert.AreEqual(countAfterAdd - 1, taskList.Tasks.Count());
            Assert.IsFalse(taskList.Tasks.Contains(task));
        }

        [Test]
        public void Delete_WithExistingTaskProvided_RemovesTaskFromFile()
        {
            // arrange
            var fileContents = File.ReadAllLines(Data.TestDataPath).ToList();
            var task = new Task(fileContents.Last());
            fileContents.Remove(fileContents.Last());
            var taskList = new TaskList(Data.TestDataPath);
            
            // act
            taskList.Delete(task);
            
            // assert
            var newFileContents = File.ReadAllLines(Data.TestDataPath);
            Assert.That(newFileContents, Is.EquivalentTo(fileContents));
        }

        [Test]
        public void Update_WithTaskAndUpdatedTaskProvided_UpdatesTaskInCollection()
        {
            // arrange
            var taskString = "(B) Update_InCollection +test @task";
            var originalTask = new Task(taskString);
            var taskList = new TaskList(Data.TestDataPath);
            taskList.Add(originalTask);
            var updatedTask = new Task(originalTask.Raw);
            updatedTask.Completed = true;
            
            // act
            taskList.Update(originalTask, updatedTask);
            
            // assert
            var newTask = taskList.Tasks.Last();
            Assert.IsTrue(newTask.Completed);
        }

		[Test]
		public void ReloadTasks_WithFileOpenInAnotherProcessProvided_HandlesFileAccessGracefully()
		{
			// arrange
			var taskList = new TaskList(Data.TestDataPath);
			var thread = new Thread(x =>
				{
					try
					{
						var f = File.Open(Data.TestDataPath, FileMode.Open, FileAccess.ReadWrite);
						using (var s = new StreamWriter(f))
						{
							s.WriteLine("hello");
							s.Flush();
						}
						Thread.Sleep(500);
					}
					catch (Exception ex)
					{
						Console.WriteLine("Exception while opening in background thread " + ex.Message);
					}
				});

			thread.Start();
			Thread.Sleep(100);

			// act & assert
			try
			{
				taskList.ReloadTasks();
				// If we get here without exception, the test passes
			}
			catch (Exception ex)
			{
				Assert.Fail($"ReloadTasks failed with exception: {ex.Message}");
			}
			finally
			{
				thread.Join();
			}
		}

        private List<Task> getTestList()
        {
            var tl = new List<Task>();
            tl.Add(new Task("(c) 3test +test2 due:2000-01-03"));//0
            tl.Add(new Task("(d) 1test +test1 @test1 due:2000-01-01"));//1
            tl.Add(new Task("x test XXXXXX "));//2
            tl.Add(new Task("x test xxxxxx due:2000-01-01"));//3
            tl.Add(new Task("x test XXXXXX yyyyyy"));//4
            tl.Add(new Task("x (a) test YYYYYY"));//5
            tl.Add(new Task("(b) 2test +test1 @test2 "));//6
            return tl;
        }

    }
}
