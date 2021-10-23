using System;
using FluentAssertions;
using Micky5991.Quests.Interfaces.Nodes;
using Micky5991.Quests.Services;
using Micky5991.Quests.Tests.TestBases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Serilog;
using Serilog.Core;

namespace Micky5991.Quests.Tests;

[TestClass]
public class QuestFactoryFixture : QuestTestBase
{
    private QuestFactory factory;

    private Mock<IQuestRootNode> quest;

    private IServiceProvider serviceProvider;

    [TestInitialize]
    public void Setup()
    {
        this.quest = new Mock<IQuestRootNode>();

        this.serviceProvider = new ServiceCollection()
                               .AddTransient(x => this.quest.Object)
                               .AddLogging(builder => builder.AddSerilog(Logger.None))
                               .BuildServiceProvider();

        this.factory = new QuestFactory(this.serviceProvider);
    }

    [TestCleanup]
    public void Cleanup()
    {
        this.quest = null;

        this.serviceProvider = null;
    }

    [TestMethod]
    public void QuestFactoryInitializesQuest()
    {
        this.quest.Setup(x => x.Initialize());

        var createdQuest = this.factory.BuildQuest<IQuestRootNode>();

        createdQuest.Should().BeSameAs(this.quest.Object);
        this.quest.Verify(x => x.Initialize(), Times.Once);
    }
}
