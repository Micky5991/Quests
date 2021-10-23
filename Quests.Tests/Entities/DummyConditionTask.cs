using System.Collections.Generic;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Tests.Entities;

public class DummyConditionTask : QuestConditonNode
{
    private readonly IEventAggregator eventAggregator;

    private IList<ISubscription> fakeSubscriptions;

    public int EventSubscriptionAmount { get; private set; }

    public int EventTriggeredAmount { get; private set; }

    public int OnStatusChangedAmount { get; private set; }

    public DummyConditionTask(IQuestRootNode rootNode, IEventAggregator eventAggregator)
        : base(rootNode)
    {
        this.eventAggregator = eventAggregator;
    }

    protected override IEnumerable<ISubscription> GetEventSubscriptions()
    {
        this.EventSubscriptionAmount += 1;

        if (this.fakeSubscriptions != null)
        {
            foreach (var fakeSubscription in this.fakeSubscriptions)
            {
                yield return fakeSubscription;
            }

            yield break;
        }

        yield return this.eventAggregator.Subscribe<TestEvent>(this.OnTestEvent);
    }

    private void OnTestEvent(TestEvent eventdata)
    {
        this.EventTriggeredAmount += 1;
    }

    public void ForceSetState(QuestStatus newStatus)
    {
        if (newStatus == QuestStatus.Success)
        {
            this.MarkAsSuccess();
        }
        else
        {
            this.SetStatus(newStatus);
        }
    }

    public bool ForceCanSetState(QuestStatus newStatus)
    {
        if (newStatus == QuestStatus.Success)
        {
            return this.CanMarkAsSuccess();
        }

        return this.CanSetToStatus(newStatus);
    }

    public void FakeSubscriptions(IList<ISubscription> newFakeSubscriptions)
    {
        this.fakeSubscriptions = newFakeSubscriptions;
    }

    protected override void OnStatusChanged(QuestStatus newStatus)
    {
        this.OnStatusChangedAmount += 1;

        base.OnStatusChanged(newStatus);
    }
}
