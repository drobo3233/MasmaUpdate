using System;
using System.Collections.Generic;
using System.Linq;
using ActressMas;
using System.Timers;
using MASMA.Message;
namespace MASMA.MergeSortParallel
{
    class MasterAgent : Agent
    {

        private Timer _timer;
        int doneAssignData = 0;
        int doneSplit = 0;
        public MasterAgent()
        {
            _timer = new Timer();
        }
        public override void Setup()
        {
            Send(Utils.agentPool[0], Utils.Str("split", fromArray(Utils.Source)));
            _timer.Start();
        }
        public override void Act(ActressMas.Message message)
        {
            try
            {
                Console.WriteLine("\t[{1} -> {0}]: {2}", this.Name, message.Sender, message.Content);

                string action;
                List<string> parameters;
                Utils.ParseMessage(message.Content, out action, out parameters);

                switch (action)
                {
                    case Actions.DonePhase1:
                        doneSplit++;
                        if (doneSplit == Utils.validagentPool.Count)
                        {
                            //remove invalid workers(Nodes)
                            for (int j = 0; j < Utils.agentPool.Count; j++)
                            {
                                if (!Utils.validagentPool.Contains(Utils.agentPool[j]))
                                {
                                    Send(Utils.agentPool[j], new BaseMessage
                                    {
                                        Action = Actions.Terminate2
                                    }.ToString());
                                }
                            }
                            Utils.agentPool = Utils.validagentPool;
                            for (int j = 0; j < Utils.validagentPool.Count; j++)
                            {
                                Send(Utils.validagentPool[j], new BaseMessage
                                {
                                    Action = Actions.SortAndAssignData
                                }.ToString());
                            }
                        }

                        break;
                    case Actions.DoneSortAndAssignData:
                        doneAssignData++;
                        if (doneAssignData == Utils.agentPool.Count)
                        {
                            while (Utils.agentPool.Count > 1)
                            {
                                MergeAndCompare();

                            }
                            Send(Utils.agentPool[0], new BaseMessage
                            {
                                Action = Actions.Print
                            }.ToString());
                            Stop();
                        }
                        break;

                }
            }
            catch (Exception e)
            {

            }

        }

        private void MergeAndCompare()
        {
            if (Utils.agentPool.Count > 1)
            {
                for (int i = 1; i < Utils.agentPool.Count; i += 2)
                {
                    Send(Utils.agentPool[i++], new BaseMessage
                    {
                        Action = Actions.MergeAndCompare
                    }.ToString());
                }
            }
        }

        public void Split()
        {
            int agentIndex = 0;
            int startindex = 0;
            int endindex = 0;
            int blockSize = (int)decimal.Round((decimal)Utils.Source.Length / Utils.numAg);
            while (agentIndex < Utils.numAg)
            {
                startindex = blockSize * agentIndex;

                if (Utils.numAg - 1 == agentIndex || startindex + blockSize > Utils.Source.Length)
                {
                    endindex = Utils.Source.Length;
                }
                else
                {
                    endindex = (agentIndex + 1) * blockSize;
                }
                int[] rData = retreiveData(startindex, endindex);

                if (rData.Length < 1)
                {
                    Send(Utils.agentPool[agentIndex], "terminate");

                }
                else
                    Send(Utils.agentPool[agentIndex], Utils.Str("sort-asignData", fromArray(rData)));
                agentIndex++;
            }


        }
        int[] retreiveData(int limitA, int limitB)
        {

            List<int> list = new List<int>();
            for (int i = limitA; i < limitB; i++)
            {
                list.Add(Utils.Source[i]);
            }

            return list.ToArray();

        }
        string fromArray(int[] arr)
        {
            string ret = "";
            for (int i = 0; i < arr.Length; i++)
            {
                ret = ret + arr[i] + ",";
            }
            return ret.Remove(ret.Length - 1);
        }
    }
}
