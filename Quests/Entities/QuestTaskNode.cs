using System.Collections.Immutable;
using Dawn;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

/// <inheritdoc cref="IQuestTaskNode" />
public abstract class QuestTaskNode : QuestChildNode, IQuestTaskNode
{
    private IImmutableSet<ISubscription> eventSubscriptions = ImmutableHashSet<ISubscription>.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestTaskNode"/> class.
    /// </summary>
    /// <param name="rootNode">Reference to the root node of this quest.</param>
    public QuestTaskNode(IQuestRootNode rootNode)
        : base(rootNode)
    {
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        this.DetachEventListeners();

        base.Dispose();
    }

    /// <summary>
    /// Calls to this method trigger a subscription of all needed events for this quest tree.
    /// </summary>
    /// <returns>List of created subscriptions.</returns>
    protected abstract IEnumerable<ISubscription> GetEventSubscriptions();

    /// <inheritdoc />
    protected override void OnStatusChanged(QuestStatus newStatus)
    {
        Guard.Argument(newStatus, nameof(newStatus)).Defined();

        switch (newStatus)
        {
            case QuestStatus.Active:
                this.AttachEventListeners();

                break;

            default:
                this.DetachEventListeners();

                break;
        }

        base.OnStatusChanged(newStatus);
    }

    private void AttachEventListeners()
    {
        this.DetachEventListeners();

        this.eventSubscriptions = this.GetEventSubscriptions().ToImmutableHashSet();
    }

    private void DetachEventListeners()
    {
        var subscriptions = this.eventSubscriptions;
        this.eventSubscriptions = ImmutableHashSet<ISubscription>.Empty;

        foreach (var subscription in subscriptions)
        {
            subscription.Dispose();
        }
    }
}
