/**************************************************************************
 *                                                                        *
 *  File:        Serializer.cs                                            *
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

using Newtonsoft.Json;

namespace MASMA.Message
{
    public interface ISerializer<T>
    {
        string Serialize(Message<T> data);
        Message<T> Deserialize(string json);
    }

    public class Serializer<T> : ISerializer<T>
    {
        public Message<T> Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<Message<T>>(json);
        }

        public string Serialize(Message<T> data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
