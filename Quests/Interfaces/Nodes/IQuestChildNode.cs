namespace Micky5991.Quests.Interfaces.Nodes;

/// <summary>
/// Interface that represents a node that can be placed inside other <see cref="IQuestChildNode"/> and
/// <see cref="IQuestRootNode"/>.
/// </summary>
public interface IQuestChildNode : IQuestNode
{
    /// <summary>
    /// Gets the reference to the <see cref="IQuestRootNode"/> of this quest tree.
    /// </summary>
    public IQuestRootNode RootNode { get; }
}
