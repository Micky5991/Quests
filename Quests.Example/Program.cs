using System;
using System.Runtime.CompilerServices;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.EventAggregator.Services;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Example.Entities;
using Micky5991.Quests.Example.Quests;
using Micky5991.Quests.Interfaces.Services;
using Micky5991.Quests.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Micky5991.Quests.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start Quests Example");

            var questRegistry = new ExampleQuestRegistry();
            var player = new Player();
            var enemy = new Enemy();
            var provider = BuildServiceProvider(questRegistry);

            Console.WriteLine("ServiceProvider built");

            var factory = provider.GetService<IQuestFactory>();
            if (factory == null)
            {
                Console.WriteLine($"Unable to find {nameof(IQuestFactory)} service");

                return;
            }

            var eventAggregator = provider.GetService<IEventAggregator>();
            if (eventAggregator == null)
            {
                Console.WriteLine($"Unable to find {nameof(IEventAggregator)} service");

                return;
            }

            var killQuest = factory.BuildQuest<KillQuest>();
            killQuest.TransitionTo(QuestStatus.Active);

            player.AddQuest(killQuest);
            player.PrintQuestGoals();

            eventAggregator.Publish(new KillEvent(player, enemy, 1));

            player.PrintQuestGoals();
        }

        private static IServiceProvider BuildServiceProvider(QuestRegistry registry)
        {
            var logger = new LoggerConfiguration()
                         .Enrich.FromLogContext()
                         .WriteTo.Console()
                         .CreateLogger();

            var services = new ServiceCollection();
            services
                .AddTransient<IQuestFactory, QuestFactory>()
                .AddSingleton<IEventAggregator, EventAggregatorService>()
                .AddSingleton(_ => registry)
                .AddLogging(builder => builder.AddSerilog(logger, true));

            registry.AddQuestsToServiceCollection(services);

            return services.BuildServiceProvider();
        }
    }
}
