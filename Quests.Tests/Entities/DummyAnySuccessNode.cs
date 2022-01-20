using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Tests.Entities;

public class DummyAnySuccessNode : QuestAnySuccessNode
{
    public DummyAnySuccessNode(IQuestRootNode rootNode)
        : base(rootNode)
    {
    }
}
