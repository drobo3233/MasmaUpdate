/**************************************************************************
 *                                                                        *
 *  File:        WorkerAgent.cs                                           *
 *  Description: Merge Sort multi-agent                                   *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

using System;
using ActressMas;
using MASMA.Message;

namespace MASMA.MergeSort
{
    public class WorkerAgent : Agent
    {
        public override void Act(ActressMas.Message message)
        {
            var standardMessage = new StandardMessage(message.Content);

            switch (standardMessage.Action)
            {
                case Actions.Sort:

                    if (standardMessage.Right == standardMessage.Left)
                    {
                        if (standardMessage.SourceToDestination)
                        {
                            Utils.Destination[standardMessage.Left] = Utils.Source[standardMessage.Left];
                        }

                        SendMergeMessage();
                    }
                    else if ((standardMessage.Right - standardMessage.Left) <= Utils.Threshold)
                    {
                        Array.Sort(Utils.Source, standardMessage.Left,
                            standardMessage.Right - standardMessage.Left + 1);

                        if (standardMessage.SourceToDestination)
                            for (int i = standardMessage.Left; i <= standardMessage.Right; i++)
                                Utils.Destination[i] = Utils.Source[i];

                        SendMergeMessage();
                    }
                    else
                    {
                        standardMessage.Action = Actions.Split;
                        Send(Agents.MasterAgent, standardMessage.ToString());
                    }

                    break;
            }
        }

        private void SendMergeMessage()
        {
            Send(Agents.MasterAgent, new StandardMessage
            {
                Action = Actions.Merge
            }.ToString());
        }
    }
}
