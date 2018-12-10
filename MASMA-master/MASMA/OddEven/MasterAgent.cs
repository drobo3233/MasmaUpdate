using ActressMas;
using MASMA.Common;
using MASMA.Message;
using System;
using System.Collections.Generic;

namespace MASMA.OddEven
{
    public class MasterAgent : Agent
    {
 
        private HighResTimer _counter = new HighResTimer();

        private int countdone_sort_asignData = 0;
        private int countdone_done_print = 0;

        public override void BeforeStop()
        {
            Console.WriteLine(_counter.Stop());
            Utils.Assert(Utils.Destination);
        }

        public override void Setup()
        {
            _counter.Start();
            split();
        }

        public override void Act(ActressMas.Message message)
        {
            var baseMessage = new BaseMessage(message.Content);
            Console.WriteLine("\t[{1} -> {0}]: {2}", this.Name, message.Sender, message.Content);

            string action;
            string parameters;
            Utils.ParseMessage(message.Content, out action, out parameters);

            switch (baseMessage.Action)
            {
                case Actions.DoneAssignAndSort:
                    countdone_sort_asignData++;
                    if (countdone_sort_asignData == Utils.numAg)
                    {
                       
                        int k = Utils.numAg - 1;
                        while (k > 0)
                        {
                            evenPhase();
                            oddPhase();
                            k--;
                               Console.WriteLine(k+" Phase ");
                        }
                        for (int j = 0; j < Utils.agentPool.Count; j++)
                            Send(Utils.agentPool[j], new BaseMessage { Action = Actions.PrintData }.ToString());
                    }


                    break;
                case Actions.DonePrint:
                    countdone_done_print++;
                    if (countdone_done_print == Utils.numAg)
                    {
                        ////asdas
                        ///
                        for (int i = 0; i < Utils.lsort.Count; i++)
                        {
                            Console.Write(Utils.lsort[i] + ",");
                        }
                        Utils.Destination = Utils.lsort.ToArray();
                        Stop();
                    }

                    break;
            }
        }

        public void split()
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

                Send(Utils.agentPool[agentIndex], Utils.Str(new BaseMessage
                {
                    Action = Actions.AssignAndSort
                }.ToString(), fromArray(rData)));

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
        Boolean hasNeig(string agent)
        {
            Boolean has = false;
            int indx = Utils.agentPool.IndexOf(agent);
            try
            {
                Utils.agentPool[indx+1].ToString();
                has = true;
            }
            catch (Exception e)
            {
                has = false;
            }
            return has;

        }

        void evenPhase()
        {
            for (int i = 0; i < Utils.agentPool.Count; i++)
            {
                if (i % 2 == 0)
                    if (hasNeig(Utils.agentPool[i]))
                        Send(Utils.agentPool[i], new BaseMessage
                        {
                            Action = Actions.EvenPhase
                        }.ToString());
            }

        }
        void oddPhase()
        {
            for (int i = 0; i < Utils.agentPool.Count; i++)
            {
                if (i % 2 == 1)
                    if (hasNeig(Utils.agentPool[i]))
                        Send(Utils.agentPool[i], new BaseMessage
                        {
                            Action = Actions.OddPhase
                        }.ToString());
            }

        }
    }
}
