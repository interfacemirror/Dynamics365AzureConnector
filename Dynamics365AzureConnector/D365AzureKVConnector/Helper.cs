using System;
using System. Collections. Generic;
using System. IO;
using System. Linq;
using System. Runtime. Serialization. Json;
using System. Text;
using System. Threading. Tasks;

namespace D365AzureKVConnector
{
    public class Helper
    {
        //Generic JSON to object deserialization 
        public static T DeserializeObject<T> ( string json )
        {
            using ( MemoryStream stream = new MemoryStream ( ) )
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                StreamWriter writer = new StreamWriter(stream);
                writer. Write ( json );
                writer. Flush ( );
                stream. Position = 0;
                T o = (T)serializer.ReadObject(stream);

                return o;
            }
        }

        //Generic object JSON serialization 
        public static string SerializeObject<T> ( object o )
        {
            using ( MemoryStream stream = new MemoryStream ( ) )
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                serializer. WriteObject ( stream , o );
                stream. Position = 0;
                StreamReader reader = new StreamReader(stream);
                string json = reader.ReadToEnd();

                return json;
            }
        }

    }
}
