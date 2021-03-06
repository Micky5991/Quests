using System;
using System.Linq;
using FluentAssertions;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Tests.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;

namespace Micky5991.Quests.Tests
{
    [TestClass]
    public class SingleNestedQuestFixture
    {
        private const string QuestTitle = "Example Quest";

        private IServiceProvider serviceProvider;

        private DummyQuest quest;

        private QuestSequenceNode ChildNode => (QuestSequenceNode) this.quest!.ChildNode!;

        private DummyTask NestedChildNode => (DummyTask) this.ChildNode.ChildNodes.First();

        [TestInitialize]
        public void Setup()
        {
            this.serviceProvider = new ServiceCollection()
                                   .AddLogging(builder => builder.AddSerilog(Logger.None))
                                   .BuildServiceProvider();

            this.quest = new DummyQuest(QuestTitle,
                                          q => new QuestSequenceNode(q)
                                          {
                                              new DummyTask(q),
                                          });

            this.quest.Initialize();
        }

        [TestCleanup]
        public void TearDown()
        {
            this.serviceProvider = null;
            this.quest = null!;
        }

        [TestMethod]
        public void VerifyCorrectTestProperties()
        {
            this.ChildNode.Should()
                .NotBeNull()
                .And.BeOfType<QuestSequenceNode>();

            this.NestedChildNode.Should()
                .NotBeNull()
                .And.BeOfType<DummyTask>()
                .And.Be(this.ChildNode.ChildNodes.First());
        }

        [TestMethod]
        public void InitializationHasReachedNestedChild()
        {
            this.NestedChildNode.Initialized.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(QuestStatus.Sleeping)]
        [DataRow(QuestStatus.Active)]
        [DataRow(QuestStatus.Failure)]
        public void ActivatingRootWillActivateChildAndNestedChild(QuestStatus newStatus)
        {
            if (newStatus == QuestStatus.Sleeping)
            {
                this.quest!.SetStatus(QuestStatus.Active);
            }

            this.quest!.SetStatus(newStatus);

            this.ChildNode.Status.Should().Be(newStatus);
            this.NestedChildNode.Status.Should().Be(newStatus);
        }

        [TestMethod]
        public void DisposingRootWillDisposeChildAndNestedChild()
        {
            var childNode = this.ChildNode;
            var nestedChildNode = this.NestedChildNode;

            this.quest!.Dispose();

            this.quest!.ChildNode.Should().BeNull();
            childNode.ChildNodes.Should().BeEmpty();

            childNode.Disposed.Should().BeTrue();
            nestedChildNode.Disposed.Should().BeTrue();
        }
    }
}
