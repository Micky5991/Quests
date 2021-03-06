using System.Collections.Generic;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Example.Events;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Example.Quests.Childs
{
    public class EnterZoneTask : QuestEventTaskNode
    {
        private readonly int zoneId;

        private readonly IEventAggregator eventAggregator;

        public EnterZoneTask(IQuestRootNode rootNode, int zoneId, IEventAggregator eventAggregator)
            : base(rootNode)
        {
            this.zoneId = zoneId;
            this.eventAggregator = eventAggregator;
            this.Title = $"Reach zone {zoneId}";
        }

        protected override IEnumerable<ISubscription> GetEventSubscriptions()
        {
            yield return this.eventAggregator.Subscribe<EnterZoneEvent>(this.OnEnterZoneEvent);
        }

        private void OnEnterZoneEvent(EnterZoneEvent eventdata)
        {
            if (eventdata.ZoneId != this.zoneId)
            {
                return;
            }

            this.MarkAsSuccess();
        }
    }
}
