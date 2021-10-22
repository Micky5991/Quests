using System;
using System.Linq;
using FluentAssertions;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.EventAggregator.Services;
using Micky5991.Quests.Tests.Entities;
using Micky5991.Quests.Tests.TestBases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;

namespace Micky5991.Quests.Tests;

[TestClass]
public class QuestCompositeFixture : QuestTestBase
{
    private DummyTask[] tasks;

    private DummyCompositeNode composite;

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

            this.composite = new DummyCompositeNode(q);
            foreach (var task in this.tasks)
            {
                this.composite.Add(task);
            }

            return this.composite;
        }, initialize);
    }

    [TestMethod]
    public void AddingNoCompositeNodeWillReturnEmptyChildNodes()
    {
        this.quest = this.CreateExampleQuest(q =>
        {
            this.composite = new DummyCompositeNode(q);

            return this.composite;
        });

        this.composite.ChildNodes.Should().BeEmpty();
    }

    [TestMethod]
    public void AddingNullAsChildNodeThrowsException()
    {
        Action action = () =>
        {
            this.quest = this.CreateExampleQuest(q =>
            {
                this.composite = new DummyCompositeNode(q)
                {
                    null!,
                };

                return this.composite;
            });
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void InitializingQuestInitializesAllChildQuests()
    {
        this.SetupQuests(false);

        this.tasks.Any(x => x.Initialized).Should().BeFalse();

        this.quest.Initialize();

        this.tasks.All(x => x.Initialized).Should().BeTrue();
    }

    [TestMethod]
    public void AddingChildQuestAfterInitWillInitializeNewChildQuest()
    {
        var task = new DummyTask(this.quest, this.eventAggregator!);
        this.composite.Add(task);

        task.Initialized.Should().BeTrue();
    }

    [TestMethod]
    public void AddingChildQuestWithWrongRootNodeWillThrowException()
    {
        var fakeQuest = new DummyQuest("FAKE");

        var task = new DummyTask(fakeQuest, this.eventAggregator!);

        Action action = () => this.composite.Add(task);
        action.Should().Throw<ArgumentException>().WithMessage("*root*");
    }

    [TestMethod]
    public void AddingDisposedChildNodeThrowsException()
    {
        var task = new DummyTask(this.quest, this.eventAggregator!);
        task.Initialize();
        task.Dispose();

        Action action = () => this.composite.Add(task);
        action.Should().Throw<ObjectDisposedException>();
    }

    [TestMethod]
    public void DisposingQuestWillDisposeChildNodes()
    {
        this.quest.Dispose();

        this.tasks.All(x => x.Disposed).Should().BeTrue();
    }

    [TestMethod]
    public void DisposingQuestWillSetDisposedToTrue()
    {
        this.quest.Dispose();

        this.quest.Disposed.Should().BeTrue();
    }
}
