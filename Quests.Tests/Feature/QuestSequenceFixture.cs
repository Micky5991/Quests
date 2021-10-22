using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.EventAggregator.Services;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Tests.Entities;
using Micky5991.Quests.Tests.TestBases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;

namespace Micky5991.Quests.Tests.Feature;

[TestClass]
public class QuestSequenceFixture : QuestTestBase
{
    private DummyTask[] tasks;

    private DummySequenceNode composite;

    private DummyQuest quest;

    private IEventAggregator? eventAggregator;

    private IServiceProvider? serviceProvider;

    [TestInitialize]
    public void Setup()
    {
        this.serviceProvider = new ServiceCollection()
                               .AddSingleton<IEventAggregator, EventAggregatorService>()
                               .AddLogging(builder => builder.AddSerilog(Logger.None))
                               .BuildServiceProvider();

        this.eventAggregator = this.serviceProvider.GetService<IEventAggregator>()!;

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

        this.eventAggregator = null;
        this.serviceProvider = null;
    }

    private void SetupQuests(bool initialize)
    {
        this.quest = this.CreateExampleQuest(q =>
        {
            this.tasks = new[]
            {
                new DummyTask(q, this.eventAggregator!),
                new DummyTask(q, this.eventAggregator!),
                new DummyTask(q, this.eventAggregator!),
                new DummyTask(q, this.eventAggregator!),
                new DummyTask(q, this.eventAggregator!),
            };

            this.composite = new DummySequenceNode(q);
            foreach (var task in this.tasks)
            {
                this.composite.Add(task);
            }

            return this.composite;
        }, initialize);
    }

    [TestMethod]
    public void ActivateSequenceWillActivateOnlyFirstItem()
    {
        this.quest.SetStatus(QuestStatus.Active);

        var dummySequence = this.quest.ChildNode as DummySequenceNode;

        dummySequence!.ChildNodes[0].Status.Should().Be(QuestStatus.Active);
        dummySequence!.ChildNodes.Skip(1).Should().OnlyContain(x => x.Status == QuestStatus.Sleeping);
    }

    [TestMethod]
    public void SucceedingCurrentActiveNodeWillActivateNext()
    {
        this.quest.SetStatus(QuestStatus.Active);

        var dummySequence = this.quest.ChildNode as DummySequenceNode;
        var taskNodes = dummySequence!.ChildNodes.Select(x => (DummyTask)x).ToList();

        taskNodes[0].ForceSetState(QuestStatus.Success);

        taskNodes[0].Status.Should().Be(QuestStatus.Success);
        taskNodes[1].Status.Should().Be(QuestStatus.Active);

        taskNodes.Skip(2).Should().OnlyContain(x => x.Status == QuestStatus.Sleeping);
    }

    [TestMethod]
    public void SucceedingPrematurelyInBetweenWillKeepFirstActive()
    {
        this.quest.SetStatus(QuestStatus.Active);

        var dummySequence = this.quest.ChildNode as DummySequenceNode;
        var taskNodes = dummySequence!.ChildNodes.Select(x => (DummyTask)x).ToList();

        taskNodes[1].ForceSetState(QuestStatus.Success);

        taskNodes[0].Status.Should().Be(QuestStatus.Active);
        taskNodes[1].Status.Should().Be(QuestStatus.Success);

        taskNodes.Skip(2).Should().OnlyContain(x => x.Status == QuestStatus.Sleeping);
    }

    [TestMethod]
    public void FailASingleTaskWillFailComposite()
    {
        this.quest.SetStatus(QuestStatus.Active);

        var dummySequence = this.quest.ChildNode as DummySequenceNode;
        var taskNodes = dummySequence!.ChildNodes.Select(x => (DummyTask)x).ToList();

        taskNodes[3].ForceSetState(QuestStatus.Failure);

        taskNodes.Should().OnlyContain(x => x.Status == QuestStatus.Failure);
        dummySequence.Status.Should().Be(QuestStatus.Failure);
    }

    [TestMethod]
    public void SucceedingAllSubtasksWillSucceedParent()
    {
        this.quest.SetStatus(QuestStatus.Active);

        var dummySequence = this.quest.ChildNode as DummySequenceNode;
        var taskNodes = dummySequence!.ChildNodes.Select(x => (DummyTask)x).ToList();

        foreach (var taskNode in taskNodes)
        {
            taskNode.ForceSetState(QuestStatus.Success);
        }

        taskNodes.Should().OnlyContain(x => x.Status == QuestStatus.Success);
        dummySequence.Status.Should().Be(QuestStatus.Success);
    }
}
