using System.Collections.Generic;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Tests.Entities;

public class DummyTask : QuestTaskNode
{
    private readonly IEventAggregator eventAggregator;

    public int EventTriggeredAmount { get; private set; }

    public bool Initialized { get; private set; }

    public DummyTask(IQuestRootNode rootNode, IEventAggregator eventAggregator)
        : base(rootNode)
    {
        this.eventAggregator = eventAggregator;
    }

    public override void Initialize()
    {
        base.Initialize();

        this.Initialized = true;
    }

    protected override IEnumerable<ISubscription> GetEventSubscriptions()
    {
        yield return this.eventAggregator.Subscribe<TestEvent>(this.OnTestEvent);
    }

    private void OnTestEvent(TestEvent eventdata)
    {
        this.EventTriggeredAmount += 1;
    }

    public void TriggerSucceed()
    {
        this.MarkAsSuccess();
    }
}
