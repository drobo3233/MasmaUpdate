using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ActressMas;
using MASMA.Message;
namespace MASMA.MergeSortParallel
{

    class Worker : Agent
    {
        int[] data;
        int doneAssign = 0;
        public Worker()
        {
            data = new int[1000];
        }
        public int CreateWorker(int index)
        {
            var bidderAgent = new Worker();

            Utils.env.Add(bidderAgent, string.Format("Slave{0:D2}", index));
            Utils.agentPool.Add(bidderAgent.Name);
            bidderAgent.Start();
            return index;
        }
        public override void Act(ActressMas.Message message)
        {
            try
            {
                Console.WriteLine("\t[{1} -> {0}]: {2}", this.Name, message.Sender, message.Content);

                string action;
                string parameters;
                Utils.ParseMessage(message.Content, out action, out parameters);

                switch (action)
                {
                    case "done-asignData":
                        doneAssign++;
                        // first split [L,R]
                        if (doneAssign == 2)
                        {
                            while (!Utils.finalSplit)
                            {
                                for (int i = 1; i < Utils.agentPool.Count; i += 1)
                                {
                                    Send(Utils.agentPool[i], "split-own");
                                }
                            }
                        }

                        break;

                    case "split-own":
                        int[] sp1_own = data.Take((data.Length + 1) / 2).ToArray();
                        int[] sp2_own = data.Skip((data.Length + 1) / 2).ToArray();
                        if (sp1_own.Length >= Utils.Threshold && sp2_own.Length >= Utils.Threshold)
                        {
                            int a = CreateWorker(Utils.agentPool.Count);
                            int b = CreateWorker(Utils.agentPool.Count);
                            Thread.Sleep(200);
                            Send(Utils.agentPool[a], Utils.Str("asign", fromArrayToString(sp1_own)));
                            Send(Utils.agentPool[b], Utils.Str("asign", fromArrayToString(sp2_own)));
                        }
                        else
                        {
                            if (this.data.Length >= Utils.Threshold)
                                Utils.validagentPool.Add(this.Name);
                            Utils.finalSplit = true;
                            Send(Agents.MasterAgent, Utils.Str("done-phase1", this.Name));

                        }
                        break;
                    //main split from [Worker00]
                    case "split":

                        int[] toSplit = fromStringToArray(parameters);
                        int[] sp1 = toSplit.Take((toSplit.Length + 1) / 2).ToArray();
                        int[] sp2 = toSplit.Skip((toSplit.Length + 1) / 2).ToArray();
                        if (sp1.Length >= 2 && sp2.Length >= 2)
                        {
                            CreateWorker(Utils.agentPool.Count);
                            CreateWorker(Utils.agentPool.Count);
                            Thread.Sleep(200);
                            Send(Utils.agentPool[1], Utils.Str("asignData", fromArrayToString(sp1)));
                            Send(Utils.agentPool[2], Utils.Str("asignData", fromArrayToString(sp2)));
                        }
                        break;
                    case "asign":
                        Assign(parameters);
                        break;
                    case "asignData":
                        Assign(parameters);
                        Send(Utils.agentPool[0], "done-asignData");

                        break;

                    case "sort-asignData":
                        //SortAndAssign(parameters);
                        Sort();
                        Send(Agents.MasterAgent, "done-sort-asignData");
                        break;
                    //i+1
                    case "merge-and-compare":
                        //send i -> data  ->merge
                        int index = Utils.agentPool.IndexOf(this.Name);
                        if (index != 0 && index != Utils.numAg)
                            Send(Utils.agentPool[index - 1], Utils.Str("merge", fromArrayToString(this.data)));
                        Utils.agentPool.Remove(this.Name);
                        Stop();
                        //stop
                        break;
                    case "merge":
                        int[] received = fromStringToArray(parameters);
                        int[] own = this.data;
                        int[] result = received
                             .Concat(own)
                             .OrderBy(x => x)
                             .ToArray();
                        this.data = result;

                        break;
                    case "print":

                        Console.WriteLine();
                        for (int i = 0; i < data.Length; i++)
                        {
                            Utils.Destination[i] = data[i];
                            Console.Write(data[i] + "_  ");
                        }
                        Stop();
                        break;
                    case "terminate":
                        Utils.agentPool.Remove(this.Name);
                        Stop();
                        break;
                    //stop - without removing from agentPool
                    case "terminate2":
                        Stop();
                        break;
                }
            }
            catch (Exception e)
            {
                Stop();
            }

        }
        string fromArrayToString(int[] arr)
        {
            string ret = "";
            for (int i = 0; i < arr.Length; i++)
            {
                ret = ret + arr[i] + ",";
            }
            return ret.Remove(ret.Length - 1);
        }
        void SortAndAssign(String data)
        {
            string[] spl = data.Split(',');
            int[] ret = new int[spl.Length];
            for (int i = 0; i < spl.Length; i++)
            {
                ret[i] = Int32.Parse(spl[i]);

            }
            this.data = ret;
            Sort();
        }
        void Assign(String data)
        {
            string[] spl = data.Split(',');
            int[] ret = new int[spl.Length];
            for (int i = 0; i < spl.Length; i++)
            {
                ret[i] = Int32.Parse(spl[i]);

            }
            this.data = ret;

        }
        int[] fromStringToArray(String data)
        {
            string[] spl = data.Split(',');
            int[] ret = new int[spl.Length];
            for (int i = 0; i < spl.Length; i++)
            {
                ret[i] = Int32.Parse(spl[i]);

            }
            return ret;

        }
        void Sort()
        {
            Array.Sort(this.data);
        }
    }
}

