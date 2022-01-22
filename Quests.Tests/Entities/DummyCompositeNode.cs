using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Tests.Entities
{
    public class DummyCompositeNode : QuestCompositeNode
    {
        public DummyCompositeNode(IQuestRootNode rootNode)
            : base(rootNode)
        {
        }
    }
}
