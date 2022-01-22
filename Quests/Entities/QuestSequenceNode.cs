using System.ComponentModel;
using System.Linq;
using Dawn;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities
{
    /// <summary>
    /// Type that will execute all nodes one after another. When the first child node fails, this node will mark all other
    /// nodes as failure and itself. When all nodes complete successfully, it will finish successfully too.
    /// </summary>
    public class QuestSequenceNode : QuestCompositeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestSequenceNode"/> class.
        /// </summary>
        /// <param name="rootNode">Root quest node of this quest tree.</param>
        public QuestSequenceNode(IQuestRootNode rootNode)
            : base(rootNode)
        {
            this.Title = "SEQUENCE NODE";
        }

        /// <inheritdoc />
        public override void Add(IQuestChildNode childNode)
        {
            base.Add(childNode);

            childNode.PropertyChanged += this.OnChildPropertyChanged;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            foreach (var childNode in this.ChildNodes)
            {
                childNode.PropertyChanged -= this.OnChildPropertyChanged;
            }

            base.Dispose();
        }

        /// <inheritdoc />
        protected override void OnStatusChanged(QuestStatus newStatus)
        {
            Guard.Argument(newStatus, nameof(newStatus)).Defined();

            switch (newStatus)
            {
                case QuestStatus.Failure:
                case QuestStatus.Sleeping:
                    this.SetStatusOfChildNodes(newStatus);

                    break;

                case QuestStatus.Active:
                    this.MarkNextChildNodeAsActive();

                    break;
            }

            base.OnStatusChanged(newStatus);
        }

        private void SetStatusOfChildNodes(QuestStatus newStatus)
        {
            foreach (var activeNode in this.ChildNodes.Where(x => x.CanSetToStatus(newStatus)))
            {
                activeNode.SetStatus(newStatus);
            }
        }

        private void MarkNextChildNodeAsActive()
        {
            if (this.ChildNodes.Any(x => x.Status == QuestStatus.Failure))
            {
                this.SetStatus(QuestStatus.Failure);

                return;
            }

            this.SetStatusOfChildNodes(QuestStatus.Sleeping);

            var firstUncompletedQuest = this.ChildNodes.FirstOrDefault(x => x.Finished == false);
            if (firstUncompletedQuest == null)
            {
                this.MarkAsSuccess();

                return;
            }

            firstUncompletedQuest.SetStatus(QuestStatus.Active);
        }

        private void OnChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Guard.Argument(sender, nameof(sender)).NotNull();
            Guard.Argument(e, nameof(e)).NotNull();

            switch (e.PropertyName)
            {
                case nameof(IQuestChildNode.Status)
                    when sender is IQuestChildNode childNode &&
                         childNode.Status is QuestStatus.Success or QuestStatus.Failure:

                    this.MarkNextChildNodeAsActive();

                    break;
            }
        }
    }
}
