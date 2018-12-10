using ActressMas;
using MASMA.Message;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MASMA.OddEven
{
    public class WorkerAgent : Agent
    {

        private int[] data;
        public WorkerAgent(int id)
        {
            data = new int[Utils.Length];
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
                case Actions.AssignAndSort:
                    asignData(parameters);
                    //Sort();
                    Sort2();
                    Send(Agents.MasterAgent, new BaseMessage
                    {
                        Action = Actions.DoneAssignAndSort
                    }.ToString());
                    break;
                case Actions.EvenPhase:

                    if (hasNeig(this.Name))
                    {
                        int indx = Utils.agentPool.IndexOf(this.Name);
                        Send(Utils.agentPool[indx + 1], Utils.Str(new BaseMessage
                        {
                            Action = Actions.MergeAndSplit
                        }.ToString(), fromArray(this.data)));
                    }

                    break;
                case Actions.OddPhase:

                    if (hasNeig(this.Name))
                    {
                        int indx = Utils.agentPool.IndexOf(this.Name);
                        Send(Utils.agentPool[indx + 1], Utils.Str(new BaseMessage
                        {
                            Action = Actions.MergeAndSplit
                        }.ToString(), fromArray(this.data)));
                    }
                    break;
                case Actions.MergeAndSplit:

                    Console.WriteLine("Receive: " + parameters + " Own: " + fromArray(this.data));

                    int[] received = asignData(parameters, 0);
                    int[] own = this.data;
                    int[] result = received
                         .Concat(own)
                         .OrderBy(x => x)
                         .ToArray();
                    int[] sendBck = result.Take((result.Length + 1) / 2).ToArray();
                    this.data = result.Skip((result.Length + 1) / 2).ToArray();
                    Send(message.Sender, Utils.Str(new BaseMessage
                    {
                        Action = Actions.RefreshData
                    }.ToString(), fromArray(sendBck)));

                    break;
                case Actions.RefreshData:
                    Console.Write("  Agent: " + this.Name + " data:  " );
                    for (int i = 0; i < data.Length; i++)
                        Console.Write(data[i] + ". ");
                    Console.WriteLine();
                    Console.WriteLine("  will update data with : " + parameters);
                    this.data = asignData(parameters, 0);
                    break;
                case Actions.PrintData:
                    for (int i = 0; i < data.Length; i++)
                    {
                        Utils.lsort.Add(data[i]);
                    }

                    Utils.destination.Add(this.Name, this.data);
                    Send(Agents.MasterAgent, new BaseMessage
                    {
                        Action = Actions.DonePrint
                    }.ToString());
                    Stop();
                    break;
            }
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
        public void asignData(string data)
        {

            string[] spl = data.Split(',');
            this.data = new int[spl.Length];
            for (int i = 0; i < spl.Length; i++)
            {
                this.data[i] = Int32.Parse(spl[i]);

            }
        }
        public int[] asignData(string data, int ir)
        {

            string[] spl = data.Split(',');
            int[] ret = new int[spl.Length];
            for (int i = 0; i < spl.Length; i++)
            {
                ret[i] = Int32.Parse(spl[i]);

            }
            return ret;
        }
        public void Sort()
        {
            Array.Sort(data);
        }
        public void Sort2()
        {
            Boolean IsSorted = false;
            int phase = 0;
            if (!IsSorted)
            {


                // event phase
                if (phase % 2 == 0)
                {
                    IsSorted = true;
                    for (int i = 0; i <= this.data.Length - 2; i += 2)
                    {

                        if (this.data[i] > this.data[i + 1])
                        {
                            Utils.Swap(ref this.data[i], ref this.data[i + 1]);
                            IsSorted = false;
                        }

                    }
                }
                else // odd phase
                {
                    IsSorted = true;
                    for (int i = 1; i <= this.data.Length - 2; i += 2)
                    {

                        if (this.data[i] > this.data[i + 1])
                        {
                            Utils.Swap(ref this.data[i], ref this.data[i + 1]);
                            IsSorted = false;
                        }

                    }
                }
                phase++;
            }
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
