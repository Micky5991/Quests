using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Example.Entities;
using Micky5991.Quests.Example.Quests;
using Micky5991.Quests.Interfaces.Services;

namespace Micky5991.Quests.Example
{
    public class GameLogic
    {
        private readonly IEventAggregator eventAggregator;

        private readonly IQuestFactory questFactory;

        public GameLogic(IEventAggregator eventAggregator, IQuestFactory questFactory)
        {
            this.eventAggregator = eventAggregator;
            this.questFactory = questFactory;
        }

        public void Run()
        {
            var player = new Player();
            var enemy = new Enemy();

            player.AddQuest(this.questFactory.BuildQuest<WelcomeHomeQuest>());

            player.PrintQuestGoals();

            this.eventAggregator.Publish(new KillEvent(player, enemy, 1));

            player.PrintQuestGoals();
        }
    }
}
