using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

public class QuestAnySuccessNode : QuestCompositeNode
{
    public QuestAnySuccessNode(IQuestRootNode rootNode)
        : base(rootNode)
    {
    }
}
