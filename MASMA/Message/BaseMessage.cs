﻿/**************************************************************************
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

    public class BaseMessage<T>
    {
        private Serializer<T> _serializer = new Serializer<T>();

        public Message<T> Message { get; set; }

        public BaseMessage()
        {

        }

        public BaseMessage(string json)
        {
            Message = _serializer.Deserialize(json);
        }

        public override string ToString()
        {
            return _serializer.Serialize(Message);
        }
    }
}
