using Dawn;
using JetBrains.Annotations;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities
{
    /// <summary>
    /// Type that has always success status, unless it fails explicitly.
    /// </summary>
    [PublicAPI]
    public abstract class QuestConditonNode : QuestChildNode, IQuestTaskNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestConditonNode"/> class.
        /// </summary>
        /// <param name="rootNode">Reference to the root node of this quest.</param>
        protected QuestConditonNode(IQuestRootNode rootNode)
            : base(rootNode)
        {
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            this.DetachEventListeners();
        }

        /// <inheritdoc />
        public override bool CanSetToStatus(QuestStatus newStatus)
        {
            switch (newStatus)
            {
                case QuestStatus.Sleeping:
                case QuestStatus.Failure:
                case QuestStatus.Active:
                    return this.Status == QuestStatus.Success || base.CanSetToStatus(newStatus);

                default:
                    return base.CanSetToStatus(newStatus);
            }
        }

        /// <inheritdoc />
        protected override void OnStatusChanged(QuestStatus newStatus)
        {
            Guard.Argument(newStatus, nameof(newStatus)).Defined();

            switch (newStatus)
            {
                case QuestStatus.Active:
                    this.MarkAsSuccess();

                    break;

                case QuestStatus.Success:
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
