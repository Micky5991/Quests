using System.Collections.Immutable;

namespace Micky5991.Quests.Interfaces.Nodes
{
    /// <summary>
    /// Interface that represents a quest node which contains multiple child nodes.
    /// </summary>
    public interface IQuestCompositeNode : IQuestChildNode, IEnumerable<IQuestChildNode>
    {
        /// <summary>
        /// Gets the list of child nodes of this node.
        /// </summary>
        public IImmutableList<IQuestChildNode> ChildNodes { get; }
    }
}
