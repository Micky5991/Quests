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
    public string Title { get; }

    /// <summary>
    /// Gets the current, changing status of this quest.
    /// </summary>
    public QuestStatus Status { get; }

    /// <summary>
    /// Gets a value indicating whether the current quest node has been completed.
    /// </summary>
    public bool Completed { get; }

    /// <summary>
    /// Prepares the quest and executes actions needed to use this <see cref="IQuestNode"/>.
    /// </summary>
    public void Initialize();
}
