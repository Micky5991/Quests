using System.ComponentModel;
using Dawn;
using JetBrains.Annotations;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

/// <summary>
/// Type that holds multiple child quests and activates them all at the same time. When all child nodes signal success,
/// this node will change to success. When any child node fails, all remaining child nodes will fail and this node will
/// be marked as failure.
/// </summary>
[PublicAPI]
public class QuestParallelNode : QuestCompositeNode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuestParallelNode"/> class.
    /// </summary>
    /// <param name="rootNode">Root quest node of this quest tree.</param>
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
        Guard.Argument(newStatus, nameof(newStatus)).Defined();

        switch (newStatus)
        {
            case QuestStatus.Active:
            case QuestStatus.Sleeping:
            case QuestStatus.Failure:
                foreach (var childNode in this.ChildNodes.Where(x => x.CanSetToStatus(newStatus)))
                {
                    childNode.SetStatus(newStatus);
                }

                break;
        }

        base.OnStatusChanged(newStatus);
    }

    private void OnChildNodePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        Guard.Argument(sender, nameof(sender)).NotNull();
        Guard.Argument(e, nameof(e)).NotNull();

        if (sender is not IQuestChildNode childNode || this.ChildNodes.Contains(childNode) == false)
        {
            return;
        }

        switch (e.PropertyName)
        {
            case nameof(IQuestChildNode.Status) when childNode.Status == QuestStatus.Failure:
                this.SetStatus(QuestStatus.Failure);

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
