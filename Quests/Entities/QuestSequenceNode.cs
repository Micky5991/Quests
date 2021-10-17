using System.ComponentModel;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

public class QuestSequenceNode : QuestCompositeNode
{
    private IQuestChildNode? currentActiveNode;

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

        this.ActivateNextChildNode();
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

        if (this.currentActiveNode == null)
        {
            return;
        }

        this.UnwatchChildNodeProperties(this.currentActiveNode);
        this.currentActiveNode = null;
    }

    private bool ActivateNextChildNode()
    {
        if (this.currentActiveNode != null)
        {
            this.UnwatchChildNodeProperties(this.currentActiveNode);
            this.currentActiveNode = null;
        }

        var firstUncompletedQuest = this.ChildNodes.FirstOrDefault(x => x.CanActivate());
        if (firstUncompletedQuest == null)
        {
            return false;
        }

        this.WatchChildNodeProperties(firstUncompletedQuest);
        this.currentActiveNode = firstUncompletedQuest;

        firstUncompletedQuest.Activate();

        return true;
    }

    private void WatchChildNodeProperties(IQuestChildNode node)
    {
        node.PropertyChanged += this.OnChildPropertyChanged;
    }

    private void UnwatchChildNodeProperties(IQuestChildNode node)
    {
        node.PropertyChanged -= this.OnChildPropertyChanged;
    }

    private void OnChildPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (this.currentActiveNode == null)
        {
            return;
        }

        switch (e.PropertyName)
        {
            case nameof(IQuestChildNode.Status) when this.currentActiveNode.Status is QuestStatus.Success:
                var nextFound = this.ActivateNextChildNode();
                if (nextFound == false)
                {
                    this.MarkAsSuccess();
                }

                break;

            case nameof(IQuestChildNode.Status) when this.currentActiveNode.Status is QuestStatus.Failure:
                this.UnwatchChildNodeProperties(this.currentActiveNode);
                this.currentActiveNode = null;

                foreach (var uncompletedChildNode in this.ChildNodes.Where(x => x.Completed == false))
                {
                    uncompletedChildNode.MarkAsFailure();
                }

                break;
        }
    }
}
