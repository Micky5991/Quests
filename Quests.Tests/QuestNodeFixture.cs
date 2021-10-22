using System;
using FluentAssertions;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Tests.Entities;
using Micky5991.Quests.Tests.TestBases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Micky5991.Quests.Tests;

[TestClass]
public class QuestNodeFixture : QuestTestBase
{
    private DummyNode node;

    [TestInitialize]
    public void Setup()
    {
        this.node = new DummyNode();
        this.node.Initialize();
    }

    [TestCleanup]
    public void Cleanup()
    {
        try
        {
            this.node.Dispose();
        }
        catch (ObjectDisposedException)
        {
            // ignore.
        }

        this.node = null!;
    }

    [TestMethod]
    public void InitializingNodeSetsVariable()
    {
        var currentNode = new DummyNode();
        currentNode.Initialize();

        currentNode.Initialized.Should().BeTrue();
    }

    [TestMethod]
    [DataRow(QuestStatus.Active, false)]
    [DataRow(QuestStatus.Sleeping, false)]
    [DataRow(QuestStatus.Success, true)]
    [DataRow(QuestStatus.Failure, true)]
    public void QuestStatusMeansFinished(QuestStatus status, bool isFinished)
    {
        this.node.ForceSetState(status);

        this.node.Finished.Should().Be(isFinished);
    }

    [TestMethod]
    [DataRow(QuestStatus.Active)]
    [DataRow(QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Success)]
    [DataRow(QuestStatus.Failure)]
    public void ChangingStatusNotifiesListeners(QuestStatus targetStatus)
    {
        if (targetStatus == QuestStatus.Sleeping)
        {
            this.node.ForceSetState(QuestStatus.Active);
        }

        var notified = false;
        this.node.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(DummyNode.Status))
            {
                notified = true;
            }
        };

        this.node.ForceSetState(targetStatus);

        notified.Should().BeTrue();
    }

    [TestMethod]
    [DataRow("Cool")]
    [DataRow("")]
    public void ChangingTitleNotifiesListeners(string? newTitle)
    {
        this.node.SetTitle("OTHER");

        var notified = false;
        this.node.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(DummyNode.Title))
            {
                notified = true;
            }
        };

        this.node.SetTitle(newTitle);

        notified.Should().BeTrue();
        this.node.Title.Should().Be(newTitle);
    }

    [TestMethod]
    [DataRow(QuestStatus.Active, QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Active, QuestStatus.Success)]
    [DataRow(QuestStatus.Active, QuestStatus.Failure)]
    [DataRow(QuestStatus.Sleeping, QuestStatus.Active)]
    [DataRow(QuestStatus.Sleeping, QuestStatus.Success)]
    [DataRow(QuestStatus.Sleeping, QuestStatus.Failure)]
    public void StatusChangeIsPossible(QuestStatus initialStatus, QuestStatus targetStatus)
    {
        this.node.ForceSetState(initialStatus);
        this.node.ForceCanSetState(targetStatus).Should().BeTrue();
    }

    [TestMethod]
    [DataRow(QuestStatus.Failure, QuestStatus.Success)]
    [DataRow(QuestStatus.Failure, QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Failure, QuestStatus.Active)]
    [DataRow(QuestStatus.Success, QuestStatus.Success)]
    [DataRow(QuestStatus.Success, QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Success, QuestStatus.Active)]
    [DataRow(QuestStatus.Sleeping, QuestStatus.Sleeping)]
    [DataRow(QuestStatus.Active, QuestStatus.Active)]
    [DataRow(QuestStatus.Failure, QuestStatus.Failure)]
    [DataRow(QuestStatus.Success, QuestStatus.Success)]
    public void StatusChangeIsNotPossible(QuestStatus initialStatus, QuestStatus targetStatus)
    {
        this.node.ForceSetState(initialStatus);
        this.node.ForceCanSetState(targetStatus).Should().BeFalse();
    }
}
