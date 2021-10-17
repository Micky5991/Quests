using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Example.Entities;
using Micky5991.Quests.Example.Quests;
using Micky5991.Quests.Interfaces.Services;

namespace Micky5991.Quests.Example
{
    public class GameLogic
    {
        private readonly IQuestFactory questFactory;

        private readonly IEventAggregator eventAggregator;

        public GameLogic(IQuestFactory questFactory, IEventAggregator eventAggregator)
        {
            this.questFactory = questFactory;
            this.eventAggregator = eventAggregator;
        }

        public void Run()
        {
            var player = new Player();
            var enemy = new Enemy();

            var killQuest = this.questFactory.BuildQuest<KillQuest>();
            killQuest.TransitionTo(QuestStatus.Active);

            player.AddQuest(killQuest);
            player.PrintQuestGoals();

            this.eventAggregator.Publish(new KillEvent(player, enemy, 1));

            player.PrintQuestGoals();
        }
    }
}
