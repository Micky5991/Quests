using System.ComponentModel;
using Micky5991.Quests.Enums;
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
    public override void Add(IQuestChildNode childNode)
    {
        base.Add(childNode);

        childNode.PropertyChanged += this.OnChildPropertyChanged;
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        foreach (var childNode in this.ChildNodes)
        {
            childNode.PropertyChanged -= this.OnChildPropertyChanged;
        }

        base.Dispose();
    }

    /// <inheritdoc />
    protected override void OnStatusChanged(QuestStatus newStatus)
    {
        switch (newStatus)
        {
            case QuestStatus.Failure:
                foreach (var childNode in this.ChildNodes.Where(x => x.CanMarkAsFailure()))
                {
                    childNode.MarkAsFailure();
                }

                break;

            case QuestStatus.Active:
                this.MarkNextChildNodeAsActive();

                break;

            case QuestStatus.Sleeping:
                this.MarkAllChildNodesAsSleeping();

                break;
        }

        base.OnStatusChanged(newStatus);
    }

    private void MarkAllChildNodesAsSleeping()
    {
        foreach (var activeNode in this.ChildNodes.Where(x => x.CanMarkAsSleeping()))
        {
            activeNode.MarkAsSleeping();
        }
    }

    private void MarkNextChildNodeAsActive()
    {
        if (this.ChildNodes.Any(x => x.Status == QuestStatus.Failure))
        {
            this.MarkAsFailure();

            return;
        }

        this.MarkAllChildNodesAsSleeping();

        var firstUncompletedQuest = this.ChildNodes.FirstOrDefault(x => x.Finished == false);
        if (firstUncompletedQuest == null)
        {
            this.MarkAsSuccess();

            return;
        }

        firstUncompletedQuest.MarkAsActive();
    }

    private void OnChildPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(IQuestChildNode.Status)
                when sender is IQuestChildNode childNode &&
                     childNode.Status is QuestStatus.Success or QuestStatus.Failure:

                this.MarkNextChildNodeAsActive();

                break;
        }
    }
}
