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
    }
}
