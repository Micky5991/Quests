using System.Collections.Immutable;
using Micky5991.Quests.Interfaces.Entities;

namespace Micky5991.Quests.Interfaces.Services
{
    /// <summary>
    /// Factory that creates simple <see cref="IQuest"/> objects.
    /// </summary>
    public interface IQuestFactory
    {
        /// <summary>
        /// Builds a new quest object and executes needed steps to prepare a correct run.
        /// </summary>
        /// <typeparam name="T">Quest type to create.</typeparam>
        /// <param name="context">Initial context of this quest.</param>
        /// <returns>Returns the initialized quest object.</returns>
        public T BuildQuest<T>(IImmutableDictionary<string, object>? context = null)
            where T : IQuest;
    }
}
