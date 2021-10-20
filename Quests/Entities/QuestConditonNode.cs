using System.Collections.Immutable;
using Dawn;
using JetBrains.Annotations;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

/// <summary>
/// Type that has always success status, unless it fails explicitly.
/// </summary>
[PublicAPI]
public abstract class QuestConditonNode : QuestChildNode, IQuestTaskNode
{
    private IImmutableSet<ISubscription> eventSubscriptions = ImmutableHashSet<ISubscription>.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestConditonNode"/> class.
    /// </summary>
    /// <param name="rootNode">Reference to the root node of this quest.</param>
    protected QuestConditonNode(IQuestRootNode rootNode)
        : base(rootNode)
    {
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        this.DetachEventListeners();
    }

    /// <inheritdoc />
    public override bool CanMarkAsSleeping()
    {
        return this.Status is QuestStatus.Success || base.CanMarkAsSleeping();
    }

    /// <inheritdoc />
    public override bool CanMarkAsFailure()
    {
        return this.Status == QuestStatus.Success || base.CanMarkAsFailure();
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
                this.MarkAsSuccess();

                break;

            case QuestStatus.Success:
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
