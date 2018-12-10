/**************************************************************************
 *                                                                        *
 *  File:        StandardMessage.cs                                       *
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
    public enum StandardMessageParts
    {
        Action = 0,
        Left,
        Right,
        SourceToDestination
    }

    public class StandardMessage
    {
        public string Action { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public bool SourceToDestination { get; set; }

        public StandardMessage()
        {

        }

        public StandardMessage(string content)
        {
            string[] messageParts = content.Split();

            Action = messageParts[(int)StandardMessageParts.Action];
            Left = int.Parse(messageParts[(int)StandardMessageParts.Left]);
            Right = int.Parse(messageParts[(int)StandardMessageParts.Right]);
            SourceToDestination = bool.Parse(messageParts[(int)StandardMessageParts.SourceToDestination]);
        }
        public static string Str(object p1, object p2)
        {
            return string.Format("{0} {1}", p1, p2);
        }
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Action, Left, Right, SourceToDestination);
        }
    }
}
