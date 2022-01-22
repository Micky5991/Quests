using System;
using FluentAssertions;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Micky5991.Quests.Tests
{
    [TestClass]
    public class QuestMetaFixture
    {
        [TestMethod]
        public void PassingNullAsTypeThrowsException()
        {
            Action action = () =>
            {
                var _ = new QuestMeta(null!);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void PassingNonRootNodeTypeThrowsException()
        {
            Action action = () =>
            {
                var _ = new QuestMeta(typeof(int));
            };

            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TypePropertyWillReturnCorrectValue()
        {
            var typeValue = typeof(DummyQuest);
            var meta = new QuestMeta(typeValue);

            meta.Type.Should().Be(typeValue);
        }
    }
}
