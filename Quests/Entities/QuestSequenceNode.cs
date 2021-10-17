using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

public class QuestSequenceNode : QuestCompositeNode
{
    public QuestSequenceNode(IQuestRootNode rootNode)
        : base(rootNode)
    {
        this.Title = "SEQUENCE NODE";
    }

    /// <inheritdoc />
    public override void Activate()
    {
        if (this.CanActivate() == false)
        {
            return;
        }

        var firstUncompletedQuest = this.ChildNodes.FirstOrDefault(x => x.CanActivate());
        if (firstUncompletedQuest == null)
        {
            return;
        }

        firstUncompletedQuest.Activate();
        base.Activate();
    }

    /// <inheritdoc />
    public override void Deactivate()
    {
        if (this.CanDeactivate() == false)
        {
            return;
        }

        foreach (var childNode in this.ChildNodes)
        {
            childNode.Deactivate();
        }

        base.Deactivate();
    }
}
