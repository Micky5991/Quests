using System;
using System.Collections.Generic;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Example.Entities;
using Micky5991.Quests.Example.Events;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Example.Quests.Childs
{
    public class KillTask : QuestEventTaskNode
    {
        public const int RequiredKills = 5;

        private readonly IEventAggregator eventAggregator;

        private int kills;

        public KillTask(IQuestRootNode rootNode, IEventAggregator eventAggregator)
            : base(rootNode)
        {
            this.eventAggregator = eventAggregator;

            this.UpdateTitle();
        }

        protected override IEnumerable<ISubscription> GetEventSubscriptions()
        {
            yield return this.eventAggregator.Subscribe<KillEvent>(this.OnPlayerKill);
        }

        private void UpdateTitle()
        {
            this.Title = $"Kill {RequiredKills - this.kills} enemies";
        }

        private void OnPlayerKill(KillEvent eventdata)
        {
            if (eventdata.Killer is not Player)
            {
                return;
            }

            this.kills = Math.Min(RequiredKills, this.kills + 1);

            this.UpdateTitle();

            if (this.kills >= RequiredKills)
            {
                this.MarkAsSuccess();
            }
        }
    }
}
