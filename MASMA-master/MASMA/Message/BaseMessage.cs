/**************************************************************************
 *                                                                        *
 *  File:        BaseMessage.cs                                           *
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

namespace MASMA.Message
{
    public class BaseMessage
    {
        public string Action { get; set; }

        public BaseMessage()
        {

        }

        public BaseMessage(string content)
        {
            string[] messageParts = content.Split();

            Action = messageParts[0];
        }

        public override string ToString()
        {
            return string.Format("{0}", Action);
        }
        public static void ParseMessage(string content, out string action, out string parameters)
        {
            string[] t = content.Split();

            action = t[0];

            parameters = "";

            if (t.Length > 1)
            {
                for (int i = 1; i < t.Length - 1; i++)
                    parameters += t[i] + " ";
                parameters += t[t.Length - 1];
            }
        }
    }
}
