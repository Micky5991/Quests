using System.ComponentModel;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

public class QuestParallelNode : QuestCompositeNode
{
    public QuestParallelNode(IQuestRootNode rootNode)
        : base(rootNode)
    {
        this.Title = "PARALLEL NODE";
    }

    /// <inheritdoc />
    public override void Add(IQuestChildNode childNode)
    {
        base.Add(childNode);

        childNode.PropertyChanged += this.OnChildNodePropertyChanged;
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        foreach (var childNode in this.ChildNodes)
        {
            childNode.PropertyChanged -= this.OnChildNodePropertyChanged;
        }

        base.Dispose();
    }

    /// <inheritdoc />
    protected override void OnStatusChanged(QuestStatus newStatus)
    {
        switch (newStatus)
        {
            case QuestStatus.Active:
                foreach (var childNode in this.ChildNodes.Where(x => x.CanMarkAsActive()))
                {
                    childNode.MarkAsActive();
                }

                break;

            case QuestStatus.Sleeping:
                foreach (var childNode in this.ChildNodes.Where(x => x.CanMarkAsSleeping()))
                {
                    childNode.MarkAsSleeping();
                }

                break;

            case QuestStatus.Failure:
                foreach (var childNode in this.ChildNodes.Where(x => x.CanMarkAsFailure()))
                {
                    childNode.MarkAsFailure();
                }

                break;
        }

        base.OnStatusChanged(newStatus);
    }

    private void OnChildNodePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is not IQuestChildNode childNode || this.ChildNodes.Contains(childNode) == false)
        {
            return;
        }

        switch (e.PropertyName)
        {
            case nameof(IQuestChildNode.Status) when childNode.Status == QuestStatus.Failure:
                this.MarkAsFailure();

                break;

            case nameof(IQuestChildNode.Status) when childNode.Status == QuestStatus.Success:
                if (this.ChildNodes.Count(x => x.Finished == false) == 0)
                {
                    this.MarkAsSuccess();
                }

                break;
        }
    }
}
