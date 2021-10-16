using System.Collections.Generic;
using System.Collections.Immutable;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Quests.Services
{
    public abstract class QuestRegistry
    {
        private IImmutableSet<QuestMeta>? registeredQuests = null;

        protected abstract IEnumerable<QuestMeta> LoadQuests();

        protected QuestMeta RegisterQuest<T>()
            where T : IQuest
        {
            return new QuestMeta(typeof(T));
        }

        public IImmutableSet<QuestMeta> GetRegisteredQuests()
        {
            if (this.registeredQuests == null)
            {
                this.registeredQuests = this.LoadQuests().ToImmutableHashSet();
            }

            return this.registeredQuests;
        }

        public void AddQuestsToServiceCollection(IServiceCollection serviceCollection)
        {
            foreach (var registeredQuest in this.GetRegisteredQuests())
            {
                serviceCollection.AddTransient(registeredQuest.Type);
            }
        }
    }
}
