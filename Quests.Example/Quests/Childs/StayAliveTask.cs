using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Example.Entities;
using Micky5991.Quests.Example.Events;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Example.Quests.Childs;

public class StayAliveTask : QuestEventConditionTaskNode
{
    private readonly IEventAggregator eventAggregator;

    public StayAliveTask(IQuestRootNode rootNode, IEventAggregator eventAggregator)
        : base(rootNode)
    {
        this.eventAggregator = eventAggregator;
        this.Title = "Stay alive during the attack.";
    }

    protected override IEnumerable<ISubscription> GetEventSubscriptions()
    {
        yield return this.eventAggregator.Subscribe<KillEvent>(this.OnEntityKill);
    }

    private void OnEntityKill(KillEvent eventdata)
    {
        if (eventdata.Victim is not Player)
        {
            return;
        }

        this.SetStatus(QuestStatus.Failure);
    }
}
