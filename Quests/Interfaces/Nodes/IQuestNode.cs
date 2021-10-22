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
    /// Gets a value indicating whether the current node has been initialized before.
    /// </summary>
    public bool Initialized { get; }

    /// <summary>
    /// Gets a value indicating whether the quest has been disposed.
    /// </summary>
    public bool Disposed { get; }

    /// <summary>
    /// Prepares the quest and executes actions needed to use this <see cref="IQuestNode"/>.
    /// </summary>
    public void Initialize();

    /// <summary>
    /// Sets the Status of this quest to the given <paramref name="newStatus"/>.
    /// </summary>
    /// <param name="newStatus">New status this quest should reach.</param>
    /// <exception cref="ArgumentNullException">Success can only be set by the quest itself.</exception>
    public void SetStatus(QuestStatus newStatus);

    /// <summary>
    /// Checks if this quest could reach the status provided in <paramref name="newStatus"/>.
    /// </summary>
    /// <param name="newStatus">New status this quest should reach.</param>
    /// <returns>true if the status can be set, false otherwise.</returns>
    public bool CanSetToStatus(QuestStatus newStatus);
}
