using System.Collections.Immutable;

namespace Micky5991.Quests.Interfaces.Nodes;

public interface IQuestCompositeNode : IQuestChildNode, IEnumerable<IQuestChildNode>
{
    public IImmutableList<IQuestChildNode> ChildNodes { get; }
}
