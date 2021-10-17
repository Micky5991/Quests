using JetBrains.Annotations;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

public class QuestParallelNode : QuestCompositeNode
{
    public QuestParallelNode([NotNull] IQuestRootNode rootNode)
        : base(rootNode)
    {
        this.Title = "PARALLEL NODE";
    }

    /// <inheritdoc />
    public override void Activate()
    {
        if (this.CanActivate() == false)
        {
            return;
        }

        foreach (var childNode in this.ChildNodes.Where(x => x.CanActivate()))
        {
            childNode.Activate();
        }

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
