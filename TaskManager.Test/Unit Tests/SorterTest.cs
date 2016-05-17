using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Models.Project;
using TaskManager.ProjectTreeViewUI;

namespace TaskManager.Test
{
    [TestFixture]
    public class SorterTest
    {
        [Test]
        public void Projects_Are_Sorted_By_Priority()
        {
            var fixture = new Fixture();
            var sorter = new Sorter();
            string deadline = DateTime.UtcNow.ToString();
            var title = fixture.Create<string>();
            var node1 = new ProjectTreeNode(fixture.Create<string>(), title, deadline, ProjectPriority.None.DisplayName);
            var node2 = new ProjectTreeNode(fixture.Create<string>(), title, deadline, ProjectPriority.High.DisplayName);
            var node3 = new ProjectTreeNode(fixture.Create<string>(), title, deadline, ProjectPriority.Medium.DisplayName);
            var treeNodes = new List<ProjectTreeNode> {node1, node2, node3};

            var sortedTreeNodes = sorter.ByPriority(treeNodes);

            Assert.That(sortedTreeNodes[0].Priority, Is.EqualTo(ProjectPriority.High.DisplayName));
            Assert.That(sortedTreeNodes[1].Priority, Is.EqualTo(ProjectPriority.Medium.DisplayName));
            Assert.That(sortedTreeNodes[2].Priority, Is.EqualTo(ProjectPriority.None.DisplayName));
        }

        [Test]
        public void Projects_With_Same_Priority_Are_Sorted_On_Title()
        {
              var fixture = new Fixture();
            var sorter = new Sorter();
            string deadline = DateTime.UtcNow.ToString();
            var node1 = new ProjectTreeNode(fixture.Create<string>(), "abc", deadline, ProjectPriority.None.DisplayName);
            var node2 = new ProjectTreeNode(fixture.Create<string>(), "abb", deadline, ProjectPriority.Medium.DisplayName);
            var node3 = new ProjectTreeNode(fixture.Create<string>(), "aad", deadline, ProjectPriority.High.DisplayName);
            var treeNodes = new List<ProjectTreeNode> {node1, node2, node3};

            var sortedTreeNodes = sorter.ByPriority(treeNodes);

            Assert.That(sortedTreeNodes[0].Title, Is.EqualTo("aad"));
            Assert.That(sortedTreeNodes[1].Title, Is.EqualTo("abb"));
            Assert.That(sortedTreeNodes[2].Title, Is.EqualTo("abc"));
        }

    }
}