using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Tests.Entities;

public class DummyFirstSuccessNode : QuestFirstSuccessSequenceNode
{
    public DummyFirstSuccessNode(IQuestRootNode rootNode)
        : base(rootNode)
    {
    }
}
