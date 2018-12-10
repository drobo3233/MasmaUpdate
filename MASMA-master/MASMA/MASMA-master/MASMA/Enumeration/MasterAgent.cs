using ActressMas;
using MASMA.Common;
using MASMA.Message;
using System;

namespace MASMA.Enumeration
{
    public class MasterAgent: Agent
    {
        private int _done = 0;
        private HighResTimer _counter = new HighResTimer();

        public override void Setup()
        {
            _counter.Start();

            BroadcastAll(new BaseMessage
            {
                Action = Actions.Sort
            }.ToString());
        }

        public override void BeforeStop()
        {
            Console.WriteLine(_counter.Stop());
            Utils.Assert(Utils.Destination);
        }

        public override void Act(ActressMas.Message message)
        {
            var baseMessage = new BaseMessage(message.Content);

            switch (baseMessage.Action)
            {
                case Actions.Done:
                    _done++;
                    if (_done == Utils.NoAgents)
                    {
                        Stop();
                    }
                    break;

            }
        }
    }
}
