using System.ComponentModel;
using System.Linq;
using Dawn;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities
{
    /// <summary>
    /// Type that holds multiple child nodes and activates them all at once on node activation. As soon as the first child
    /// node signals a success, this node will also mark as success. When no other succeedable nodes are in this composite,
    /// this node will be marked as failure.
    /// </summary>
    public class QuestAnySuccessNode : QuestCompositeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestAnySuccessNode"/> class.
        /// </summary>
        /// <param name="rootNode">Reference to root node.</param>
        public QuestAnySuccessNode(IQuestRootNode rootNode)
            : base(rootNode)
        {
            this.Title = "ANY SUCCESS NODE";
        }

        /// <inheritdoc />
        public override void Add(IQuestChildNode childNode)
        {
            Guard.Argument(childNode, nameof(childNode)).NotNull();

            base.Add(childNode);

            childNode.PropertyChanged += this.OnChildNodeOnPropertyChanged;
        }

        /// <inheritdoc />
        protected override void OnStatusChanged(QuestStatus newStatus)
        {
            Guard.Argument(newStatus, nameof(newStatus)).Defined();

            switch (newStatus)
            {
                case QuestStatus.Failure:
                    foreach (var childNode in this.ChildNodes)
                    {
                        childNode.SetStatus(QuestStatus.Failure);
                    }

                    break;

                case QuestStatus.Active:
                    this.MarkAllChildsAsActive();

                    break;

                case QuestStatus.Sleeping:
                    this.MarkAllChildsAsSleeping();

                    break;
            }

            base.OnStatusChanged(newStatus);
        }

        /// <inheritdoc />
        protected override void MarkAsSuccess()
        {
            this.MarkAllChildsAsSleeping();

            base.MarkAsSuccess();
        }

        private void MarkAllChildsAsActive()
        {
            if (this.ChildNodes.Count == 0 || this.ChildNodes.All(x => x.Status == QuestStatus.Failure))
            {
                this.SetStatus(QuestStatus.Failure);

                return;
            }

            if (this.ChildNodes.Any(x => x.Status == QuestStatus.Success))
            {
                this.MarkAsSuccess();

                return;
            }

            foreach (var node in this.ChildNodes.Where(x => x.Status == QuestStatus.Sleeping))
            {
                node.SetStatus(QuestStatus.Active);
            }
        }

        private void MarkAllChildsAsSleeping()
        {
            foreach (var childNode in this.ChildNodes.Where(x => x.CanSetToStatus(QuestStatus.Sleeping)))
            {
                childNode.SetStatus(QuestStatus.Sleeping);
            }
        }

        private void OnChildNodeOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Guard.Argument(sender, nameof(sender)).NotNull();
            Guard.Argument(e, nameof(e)).NotNull();

            switch (e.PropertyName)
            {
                case nameof(IQuestChildNode.Status)
                    when sender is IQuestChildNode childNode && childNode.Status == QuestStatus.Success:
                    this.MarkAsSuccess();

                    break;

                case nameof(IQuestChildNode.Status)
                    when sender is IQuestChildNode childNode && childNode.Status == QuestStatus.Failure:
                    this.MarkAllChildsAsActive();

                    break;
            }
        }
    }
}
