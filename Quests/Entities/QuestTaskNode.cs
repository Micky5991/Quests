using Dawn;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities
{
    /// <inheritdoc cref="IQuestTaskNode" />
    public abstract class QuestTaskNode : QuestChildNode, IQuestTaskNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestTaskNode"/> class.
        /// </summary>
        /// <param name="rootNode">Reference to the root node of this quest.</param>
        public QuestTaskNode(IQuestRootNode rootNode)
            : base(rootNode)
        {
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            this.DetachEventListeners();

            base.Dispose();
        }

        /// <inheritdoc />
        protected override void OnStatusChanged(QuestStatus newStatus)
        {
            Guard.Argument(newStatus, nameof(newStatus)).Defined();

            switch (newStatus)
            {
                case QuestStatus.Active:
                    this.AttachEventListeners();

                    break;

                default:
                    this.DetachEventListeners();

                    break;
            }

            base.OnStatusChanged(newStatus);
        }

        /// <summary>
        /// Attaches listeners to quest events which contribute to their progression.
        /// </summary>
        protected abstract void AttachEventListeners();

        /// <summary>
        /// Detatches previously attached event listeners in <see cref="AttachEventListeners"/>.
        /// </summary>
        protected abstract void DetachEventListeners();
    }
}
