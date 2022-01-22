using System;
using Micky5991.Quests.Interfaces.Nodes;
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
        /// <param name="serviceProvider">Service provider needed to created quests.</param>
        public QuestFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public IQuestRootNode BuildQuest<T>()
            where T : IQuestRootNode
        {
            var quest = this.serviceProvider.GetService<T>();

            quest.Initialize();

            return quest;
        }
    }
}
