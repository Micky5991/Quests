namespace Micky5991.Quests.Interfaces.Nodes;

/// <summary>
/// Interface that represents a node that can be placed inside other <see cref="IQuestChildNode"/> and
/// <see cref="IQuestRootNode"/>.
/// </summary>
public interface IQuestChildNode : IQuestNode
{
    /// <summary>
    /// Gets the reference to the <see cref="IQuestRootNode"/> of this quest tree.
    /// </summary>
    public IQuestRootNode RootNode { get; }

    /// <summary>
    /// Checks if the current node can be marked as active.
    /// </summary>
    /// <returns>Returns true if this node can be marked as active.</returns>
    public bool CanMarkAsActive();

    /// <summary>
    /// Checks if the current node can be marked as sleeping.
    /// </summary>
    /// <returns>Returns true if this node can be marked as sleeping.</returns>
    public bool CanMarkAsSleeping();

    /// <summary>
    /// Checks if the current node can be marked as failure.
    /// </summary>
    /// <returns>Returns true if this node can be marked as failure.</returns>
    public bool CanMarkAsFailure();

    /// <summary>
    /// Changes the status of this node to active.
    /// </summary>
    public void MarkAsActive();

    /// <summary>
    /// Changes the status of this node to sleeping.
    /// </summary>
    public void MarkAsSleeping();

    /// <summary>
    /// Changes the status of this node to failure.
    /// </summary>
    public void MarkAsFailure();
}
