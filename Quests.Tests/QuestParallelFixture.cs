using System;
using System.Linq;
using FluentAssertions;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;
using Micky5991.Quests.Tests.Entities;
using Micky5991.Quests.Tests.TestBases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;

namespace Micky5991.Quests.Tests
{
    [TestClass]
    public class QuestParallelFixture : QuestTestBase
    {
        private DummyTask[] tasks;

        private DummyParallelNode composite;

        private DummyQuest quest;

        private IServiceProvider serviceProvider;

        [TestInitialize]
        public void Setup()
        {
            this.serviceProvider = new ServiceCollection()
                                   .AddLogging(builder => builder.AddSerilog(Logger.None))
                                   .BuildServiceProvider();

            this.SetupQuests(true);
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                this.quest.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // ignore.
            }

            this.quest = null!;
            this.composite = null!;
            this.tasks = null!;

            this.serviceProvider = null;
        }

        private void SetupQuests(bool initialize)
        {
            this.quest = this.CreateExampleQuest(q =>
            {
                this.tasks = new[]
                {
                    new DummyTask(q),
                    new DummyTask(q),
                    new DummyTask(q),
                    new DummyTask(q),
                    new DummyTask(q),
                };

                this.composite = new DummyParallelNode(q);
                foreach (var task in this.tasks)
                {
                    this.composite.Add(task);
                }

                return this.composite;
            }, initialize);
        }

        [TestMethod]
        public void ActivatingParallelNodeWillActivateAllOtherNodes()
        {
            this.quest.SetStatus(QuestStatus.Active);

            var childNode = this.quest.ChildNode as IQuestCompositeNode;

            childNode!.ChildNodes.All(x => x.Status == QuestStatus.Active).Should().BeTrue();
        }

        [TestMethod]
        public void FailingASingleNodeWillFailComposite()
        {
            this.quest.SetStatus(QuestStatus.Active);

            var childNode = this.quest.ChildNode as IQuestCompositeNode;

            childNode!.ChildNodes[0].SetStatus(QuestStatus.Failure);

            childNode.ChildNodes.All(x => x.Status == QuestStatus.Failure).Should().BeTrue();
            this.quest.ChildNode!.Status.Should().Be(QuestStatus.Failure);
        }

        [TestMethod]
        public void SucceedingAllChildNodesWillSucceedParallel()
        {
            this.quest.SetStatus(QuestStatus.Active);

            var childNode = this.quest.ChildNode as DummyParallelNode;

            foreach (var questChildNode in childNode!.ChildNodes)
            {
                var node = (DummyTask)questChildNode;

                node.ForceSetState(QuestStatus.Success);
            }

            childNode.ChildNodes.All(x => x.Status == QuestStatus.Success).Should().BeTrue();
            this.quest.ChildNode!.Status.Should().Be(QuestStatus.Success);
        }
    }
}
