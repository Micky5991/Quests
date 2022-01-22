using System.Collections.Generic;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Tests.Entities
{
    public class DummyTask : QuestTaskNode
    {
        public int EventSubscriptionAmount { get; private set; }

        public int EventUnsubscriptionAmount { get; private set; }

        public int OnStatusChangedAmount { get; private set; }

        public DummyTask(IQuestRootNode rootNode)
            : base(rootNode)
        {
        }

        public void ForceSetState(QuestStatus newStatus)
        {
            if (newStatus == QuestStatus.Success)
            {
                this.MarkAsSuccess();
            }
            else
            {
                this.SetStatus(newStatus);
            }
        }

        public bool ForceCanSetState(QuestStatus newStatus)
        {
            if (newStatus == QuestStatus.Success)
            {
                return this.CanMarkAsSuccess();
            }

            return this.CanSetToStatus(newStatus);
        }

        protected override void OnStatusChanged(QuestStatus newStatus)
        {
            this.OnStatusChangedAmount += 1;

            base.OnStatusChanged(newStatus);
        }

        protected override void AttachEventListeners()
        {
            this.EventSubscriptionAmount += 1;
        }

        protected override void DetachEventListeners()
        {
            this.EventUnsubscriptionAmount += 1;
        }
    }
}
