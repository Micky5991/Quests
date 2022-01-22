using System;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.EventAggregator.Services;
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
            var provider = BuildServiceProvider(questRegistry);

            Console.WriteLine("ServiceProvider built");

            var game = provider.GetService<GameLogic>();
            if (game == null)
            {
                Console.WriteLine($"Unable to find {nameof(GameLogic)} service");

                return;
            }

            game.Run();
        }

        private static IServiceProvider BuildServiceProvider(QuestRegistry registry)
        {
            var logger = new LoggerConfiguration()
                         .Enrich.FromLogContext()
                         .WriteTo.Console()
                         .CreateLogger();

            var services = new ServiceCollection();
            services
                .AddSingleton<GameLogic>()
                .AddTransient<IQuestFactory, QuestFactory>()
                .AddSingleton<IEventAggregator, EventAggregatorService>()
                .AddSingleton(_ => registry)
                .AddLogging(builder => builder.AddSerilog(logger, true));

            registry.AddQuestsToServiceCollection(services);

            return services.BuildServiceProvider();
        }
    }
}
