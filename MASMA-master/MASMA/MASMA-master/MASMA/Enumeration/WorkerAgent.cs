using ActressMas;
using MASMA.Message;

namespace MASMA.Enumeration
{
    public class WorkerAgent: Agent
    {
        private int _agentIndex;
        private int _startIndex;
        private int _endIndex;

        public WorkerAgent(int index)
        {
            _agentIndex = index;
        }

        public override void Act(ActressMas.Message message)
        {
            var baseMessage = new BaseMessage(message.Content);

            switch (baseMessage.Action)
            {
                case Actions.Sort:

                    if (Utils.SkipNext)
                    {
                        Send(Agents.MasterAgent, new BaseMessage
                        {
                            Action = Actions.Done
                        }.ToString());

                        Stop();
                    }

                    int blockSize = (int)decimal.Round((decimal)Utils.Length / Utils.NoAgents);
                    _startIndex = blockSize * _agentIndex;

                    if (Utils.NoAgents - 1 == _agentIndex || _startIndex + blockSize > Utils.Length)
                    {
                        _endIndex = Utils.Length;
                    }
                    else
                    {
                        _endIndex = (_agentIndex + 1) * blockSize;
                    }

                    if (_endIndex == Utils.Length)
                    {
                        Utils.SkipNext = true;
                    }

                    for (int i = _startIndex; i < _endIndex; i++)
                    {
                        int value = Utils.Source[i];
                        int rank = 0;

                        for (int j = 0; j < Utils.Length; j++)
                        {
                            if(value > Utils.Source[j])
                            {
                                rank++;
                            }

                            if (value == Utils.Source[j] && (i < j))
                            {
                                rank++;
                            }
                        }

                        Utils.Destination[rank] = value;
                    }

                    Send(Agents.MasterAgent, new BaseMessage
                    {
                        Action = Actions.Done
                    }.ToString());

                    Stop();

                    break;
            }
        }
    }
}
