using System.Collections.Generic;
using System.Collections.Immutable;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Example.Quests.Childs
{
    public abstract class QuestEventConditionTaskNode : QuestConditonNode
    {
        private IImmutableSet<ISubscription> eventSubscriptions = ImmutableHashSet<ISubscription>.Empty;

        protected QuestEventConditionTaskNode(IQuestRootNode rootNode)
            : base(rootNode)
        {
        }

        /// <summary>
        /// Calls to this method trigger a subscription of all needed events for this quest tree.
        /// </summary>
        /// <returns>List of created subscriptions.</returns>
        protected abstract IEnumerable<ISubscription> GetEventSubscriptions();

        protected override void AttachEventListeners()
        {
            this.DetachEventListeners();

            this.eventSubscriptions = this.GetEventSubscriptions().ToImmutableHashSet();
        }

        protected override void DetachEventListeners()
        {
            var subscriptions = this.eventSubscriptions;
            this.eventSubscriptions = ImmutableHashSet<ISubscription>.Empty;

            foreach (var subscription in subscriptions)
            {
                subscription.Dispose();
            }
        }
    }
}
