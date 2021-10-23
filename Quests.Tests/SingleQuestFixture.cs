using System;
using FluentAssertions;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Tests.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;

namespace Micky5991.Quests.Tests;

[TestClass]
public class SingleQuestFixture
{
    private const string QuestTitle = "Example Quest";

    private IServiceProvider serviceProvider;

    private DummyQuest quest;

    private DummyTask ChildNode => (DummyTask) this.quest!.ChildNode!;

    [TestInitialize]
    public void Setup()
    {
        this.serviceProvider = new ServiceCollection()
                               .AddLogging(builder => builder.AddSerilog(Logger.None))
                               .BuildServiceProvider();

        this.quest = new DummyQuest(QuestTitle, q => new DummyTask(q));
        this.quest.Initialize();
    }

    [TestCleanup]
    public void TearDown()
    {
        this.serviceProvider = null;
        this.quest = null!;
    }

    [TestMethod]
    public void TaskShouldStartSleeping()
    {
        this.ChildNode.Status.Should().Be(QuestStatus.Sleeping);
    }

    [TestMethod]
    [DataRow(QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Active)]
    [DataRow(QuestStatus.Failure)]
    public void ActivatingRootNodeActivatesChildNode(QuestStatus newStatus)
    {
        if (newStatus == QuestStatus.Sleeping)
        {
            this.quest!.SetStatus(QuestStatus.Active);
        }
        this.quest!.SetStatus(newStatus);

        this.ChildNode.Status.Should().Be(newStatus);
    }

    [TestMethod]
    public void DisposingQuestWillDisposeChildQuest()
    {
        var childNode = this.ChildNode;

        this.quest!.Dispose();

        this.quest!.Disposed.Should().BeTrue();
        childNode.Disposed.Should().BeTrue();
    }

    [TestMethod]
    public void DisposingQuestTwiceWillThrowException()
    {
        this.quest!.Dispose();

        Action action = () => this.quest!.Dispose();

        action.Should().Throw<ObjectDisposedException>().WithMessage("*quest*");
    }

    [TestMethod]
    public void FailingChildQuestWillPropagateToRoot()
    {
        this.ChildNode.SetStatus(QuestStatus.Failure);

        this.ChildNode.Status.Should().Be(QuestStatus.Failure);
        this.quest!.Status.Should().Be(QuestStatus.Failure);
    }
}
