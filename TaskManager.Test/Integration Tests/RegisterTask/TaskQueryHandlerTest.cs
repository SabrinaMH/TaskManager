using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.RegisterTask;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Test.RegisterTask
{
    [TestFixture]
    public class TaskQueryHandlerTest : BaseIntegrationTest
    {
        [Test]
        public void Task_With_Same_Title_Does_Not_Already_Exist_In_Project()
        {
            var doesTaskWithTitleAlreadyExistUnderSameProjectQuery = new DoesTaskWithTitleAlreadyExistUnderSameProjectQuery("my title");
            var taskQueryHandler = new TaskQueryHandler();
            bool doesTaskExist = taskQueryHandler.Handle(doesTaskWithTitleAlreadyExistUnderSameProjectQuery);
            Assert.That(doesTaskExist, Is.False);
        }

        [Test]
        public void Task_With_Same_Title_Does_Already_Exist_In_Project()
        {
            var fixture = new Fixture();
            var title = fixture.Create<string>();
            using (var session = DocumentStore.OpenSession())
            {
                Guid id = fixture.Create<Guid>();
                Guid projectId = fixture.Create<Guid>();
                var taskInGridView = new TaskInGridView(id, projectId, title,
                    fixture.Create<DateTime>().ToShortDateString(), ProjectPriority.Low.DisplayName, true);
                session.Store(taskInGridView);
                session.SaveChanges();
            }

            var doesTaskWithTitleAlreadyExistUnderSameProjectQuery = new DoesTaskWithTitleAlreadyExistUnderSameProjectQuery(title);
            var taskQueryHandler = new TaskQueryHandler();
            bool doesTaskExist = taskQueryHandler.Handle(doesTaskWithTitleAlreadyExistUnderSameProjectQuery);
            Assert.That(doesTaskExist, Is.True);
        }
    }
}