using System.Collections.Immutable;

namespace Micky5991.Quests.Interfaces.Nodes;

/// <summary>
/// Quest type that is the top node and holds exactly one child element.
/// </summary>
public interface IQuestRootNode : IQuestNode
{
    /// <summary>
    /// Gets or sets the data of this root node.
    /// </summary>
    public IImmutableDictionary<string, object> Blackboard { get; set; }

    /// <summary>
    /// Gets or sets the context of this root.
    /// </summary>
    public IImmutableDictionary<string, object> Context { get; set; }

    /// <summary>
    /// Gets the only child node of this root node.
    /// </summary>
    public IQuestChildNode? ChildNode { get; }
}
