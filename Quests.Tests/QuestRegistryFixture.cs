using System;
using System.Collections.Generic;
using FluentAssertions;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Services;
using Micky5991.Quests.Tests.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Micky5991.Quests.Tests;

[TestClass]
public class QuestRegistryFixture
{
    private Mock<IServiceCollection> services;

    private TestMetaRegistry registry;

    [TestInitialize]
    public void Initialize()
    {
        this.services = new Mock<IServiceCollection>();

        this.registry = new TestMetaRegistry();
    }

    [TestCleanup]
    public void Cleanup()
    {
        this.services = null;
    }

    [TestMethod]
    public void PassingNullAsServiceCollectionThrowsException()
    {
        Action action = () => this.registry.AddQuestsToServiceCollection(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void AddQuestsToServiceCollectionRegistersAllTypes()
    {
        this.services.Setup(
                            x => x.Add(
                                       It.Is<ServiceDescriptor>(s =>
                                                                    s.ServiceType == typeof(DummyQuest) &&
                                                                    s.ImplementationType == typeof(DummyQuest) &&
                                                                    s.Lifetime == ServiceLifetime.Transient
                                                                    )
                                       ));

        this.registry.AddQuestsToServiceCollection(this.services.Object);

        this.services.Verify(
                            x => x.Add(
                                       It.Is<ServiceDescriptor>(s =>
                                                                    s.ServiceType == typeof(DummyQuest) &&
                                                                    s.ImplementationType == typeof(DummyQuest) &&
                                                                    s.Lifetime == ServiceLifetime.Transient
                                                               )
                                      ), Times.Once);
    }

    private class TestMetaRegistry : QuestRegistry
    {
        protected override IEnumerable<QuestMeta> BuildAvailableQuestMeta()
        {
            yield return this.BuildQuest<DummyQuest>();
        }
    }
}
