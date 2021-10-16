using System;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Entities;

namespace Micky5991.Quests.Exceptions
{
    /// <summary>
    /// Exception which reports an invalid transition between states.
    /// </summary>
    public class QuestTransitionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestTransitionException"/> class.
        /// </summary>
        /// <param name="quest">Quest object in question to report invalid transition.</param>
        /// <param name="oldStatus">Old <see cref="QuestStatus"/>.</param>
        /// <param name="newStatus">New <see cref="QuestStatus"/>. This status could not be reached.</param>
        public QuestTransitionException(IQuest quest, QuestStatus oldStatus, QuestStatus newStatus)
            : base($"Unable to transition quest {quest} from state {oldStatus} to {newStatus}")
        {
        }
    }
}
