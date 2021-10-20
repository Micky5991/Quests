using System.Collections.Immutable;
using System.ComponentModel;
using Micky5991.Quests.Enums;

namespace Micky5991.Quests.Interfaces.Nodes;

/// <summary>
/// Basic quest node that has common behavior in it.
/// </summary>
public interface IQuestNode : INotifyPropertyChanged, IDisposable
{
    /// <summary>
    /// Gets the current, changing title of this quest.
    /// </summary>
    /// <exception cref="ArgumentNullException">Value is null.</exception>
    public string Title { get; }

    /// <summary>
    /// Gets the current, changing status of this quest.
    /// </summary>
    /// <exception cref="ArgumentException">Value is not defined inside <see cref="QuestStatus"/>.</exception>
    public QuestStatus Status { get; }

    /// <summary>
    /// Gets a value indicating whether the current quest node has been completed.
    /// </summary>
    public bool Finished { get; }

    /// <summary>
    /// Gets a value indicating whether the quest has been disposed.
    /// </summary>
    public bool Disposed { get; }

    /// <summary>
    /// Prepares the quest and executes actions needed to use this <see cref="IQuestNode"/>.
    /// </summary>
    public void Initialize();

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
