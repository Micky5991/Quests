using Micky5991.Quests.Interfaces.Nodes;
using Micky5991.Quests.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Quests.Services;

public class QuestFactory : IQuestFactory
{
    private readonly IServiceProvider serviceProvider;

    public QuestFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IQuestRootNode BuildQuest<T>()
        where T : IQuestRootNode
    {
        var quest = this.serviceProvider.GetService<T>();

        quest.Initialize();

        return quest;
    }
}
