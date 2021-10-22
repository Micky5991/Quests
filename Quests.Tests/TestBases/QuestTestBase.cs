using System;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.EventAggregator.Services;
using Micky5991.Quests.Interfaces.Nodes;
using Micky5991.Quests.Tests.Entities;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace Micky5991.Quests.Tests.TestBases;

public abstract class QuestTestBase
{
    protected IServiceProvider SetupServiceProvider(Func<IServiceCollection, IServiceCollection> serviceCollection)
    {
        return serviceCollection(
                                 new ServiceCollection()
                                     .AddSingleton<IEventAggregator, EventAggregatorService>()
                                     .AddLogging(builder => builder.AddSerilog(Logger.None)))
            .BuildServiceProvider();
    }

    protected DummyQuest CreateExampleQuest(Func<DummyQuest, IQuestChildNode> setup, bool initialize = true)
    {
        var quest = new DummyQuest(setup);

        if (initialize)
        {
            quest.Initialize();
        }

        return quest;
    }
}
