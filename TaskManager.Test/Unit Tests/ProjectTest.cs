using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Test
{
    [TestFixture]
    public class ProjectTest
    {
        private Fixture _fixture = new Fixture();

        [Test]
        public void Register_Project_Raises_Event()
        {
            var title = _fixture.Create<Title>();
            var project = new Project(title);

            var uncommittedEvents = project.GetUncommittedEvents();
            Assert.IsTrue(uncommittedEvents.Contains(new ProjectRegistered(project.Id, title, ProjectPriority.None.DisplayName)));
        }

        [Test]
        public void Register_Project_With_Deadline_Raises_Event()
        {
            var title = _fixture.Create<Title>();
            var deadline = _fixture.Create<Deadline>();
            var project = new Project(title, deadline);
            var uncommittedEvents = project.GetUncommittedEvents();
            Assert.IsTrue(uncommittedEvents.Contains(new ProjectRegistered(project.Id, title, ProjectPriority.None.DisplayName, deadline)));
        }
    }
}