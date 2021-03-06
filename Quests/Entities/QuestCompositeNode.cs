using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Dawn;
using JetBrains.Annotations;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities
{
    /// <inheritdoc cref="IQuestCompositeNode" />
    [PublicAPI]
    public abstract class QuestCompositeNode : QuestChildNode, IQuestCompositeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestCompositeNode"/> class.
        /// </summary>
        /// <param name="rootNode">Root node of this quest tree.</param>
        public QuestCompositeNode(IQuestRootNode rootNode)
            : base(rootNode)
        {
            this.Title = "COMPOSITE NODE";
        }

        /// <inheritdoc />
        public IImmutableList<IQuestChildNode> ChildNodes { get; private set; } = ImmutableList<IQuestChildNode>.Empty;

        /// <summary>
        /// Adds a node to the current quest tree.
        /// </summary>
        /// <param name="childNode">New child that should be added to this tree.</param>
        /// <exception cref="ArgumentNullException"><paramref name="childNode"/> is null.</exception>
        public virtual void Add(IQuestChildNode childNode)
        {
            Guard.Argument(childNode, nameof(childNode)).NotNull();
            Guard.Disposal(childNode.Disposed, nameof(childNode));

            if (childNode.RootNode != this.RootNode)
            {
                throw new ArgumentException($"Could not add child node {childNode} to composite. Wrong root node.");
            }

            if (this.Initialized)
            {
                childNode.Initialize();
            }

            this.ChildNodes = this.ChildNodes.Add(childNode);
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            base.Initialize();

            foreach (var childNode in this.ChildNodes)
            {
                childNode.Initialize();
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            var childs = this.ChildNodes;
            this.ChildNodes = ImmutableList<IQuestChildNode>.Empty;

            foreach (var childNode in childs)
            {
                childNode.Dispose();
            }

            base.Dispose();
        }

        /// <inheritdoc />
        public IEnumerator<IQuestChildNode> GetEnumerator()
        {
            return this.ChildNodes.GetEnumerator();
        }

        /// <inheritdoc/>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
