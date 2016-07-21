using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Models.Project;
using TaskManager.ProjectTreeViewUI;

namespace TaskManager.Test
{
    [TestFixture]
    public class TreeUtilsTest
    {
        [Test]
        public void Projects_Are_Sorted_By_Priority_In_Tree()
        {
            var fixture = new Fixture();
            var treeView = new TreeView();
            var treeUtils = new TreeUtils(treeView);
            var projects = new List<ProjectTreeNode>();
            var projectWithLowPriorityAndTitleStartingWithA = new ProjectTreeNode(fixture.Create<string>(), "ABC",
                fixture.Create<DateTime>().ToString(), ProjectPriority.Low.DisplayName, 0);
            var projectWithLowPriorityAndTitleStartingWithB = new ProjectTreeNode(fixture.Create<string>(), "BC",
                fixture.Create<DateTime>().ToString(), ProjectPriority.Low.DisplayName, 0);
            var projectWithMediumPriority = new ProjectTreeNode(fixture.Create<string>(), fixture.Create<string>(),
                fixture.Create<DateTime>().ToString(), ProjectPriority.Medium.DisplayName, 0);
            var projectWithNoPriority = new ProjectTreeNode(fixture.Create<string>(), fixture.Create<string>(),
                fixture.Create<DateTime>().ToString(), ProjectPriority.None.DisplayName, 0);
            projects.Add(projectWithLowPriorityAndTitleStartingWithA);
            projects.Add(projectWithLowPriorityAndTitleStartingWithB);
            projects.Add(projectWithMediumPriority);
            projects.Add(projectWithNoPriority);
            
            treeUtils.PopulateTreeByProjectPriority(projects);

            TreeNodeCollection nodes = treeView.Nodes;
            Assert.That(nodes[0].Text, Is.EqualTo(projectWithMediumPriority.Title));
            Assert.That(nodes[1].Text, Is.EqualTo(projectWithLowPriorityAndTitleStartingWithA.Title));
            Assert.That(nodes[2].Text, Is.EqualTo(projectWithLowPriorityAndTitleStartingWithB.Title));
            Assert.That(nodes[3].Text, Is.EqualTo(projectWithNoPriority.Title));

        }
    }
}