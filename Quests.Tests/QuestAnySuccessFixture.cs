using System;
using System.Linq;
using FluentAssertions;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Tests.Entities;
using Micky5991.Quests.Tests.TestBases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;

namespace Micky5991.Quests.Tests;

[TestClass]
public class QuestAnySuccessFixture : QuestTestBase
{
    private DummyTask[] tasks;

    private DummyAnySuccessNode composite;

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

            this.composite = new DummyAnySuccessNode(q);
            foreach (var task in this.tasks)
            {
                this.composite.Add(task);
            }

            return this.composite;
        }, initialize);
    }

    [TestMethod]
    public void QuestNodeStartsWithSleepingState()
    {
        this.composite.Status.Should().Be(QuestStatus.Sleeping);
    }

    [TestMethod]
    public void QuestNodeStartsWithSleepingTasks()
    {
        this.composite.ChildNodes.Should().BeEquivalentTo(this.tasks);
    }

    [TestMethod]
    public void ActivatingQuestWillActivateAllChildNodes()
    {
        this.quest.SetStatus(QuestStatus.Active);

        this.composite.ChildNodes.Should().OnlyContain(x => x.Status == QuestStatus.Active);
    }

    [TestMethod]
    public void FailingASingleTaskWillStillBeActive()
    {
        this.quest.SetStatus(QuestStatus.Active);
        this.tasks.First().ForceSetState(QuestStatus.Failure);

        this.composite.Status.Should().Be(QuestStatus.Active);
        this.quest.Status.Should().Be(QuestStatus.Active);
    }

    [TestMethod]
    public void FailingAllTasksWillFailQuest()
    {
        foreach (var task in this.tasks)
        {
            task.ForceSetState(QuestStatus.Failure);
        }

        this.composite.Status.Should().Be(QuestStatus.Failure);
        this.quest.Status.Should().Be(QuestStatus.Failure);
    }

    [TestMethod]
    public void DeactivatingNodeWillKeepChildQuestsStateSameIfNotActive()
    {
        this.quest.SetStatus(QuestStatus.Active);
        this.tasks.First().ForceSetState(QuestStatus.Failure);
        this.quest.SetStatus(QuestStatus.Sleeping);

        this.tasks.First().Status.Should().Be(QuestStatus.Failure);
        this.tasks.Skip(1).Should().OnlyContain(x => x.Status == QuestStatus.Sleeping);
    }

    [TestMethod]
    public void SucceedingASingleTaskWillSucceedComposite()
    {
        this.quest.SetStatus(QuestStatus.Active);
        this.tasks.First().ForceSetState(QuestStatus.Success);

        this.composite.Status.Should().Be(QuestStatus.Success);
        this.quest.Status.Should().Be(QuestStatus.Success);
    }

}
