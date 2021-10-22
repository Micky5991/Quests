using System.Diagnostics;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Example.Entities;
using Micky5991.Quests.Example.Events;
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

            var quest = this.questFactory.BuildQuest<WelcomeHomeQuest>();
            quest.SetStatus(QuestStatus.Active);

            player.AddQuest(quest);

            player.PrintQuestGoals();

            Console.WriteLine("ACTION: Player kills enemy");
            this.eventAggregator.Publish(new KillEvent(player, enemy, 1));

            player.PrintQuestGoals();

            Console.WriteLine("ACTION: Player reaches zone 5");

            this.eventAggregator.Publish(new EnterZoneEvent(player, 5));

            player.PrintQuestGoals();

            Console.WriteLine("ACTION: Player kills enemy");
            this.eventAggregator.Publish(new KillEvent(player, enemy, 1));
            this.eventAggregator.Publish(new KillEvent(player, enemy, 1));
            this.eventAggregator.Publish(new KillEvent(player, enemy, 1));
            this.eventAggregator.Publish(new KillEvent(player, enemy, 1));
            this.eventAggregator.Publish(new KillEvent(player, enemy, 1));

            this.eventAggregator.Publish(new KillEvent(player, enemy, 1));
            this.eventAggregator.Publish(new KillEvent(player, enemy, 1));
            this.eventAggregator.Publish(new KillEvent(player, enemy, 1));

            this.eventAggregator.Publish(new KillEvent(enemy, player, 1));

            this.eventAggregator.Publish(new EnterZoneEvent(player, 4));

            player.PrintQuestGoals();
        }
    }
}
