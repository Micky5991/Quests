using System;
using Micky5991.Quests.Tests.Entities;
using Micky5991.Quests.Tests.TestBases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;

namespace Micky5991.Quests.Tests;

[TestClass]
public class QuestAnySuccessFixture : QuestTestBase
{
    private DummyTask[] tasks;

    private DummyAnySuccessNode composite;

    private DummyQuest quest;

    private IServiceProvider serviceProvider;

    [TestInitialize]
    public void Setup()
    {
        this.serviceProvider = new ServiceCollection()
                               .AddLogging(builder => builder.AddSerilog(Logger.None))
                               .BuildServiceProvider();

        this.SetupQuests(true);
    }

    [TestCleanup]
    public void Cleanup()
    {
        try
        {
            this.quest.Dispose();
        }
        catch (ObjectDisposedException)
        {
            // ignore.
        }

        this.quest = null!;
        this.composite = null!;
        this.tasks = null!;

        this.serviceProvider = null;
    }

    private void SetupQuests(bool initialize)
    {
        this.quest = this.CreateExampleQuest(q =>
        {
            this.tasks = new[]
            {
                new DummyTask(q),
                new DummyTask(q),
                new DummyTask(q),
                new DummyTask(q),
                new DummyTask(q),
            };

            this.composite = new DummyAnySuccessNode(q);
            foreach (var task in this.tasks)
            {
                this.composite.Add(task);
            }

            return this.composite;
        }, initialize);
    }


}
