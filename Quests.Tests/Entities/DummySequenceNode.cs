using JetBrains.Annotations;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Tests.Entities;

public class DummySequenceNode : QuestSequenceNode
{
    public DummySequenceNode([NotNull] IQuestRootNode rootNode)
        : base(rootNode)
    {
    }
}
