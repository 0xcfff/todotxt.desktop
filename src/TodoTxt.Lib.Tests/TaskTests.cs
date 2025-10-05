using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CollectionAssert = NUnit.Framework.Legacy.CollectionAssert;
using TodoTxt.Core;
using Task = TodoTxt.Core.Task;

namespace ToDoTests
{
    [TestFixture]
    public class TaskTests
    {
        List<string> _projects = new List<string>() { "+test" };
        List<string> _contexts = new List<string>() { "@work" };

        #region Create
        [Test]
        public void TaskCtor_WithPriorityBodyProjectContextProvided_CreatesTaskWithCorrectProperties()
        {
            // arrange
            var taskString = "(A) This is a test task +test @work";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work" }));
            Assert.AreEqual("This is a test task", actual.Body);
        }

        [Test]
        public void TaskCtor_WithPriorityBodyContextProjectProvided_CreatesTaskWithCorrectProperties()
        {
            // arrange
            var taskString = "(A) This is a test task @work +test";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work" }));
            Assert.AreEqual("This is a test task", actual.Body);
        }

        [Test]
        public void TaskCtor_WithTrailingWhitespaceProvided_TrimsWhitespaceAndCreatesTask()
        {
            // arrange
            var taskString = "(A) This is a test task @work +test  ";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work" }));
            Assert.AreEqual("This is a test task", actual.Body);
        }

        [Test]
        public void TaskCtor_WithNoPriorityProvided_CreatesTaskWithEmptyPriority()
        {
            // arrange
            var taskString = "This is a test task @work +test ";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work" }));
            Assert.AreEqual("This is a test task", actual.Body);
        }

        
        [Test]
        public void TaskCtor_WithPriorityInBodyProvided_TreatsPriorityAsPartOfBody()
        {
            // arrange
            var taskString = "Oh (A) This is a test task @work +test ";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work" }));
            Assert.AreEqual("Oh (A) This is a test task", actual.Body);
        }

        [Test]
        public void TaskCtor_WithPriorityContextProjectBodyProvided_CreatesTaskWithCorrectProperties()
        {
            // arrange
            var taskString = "(A) @work +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work" }));
            Assert.AreEqual("This is a test task", actual.Body);
        }

        [Test]
        public void TaskCtor_WithCompletedTaskProvided_CreatesCompletedTask()
        {
            // arrange
            var taskString = "X @work +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual(true, actual.Completed);
        }

        [Test]
        public void TaskCtor_WithCompletedTaskWithDateProvided_SetsCorrectCompletedDate()
        {
            // arrange
            var taskString = "X 2011-05-10 (A) @work +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("2011-05-10", actual.CompletedDate);
        }

        [Test]
        public void TaskCtor_WithUncompletedTaskProvided_CreatesUncompletedTask()
        {
            // arrange
            var taskString = "(A) @work +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual(false, actual.Completed);
        }

        [Test]
        public void TaskCtor_WithMultipleProjectsProvided_CreatesTaskWithAllProjects()
        {
            // arrange
            var taskString = "(A) @work +test +test2 This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test", "+test2" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work" }));
            Assert.AreEqual("This is a test task", actual.Body);
        }

        [Test]
        public void TaskCtor_WithMultipleContextsProvided_CreatesTaskWithAllContexts()
        {
            // arrange
            var taskString = "(A) @work @home +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("This is a test task", actual.Body);
        }

        [Test]
        public void TaskCtor_WithDueDateProvided_CreatesTaskWithCorrectDueDate()
        {
            // arrange
            var taskString = "(A) due:2011-05-08 @work @home +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual("2011-05-08", actual.DueDate);
            Assert.AreEqual(false, actual.Completed);
        }

        [Test]
        public void TaskCtor_WithDueTodayProvided_SetsDueDateToCurrentDate()
        {
            // arrange
            var taskString = "(A) due:today @work @home +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), actual.DueDate);
            Assert.AreEqual(false, actual.Completed);
        }

        [Test]
        public void TaskCtor_WithDueTomorrowProvided_SetsDueDateToTomorrow()
        {
            // arrange
            var taskString = "(A) due:tOmORRoW @work @home +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), actual.DueDate);
            Assert.AreEqual(false, actual.Completed);
        }

        [Test]
        public void TaskCtor_WithDueDayOfWeekProvided_SetsDueDateToNextOccurrence()
        {
            // arrange
            var taskString = "(A) due:thUrsday @work @home +test This is a test task";
            var dueDate = DateTime.Now;
            do
            {
                dueDate = dueDate.AddDays(1);
            } while (!string.Equals(dueDate.ToString("dddd", new CultureInfo("en-US")), "thursday", StringComparison.CurrentCultureIgnoreCase));
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual(dueDate.ToString("yyyy-MM-dd"), actual.DueDate);
            Assert.AreEqual(false, actual.Completed);
        }     

        [Test]
        public void TaskCtor_WithDueDayOfWeekTodayProvided_SetsDueDateToNextWeek()
        {
            // arrange
            var dow = DateTime.Today.ToString("ddd", new CultureInfo("en-US"));
            var taskString = string.Format("(A) due:{0} @work @home +test This is a test task", dow);
            var dueDate = DateTime.Now.AddDays(7);
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual(dueDate.ToString("yyyy-MM-dd"), actual.DueDate);
            Assert.AreEqual(false, actual.Completed);
        }

        [Test]
        public void TaskCtor_WithInvalidDueDayOfWeekProvided_TreatsAsPartOfBody()
        {
            // arrange
            var taskString = "(A) due:thUrsdays @work @home +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("due:thUrsdays  This is a test task", actual.Body);
            Assert.AreEqual("", actual.DueDate);
            Assert.AreEqual(false, actual.Completed);
        }

        [Test]
        public void TaskCtor_WithOverdueKeywordProvided_TreatsAsPartOfBody()
        {
            // arrange
            var taskString = "(A) overdue:thUrsday @work @home +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("overdue:thUrsday  This is a test task", actual.Body);
            Assert.AreEqual("", actual.DueDate);
            Assert.AreEqual(false, actual.Completed);
        }

        [Test]
        public void TaskCtor_WithShortDueDayOfWeekProvided_SetsDueDateToNextOccurrence()
        {
            // arrange
            var taskString = "(A) due:wEd @work @home +test This is a test task";
            var dueDate = DateTime.Now;
            do
            {
                dueDate = dueDate.AddDays(1);
            } while (!string.Equals(dueDate.ToString("dddd", new CultureInfo("en-US")), "wednesday", StringComparison.CurrentCultureIgnoreCase));
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual(dueDate.ToString("yyyy-MM-dd"), actual.DueDate);
            Assert.AreEqual(false, actual.Completed);
        }

        [Test]
        public void TaskCtor_WithThresholdDateProvided_CreatesTaskWithCorrectThresholdDate()
        {
            // arrange
            var taskString = "(A) t:2011-05-08 @work @home +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual("2011-05-08", actual.ThresholdDate);
        }

        [Test]
        public void TaskCtor_WithThresholdTodayProvided_SetsThresholdDateToCurrentDate()
        {
            // arrange
            var taskString = "(A) t:today @work @home +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), actual.ThresholdDate);
        }

        [Test]
        public void TaskCtor_WithThresholdTomorrowProvided_SetsThresholdDateToTomorrow()
        {
            // arrange
            var taskString = "(A) t:tOmORRoW @work @home +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), actual.ThresholdDate);
        }

        [Test]
        public void TaskCtor_WithThresholdDayOfWeekProvided_SetsThresholdDateToNextOccurrence()
        {
            // arrange
            var taskString = "(A) t:thUrsday @work @home +test This is a test task";
            var thresholdDate = DateTime.Now;
            do
            {
                thresholdDate = thresholdDate.AddDays(1);
            } while (!string.Equals(thresholdDate.ToString("dddd", new CultureInfo("en-US")), "thursday", StringComparison.CurrentCultureIgnoreCase));
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual(thresholdDate.ToString("yyyy-MM-dd"), actual.ThresholdDate);
        }

        [Test]
        public void TaskCtor_WithThresholdAndDueDateProvided_CreatesTaskWithBothDates()
        {
            // arrange
            var taskString = "(A) t:2011-05-08 due:2017-11-02 @work @home +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A)", actual.Priority);
            Assert.That(actual.Projects, Is.EquivalentTo(new List<string>() { "+test" }));
            Assert.That(actual.Contexts, Is.EquivalentTo(new List<string>() { "@work", "@home" }));
            Assert.AreEqual("This is a test task", actual.Body);
            Assert.AreEqual("2017-11-02", actual.DueDate);
            Assert.AreEqual("2011-05-08", actual.ThresholdDate);
            Assert.AreEqual(false, actual.Completed);
        }

        [Test]
        public void TaskCtor_WithCreationDateProvided_SetsCorrectCreationDate()
        {
            // arrange
            var taskString = "(A) 2011-05-07 due:2011-05-08 @work @home +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("2011-05-07", actual.CreationDate);
        }

		[Test]
		public void TaskCtor_WithProjectContainingNonAlphaCharactersProvided_CreatesProjectWithSpecialCharacters()
		{
			// arrange
			var taskString = "This is a test task +work&home";
			
			// act
			var actual = new Task(taskString);
			
			// assert
			Assert.AreEqual("+work&home", actual.Projects[0]);
		}
        #endregion

        #region ToString
        [Test]
        public void ToString_WithTaskCreatedFromRawStringProvided_ReturnsOriginalString()
        {
            // arrange
            var taskString = "(A) @work +test This is a test task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual("(A) @work +test This is a test task", actual.ToString());
        }

        [Test]
        public void ToString_WithTaskCreatedFromParametersProvided_ReturnsFormattedString()
        {
            // arrange
            var priority = "(A)";
            var projects = new List<string>() { "+test" };
            var contexts = new List<string>() { "@work" };
            var body = "This is a test task";
            
            // act
            var actual = new Task(priority, projects, contexts, body);
            
            // assert
            Assert.AreEqual("(A) This is a test task +test @work", actual.ToString());
        }
        #endregion

        [Test]
        public void Completed_WithTaskSetToCompleted_AddsXToBeginning()
        {
            // arrange
            var taskString = "A new task";
            
            // act
            var actual = new Task(taskString);
            actual.Completed = true;
            
            // assert
            Assert.IsTrue(actual.ToString().StartsWith("x "));
        }


        #region Test Propery IsTaskDue

        [Test]
        public void IsTaskDue_WithTaskWithoutDueDateProvided_ReturnsNotDue()
        {
            // arrange
            var taskString = "Task with out due date task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual(Due.NotDue, actual.IsTaskDue);
        }

        [Test]
        public void IsTaskDue_WithCompletedTaskWithoutDueDateProvided_ReturnsNotDue()
        {
            // arrange
            var taskString = "x Task Complete with out due date task";
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual(Due.NotDue, actual.IsTaskDue);
        }

        [Test]
        public void IsTaskDue_WithTaskWithFutureDueDateProvided_ReturnsNotDue()
        {
            // arrange
            var futureDate = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");
            var taskString = "Task with future task due:" + futureDate;
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual(Due.NotDue, actual.IsTaskDue);
        }

        [Test]
        public void IsTaskDue_WithCompletedTaskWithFutureDueDateProvided_ReturnsNotDue()
        {
            // arrange
            var futureDate = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");
            var taskString = "x Task Complete with future task due:" + futureDate;
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual(Due.NotDue, actual.IsTaskDue);
        }

        [Test]
        public void IsTaskDue_WithTaskWithTodayDueDateProvided_ReturnsToday()
        {
            // arrange
            var todayDate = DateTime.Now.ToString("yyyy-MM-dd");
            var taskString = "Task with today due:" + todayDate;
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual(Due.Today, actual.IsTaskDue);
        }

        [Test]
        public void IsTaskDue_WithCompletedTaskWithTodayDueDateProvided_ReturnsNotDue()
        {
            // arrange
            var todayDate = DateTime.Now.ToString("yyyy-MM-dd");
            var taskString = "x Task Complete with today due:" + todayDate;
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual(Due.NotDue, actual.IsTaskDue);
        }

        [Test]
        public void IsTaskDue_WithTaskWithOverdueDateProvided_ReturnsOverdue()
        {
            // arrange
            var overdueDate = DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd");
            var taskString = "Task with overdue date due:" + overdueDate;
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual(Due.Overdue, actual.IsTaskDue);
        }

        [Test]
        public void IsTaskDue_WithCompletedTaskWithOverdueDateProvided_ReturnsNotDue()
        {
            // arrange
            var overdueDate = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
            var taskString = "x Task Complete with overdue date due:" + overdueDate;
            
            // act
            var actual = new Task(taskString);
            
            // assert
            Assert.AreEqual(Due.NotDue, actual.IsTaskDue);
        }

        #endregion
    }
}
