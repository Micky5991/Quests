using System.Collections.Immutable;
using Dawn;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Nodes;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Quests.Services
{
    /// <summary>
    /// Type that handles meta data about quests and provides types to the DI container.
    /// </summary>
    public abstract class QuestRegistry
    {
        private IImmutableSet<QuestMeta>? registeredQuests;

        /// <summary>
        /// Returns a cached list of available <see cref="QuestMeta"/> object. To add new quest types, return them inside
        /// <see cref="BuildAvailableQuestMeta"/>.
        /// </summary>
        /// <returns>List of available <see cref="QuestMeta"/>.</returns>
        public IImmutableSet<QuestMeta> GetAvailableQuestMeta()
        {
            if (this.registeredQuests == null)
            {
                this.registeredQuests = this.BuildAvailableQuestMeta().ToImmutableHashSet();
            }

            return this.registeredQuests;
        }

        /// <summary>
        /// Utility to add all build <see cref="IQuestRootNode"/> types to the <see cref="IServiceCollection"/> instance, so they
        /// are available at all times. Quests will be transient.
        /// </summary>
        /// <param name="serviceCollection">List of services to add the service to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serviceCollection"/> is null</exception>
        public void AddQuestsToServiceCollection(IServiceCollection serviceCollection)
        {
            Guard.Argument(serviceCollection, nameof(serviceCollection)).NotNull();

            foreach (var registeredQuest in this.GetAvailableQuestMeta())
            {
                serviceCollection.AddTransient(registeredQuest.Type);
            }
        }

        /// <summary>
        /// Builds a list of <see cref="QuestMeta"/> objects containing all available quests.
        /// </summary>
        /// <returns>List of available <see cref="QuestMeta"/> objects.</returns>
        protected abstract IEnumerable<QuestMeta> BuildAvailableQuestMeta();

        /// <summary>
        /// Builds a single <see cref="QuestMeta"/> object used for <see cref="BuildAvailableQuestMeta"/> to return.
        /// </summary>
        /// <typeparam name="T">Implementation of a single quest logic.</typeparam>
        /// <returns>Returns built <see cref="QuestMeta"/> object.</returns>
        protected QuestMeta BuildQuest<T>()
            where T : IQuestRootNode
        {
            return new QuestMeta(typeof(T));
        }
    }
}
