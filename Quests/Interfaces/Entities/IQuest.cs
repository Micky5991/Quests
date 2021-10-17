using System;
using System.Collections.Immutable;
using System.ComponentModel;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Exceptions;

namespace Micky5991.Quests.Interfaces.Entities
{
    /// <summary>
    /// Basic quest interfaces that describes general quest behavior.
    /// </summary>
    public interface IQuest : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Gets the current, changing title of this quest.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the current, changing description of this quest.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the current, changing status of this quest.
        /// </summary>
        public QuestStatus Status { get; }

        /// <summary>
        /// Gets arbitrary data associated with this specific quest.
        /// </summary>
        public IImmutableDictionary<string, object> Context { get; }

        /// <summary>
        /// Prepares the quest and executes actions needed to use this <see cref="IQuest"/>.
        /// </summary>
        /// <param name="context">Initial context of this quest.</param>
        public void Initialize(IImmutableDictionary<string, object>? context = null);

        /// <summary>
        /// Transitions the quest to the given status.
        /// </summary>
        /// <param name="newStatus">New state of the quest.</param>
        /// <exception cref="QuestTransitionException"><paramref name="newStatus"/> could not be reached with the current <see cref="Status"/>.</exception>
        public void TransitionTo(QuestStatus newStatus);
    }
}
