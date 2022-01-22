using Dawn;
using JetBrains.Annotations;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities
{
    /// <inheritdoc cref="IQuestChildNode" />
    [PublicAPI]
    public abstract class QuestChildNode : QuestNode, IQuestChildNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestChildNode"/> class.
        /// </summary>
        /// <param name="rootNode">Reference to root node.</param>
        public QuestChildNode(IQuestRootNode rootNode)
        {
            Guard.Argument(rootNode, nameof(rootNode)).NotNull();

            this.RootNode = rootNode;
        }

        /// <inheritdoc />
        public IQuestRootNode RootNode { get; }
    }
}
