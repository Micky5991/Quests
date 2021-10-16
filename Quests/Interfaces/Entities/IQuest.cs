using System;
using System.Collections.Immutable;
using System.ComponentModel;
using Micky5991.Quests.Enums;

namespace Micky5991.Quests.Interfaces.Entities
{
    /// <summary>
    /// Basic quest interfaces that describes general quest behavior.
    /// </summary>
    public interface IQuest : INotifyPropertyChanged, IDisposable
    {
        public string Title { get; }

        public string Description { get; }

        public QuestStatus Status { get; }

        public IImmutableDictionary<string, object> Context { get; }

        /// <summary>
        /// Prepares the quest and executes actions needed to use this <see cref="IQuest"/>.
        /// </summary>
        /// <param name="context">Initial context of this quest.</param>
        public void Initialize(IImmutableDictionary<string, object>? context = null);

        /// <summary>
        /// Transitions the quest to the given status.
        /// </summary>
        /// <param name="newStatus"></param>
        /// <exception cref=""></exception>
        public void TransitionTo(QuestStatus newStatus);
    }
}
