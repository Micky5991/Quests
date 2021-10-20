using System.ComponentModel;
using JetBrains.Annotations;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

/// <summary>
/// Node type that tries one node after another. It will succeed as soon as the first child node signals success.
/// When all child quests fail or no child quests are available, this node will fail.
/// </summary>
[PublicAPI]
public class QuestAnySuccessSequenceNode : QuestCompositeNode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuestAnySuccessSequenceNode"/> class.
    /// </summary>
    /// <param name="rootNode">Root node of this quest tree.</param>
    public QuestAnySuccessSequenceNode(IQuestRootNode rootNode)
        : base(rootNode)
    {
        this.Title = "ANY SUCCESS NODE";
    }

    /// <inheritdoc />
    public override void Add(IQuestChildNode childNode)
    {
        base.Add(childNode);

        childNode.PropertyChanged += this.OnChildNodeOnPropertyChanged;
    }

    /// <inheritdoc />
    protected override void OnStatusChanged(QuestStatus newStatus)
    {
        switch (newStatus)
        {
            case QuestStatus.Failure:
                foreach (var childNode in this.ChildNodes)
                {
                    childNode.MarkAsFailure();
                }

                break;

            case QuestStatus.Active:
                this.MarkNextChildNodeAsActive();

                break;

            case QuestStatus.Sleeping:
                this.MarkAllChildsAsSleeping();

                break;
        }

        base.OnStatusChanged(newStatus);
    }

    private void MarkNextChildNodeAsActive()
    {
        if (this.ChildNodes.Count == 0 || this.ChildNodes.All(x => x.Status == QuestStatus.Failure))
        {
            this.MarkAsFailure();

            return;
        }

        if (this.ChildNodes.Any(x => x.Status == QuestStatus.Success))
        {
            this.MarkAsSuccess();

            return;
        }

        this.MarkAllChildsAsSleeping();

        var firstUnfinishedNode = this.ChildNodes.FirstOrDefault(x => x.Finished == false);
        if (firstUnfinishedNode == null)
        {
            this.MarkAsFailure();

            return;
        }

        firstUnfinishedNode.MarkAsActive();
    }

    private void MarkAllChildsAsSleeping()
    {
        foreach (var childNode in this.ChildNodes.Where(x => x.CanMarkAsSleeping()))
        {
            childNode.MarkAsSleeping();
        }
    }

    private void OnChildNodeOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(IQuestChildNode.Status)
                when sender is IQuestChildNode childNode && childNode.Status == QuestStatus.Success:
                this.MarkAsSuccess();

                break;

            case nameof(IQuestChildNode.Status)
                when sender is IQuestChildNode childNode && childNode.Status == QuestStatus.Failure:
                this.MarkNextChildNodeAsActive();

                break;
        }
    }
}
