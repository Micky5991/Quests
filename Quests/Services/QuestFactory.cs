using System;
using System.Collections.Immutable;
using Micky5991.Quests.Interfaces.Entities;
using Micky5991.Quests.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Quests.Services
{
    /// <inheritdoc />
    public class QuestFactory : IQuestFactory
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider which is able to create <see cref="IQuest"/> objects.</param>
        public QuestFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public T BuildQuest<T>(IImmutableDictionary<string, object>? context)
            where T : IQuest
        {
            var quest = this.serviceProvider.GetService<T>();

            quest.Initialize(context);

            return quest;
        }
    }
}
