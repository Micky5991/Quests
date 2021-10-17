using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Example.Quests.Childs;

namespace Micky5991.Quests.Example.Quests;

public class WelcomeHomeQuest : QuestRootNode
{
    public WelcomeHomeQuest(IEventAggregator eventAggregator)
    {
        this.Title = "Welcome Home";

        this.SetChildQuests(
                            new QuestParallelNode(this)
                            {
                                new QuestSequenceNode(this)
                                {
                                    new KillTask(this, eventAggregator),
                                    new KillTask(this, eventAggregator),
                                    new KillTask(this, eventAggregator),
                                    new KillTask(this, eventAggregator),
                                },
                                new QuestParallelNode(this)
                                {
                                    new KillTask(this, eventAggregator),
                                    new KillTask(this, eventAggregator),
                                    new KillTask(this, eventAggregator),
                                    new KillTask(this, eventAggregator),
                                },
                            });
    }
}
