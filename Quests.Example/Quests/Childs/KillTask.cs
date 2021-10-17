using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Example.Events;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Example.Quests.Childs
{
    public class KillTask : QuestTaskNode
    {
        public const int RequiredKills = 5;

        private readonly IEventAggregator eventAggregator;

        private int kills;

        public KillTask(IQuestRootNode rootNode, IEventAggregator eventAggregator)
            : base(rootNode)
        {
            this.eventAggregator = eventAggregator;
        }

        public override void Initialize()
        {
            this.UpdateTitle();
        }

        protected override IEnumerable<ISubscription> AttachEventListeners()
        {
            yield return this.eventAggregator.Subscribe<KillEvent>(this.OnPlayerKill);
        }

        private void UpdateTitle()
        {
            this.Title = $"Kill {RequiredKills - this.kills} enemies";
        }

        private void OnPlayerKill(KillEvent eventdata)
        {
            this.kills = Math.Min(RequiredKills, this.kills + 1);

            this.UpdateTitle();

            if (this.kills >= RequiredKills)
            {
                this.MarkAsSuccess();
            }
        }
    }
}
