using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Tests.Entities;

public class DummyParallelNode : QuestParallelNode
{
    public DummyParallelNode(IQuestRootNode rootNode)
        : base(rootNode)
    {
    }
}
