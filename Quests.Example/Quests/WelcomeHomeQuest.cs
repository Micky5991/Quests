using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Example.Quests.Childs;

namespace Micky5991.Quests.Example.Quests;

public class WelcomeHomeQuest : QuestRootNode
{
    public WelcomeHomeQuest(IEventAggregator eventAggregator)
    {
        this.Title = "In The Beginning";

        this.SetChildQuests(
                            new QuestSequenceNode(this)
                            {
                                new EnterZoneTask(this, 5, eventAggregator),
                                new QuestParallelNode(this)
                                {
                                    new StayAliveTask(this, eventAggregator),
                                    new KillTask(this, eventAggregator),
                                },
                                new LeaveZoneTask(this, 5, eventAggregator),
                            });
    }
}
