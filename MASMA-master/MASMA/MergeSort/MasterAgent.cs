/**************************************************************************
 *                                                                        *
 *  File:        MasterAgent.cs                                           *
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

using ActressMas;
using MASMA.Common;
using MASMA.Message;
using System;
using System.Collections.Generic;

namespace MASMA.MergeSort
{
    public class MasterAgent : Agent
    {
        private Stack<StandardMessage> _calls = new Stack<StandardMessage>();
        private HighResTimer _counter = new HighResTimer();
        private int _expectedMerge = 0;

        public override void Setup()
        {
            _counter.Start();

            StandardMessage standardMessage = new StandardMessage
            {
                Action = Actions.Sort,
                Left = 0,
                Right = Utils.Length - 1,
                SourceToDestination = true
            };

            _calls.Push(standardMessage);

            SendMessage(standardMessage, Agents.WorkerAgentLeft, true);
            SendMessage(standardMessage, Agents.WorkerAgentRight, false);
        }

        private void SendMessage(StandardMessage standardMessage, string agentName, bool isLeft)
        {
            int middle = (standardMessage.Left + standardMessage.Right) / 2;
            StandardMessage currentMessage = new StandardMessage
            {
                Action = Actions.Sort,
                Left = isLeft ? standardMessage.Left : middle + 1,
                Right = isLeft ? middle : standardMessage.Right,
                SourceToDestination = !standardMessage.SourceToDestination
            };

            if ( 
                    isLeft ?
                        (middle - standardMessage.Left) > Utils.Threshold :
                        (standardMessage.Right - middle - 1) > Utils.Threshold
                )
            {
                _calls.Push(currentMessage);
            }
            else
            {
                _expectedMerge++;
            }

            Send(agentName, currentMessage.ToString());
        }

        public override void Act(ActressMas.Message message)
        {
            var standardMessage = new StandardMessage(message.Content);

            switch (standardMessage.Action)
            {
                case Actions.Split:
                    SendMessage(standardMessage, message.Sender , true);
                    SendMessage(standardMessage, message.Sender , false);
                    break;

                case Actions.Merge:
                    _expectedMerge--;

                    if (_expectedMerge == 0)
                    {
                        while (_calls.Count > 0)
                        {
                            var currentCall = _calls.Pop();
                            int middle = (currentCall.Left + currentCall.Right) / 2;
                            if (currentCall.SourceToDestination)
                            {
                                ParallelAlgorithm.MergeInnerPar(Utils.Source, currentCall.Left,
                                    middle, middle + 1, currentCall.Right, Utils.Destination, currentCall.Left);
                            }
                            else
                            {
                                ParallelAlgorithm.MergeInnerPar(Utils.Destination, currentCall.Left,
                                    middle, middle + 1, currentCall.Right, Utils.Source, currentCall.Left);
                            }
                        }

                        Environment.StopAll();
                    }

                    break;
            }
        }

        public override void BeforeStop()
        {
            Console.WriteLine(_counter.Stop());
            Utils.Assert(Utils.Destination);
        }
    }
}
