using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Taro.Serialization
{
    public class DefaultJsonSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public object Deserialize(string serializedObject, Type type)
        {
            if (String.IsNullOrEmpty(serializedObject)) return null;

            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(type);

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(serializedObject);
                writer.Flush();

                stream.Seek(0, SeekOrigin.Begin);

                return serializer.ReadObject(stream);
            }
        }
    }
}
