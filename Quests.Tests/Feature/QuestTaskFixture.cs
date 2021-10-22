using System;
using System.Linq;
using FluentAssertions;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.EventAggregator.Services;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;
using Micky5991.Quests.Tests.Entities;
using Micky5991.Quests.Tests.TestBases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;

namespace Micky5991.Quests.Tests.Feature;

[TestClass]
public class QuestTaskFixture : QuestTestBase
{
    private DummyTask task;

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

        this.quest = this.CreateExampleQuest(q =>
        {
            this.task = new DummyTask(q, this.eventAggregator);

            return this.task;
        });
    }

    [TestCleanup]
    public void Cleanup()
    {
        this.quest.Dispose();

        this.quest = null;
        this.task = null;

        this.eventAggregator = null;
        this.serviceProvider = null;
    }

    [TestMethod]
    public void EventsShouldBeRegisteredOnActivate()
    {
        var subscriptions = new ISubscription[]
        {
            new FakeSubscription(),
        };
        this.task.FakeSubscriptions(subscriptions);

        this.quest.SetStatus(QuestStatus.Active);

        this.task.EventSubscriptionAmount.Should().Be(1);
    }

    [TestMethod]
    [DataRow(QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Failure)]
    [DataRow(QuestStatus.Success)]
    public void SubscriptionsWillBeDisposedOnNonActiveStatus(QuestStatus newStatus)
    {
        var subscriptions = new ISubscription[]
        {
            new FakeSubscription(),
        };

        this.task.FakeSubscriptions(subscriptions);
        this.quest.SetStatus(QuestStatus.Active);

        this.task.ForceSetState(newStatus);

        this.task.EventSubscriptionAmount.Should().Be(1);
        subscriptions.All(x => x.IsDisposed).Should().BeTrue();
    }

    [TestMethod]
    [DataRow(QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Failure)]
    [DataRow(QuestStatus.Success)]
    public void SubscriptionsWillNotBeReusedOnStatusChange(QuestStatus newStatus)
    {
        var firstSubscription = new FakeSubscription();
        var secondSubscription = new FakeSubscription();

        this.task.FakeSubscriptions(new ISubscription[]
        {
            firstSubscription,
        });

        this.quest.SetStatus(QuestStatus.Active);
        this.quest.SetStatus(QuestStatus.Sleeping);

        this.task.EventSubscriptionAmount.Should().Be(1);
        firstSubscription.IsDisposed.Should().BeTrue();
        firstSubscription.DisposeAmount.Should().Be(1);

        this.task.FakeSubscriptions(new ISubscription[]
        {
            secondSubscription,
        });

        this.quest.SetStatus(QuestStatus.Active);
        this.quest.SetStatus(QuestStatus.Sleeping);

        this.task.EventSubscriptionAmount.Should().Be(2);
        firstSubscription.IsDisposed.Should().BeTrue();
        firstSubscription.DisposeAmount.Should().Be(1);

        secondSubscription.IsDisposed.Should().BeTrue();
        secondSubscription.DisposeAmount.Should().Be(1);
    }

    [TestMethod]
    [DataRow(QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Failure)]
    [DataRow(QuestStatus.Active)]
    [DataRow(QuestStatus.Success)]
    public void StatusChangedWillNotTriggerOnSameValue(QuestStatus status)
    {
        var amount = 0;

        this.task.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(IQuestNode.Status))
            {
                amount += 1;
            }
        };

        if (status == QuestStatus.Sleeping)
        {
            this.task.SetStatus(QuestStatus.Active);

            amount = 0;
        }

        this.task.ForceSetState(status);
        this.task.ForceSetState(status);

        amount.Should().Be(1);
    }

    [TestMethod]
    [DataRow(QuestStatus.Active, QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Active, QuestStatus.Success)]
    [DataRow(QuestStatus.Active, QuestStatus.Failure)]
    [DataRow(QuestStatus.Sleeping, QuestStatus.Active)]
    [DataRow(QuestStatus.Sleeping, QuestStatus.Success)]
    [DataRow(QuestStatus.Sleeping, QuestStatus.Failure)]
    public void StatusChangeIsPossible(QuestStatus initialStatus, QuestStatus targetStatus)
    {
        this.task.ForceSetState(initialStatus);
        this.task.ForceCanSetState(targetStatus).Should().BeTrue();
    }

    [TestMethod]
    [DataRow(QuestStatus.Failure, QuestStatus.Success)]
    [DataRow(QuestStatus.Failure, QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Failure, QuestStatus.Active)]
    [DataRow(QuestStatus.Success, QuestStatus.Success)]
    [DataRow(QuestStatus.Success, QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Success, QuestStatus.Active)]
    [DataRow(QuestStatus.Sleeping, QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Active, QuestStatus.Active)]
    [DataRow(QuestStatus.Failure, QuestStatus.Failure)]
    [DataRow(QuestStatus.Success, QuestStatus.Success)]
    public void StatusChangeIsNotPossible(QuestStatus initialStatus, QuestStatus targetStatus)
    {
        this.task.ForceSetState(initialStatus);
        this.task.ForceCanSetState(targetStatus).Should().BeFalse();
    }

    [TestMethod]
    public void SuccessStatusCantBeSet()
    {
        this.task.CanSetToStatus(QuestStatus.Success).Should().BeFalse();

        Action action = () => this.task.SetStatus(QuestStatus.Success);

        action.Should().Throw<ArgumentException>().WithMessage("*success*");

        this.task.Status.Should().NotBe(QuestStatus.Success);
    }
}
