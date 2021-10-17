using System.Collections.Immutable;

namespace Micky5991.Quests.Interfaces.Nodes;

/// <summary>
/// Quest type that is the top node and holds exactly one child element.
/// </summary>
public interface IQuestRootNode : IQuestNode
{
    public IImmutableDictionary<string, object> Blackboard { get; set; }

    public IImmutableDictionary<string, object> Context { get; set; }

    public IQuestChildNode? ChildNode { get; }
}
