using Micky5991.EventAggregator.Elements;

namespace Micky5991.Quests.Events
{
    public class QuestActionEvent<T>
    {
        public object Sender { get; }

        public T EventArgs { get; }

        public QuestActionEvent(object sender, T eventArgs)
        {
            this.Sender = sender;
            this.EventArgs = eventArgs;
        }
    }
}
