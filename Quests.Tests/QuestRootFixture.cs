using System;
using FluentAssertions;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.EventAggregator.Services;
using Micky5991.Quests.Tests.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;

namespace Micky5991.Quests.Tests;

[TestClass]
public class QuestRootFixture
{
    private const string QuestTitle = "Example Quest";

    private IEventAggregator? eventAggregator;

    private IServiceProvider? serviceProvider;

    private DummyQuest? quest;

    [TestInitialize]
    public void Setup()
    {
        this.serviceProvider = new ServiceCollection()
                               .AddSingleton<IEventAggregator, EventAggregatorService>()
                               .AddLogging(builder => builder.AddSerilog(Logger.None))
                               .BuildServiceProvider();

        this.eventAggregator = this.serviceProvider.GetService<IEventAggregator>()!;

        this.quest = new DummyQuest(QuestTitle, q => new DummyTask(q, this.eventAggregator));
        this.quest.Initialize();
    }

    [TestCleanup]
    public void TearDown()
    {
        this.serviceProvider = null;
    }

    [TestMethod]
    public void QuestInstantiationWillSetChildNodeProperty()
    {
        this.quest!.ChildNode.Should().NotBeNull().And.BeOfType<DummyTask>();
    }

    [TestMethod]
    public void QuestInitializatonWorks()
    {
        this.quest!.Initialized.Should().BeTrue();
        this.quest!.Blackboard.Should().NotBeNull().And.BeEmpty();

        this.quest!.Title.Should().Be(QuestTitle);
    }

    [TestMethod]
    public void PassingNullAsQuestNodeThrowsException()
    {
        Func<DummyQuest> function = () => new DummyQuest(QuestTitle, _ => null!);

        function.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void InitializingWithoutChildQuestWillJustWork()
    {
        Func<DummyQuest> function = () => new DummyQuest(QuestTitle);

        function.Should().NotThrow();
    }
}
