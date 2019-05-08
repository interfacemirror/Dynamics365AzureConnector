﻿using System;
using System. Collections. Generic;
using System. Linq;
using System. Runtime. Serialization;
using System. Text;
using System. Threading. Tasks;

namespace D365AzureKVConnector
{
    [DataContract]
    public class GetSecretResponse
    {
        [DataMember]
        public string value { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public Attributes attributes { get; set; }
    }

    [DataContract]
    public class GetSecretVersionsResponse
    {
        [DataMember]
        public Value [ ] value { get; set; }
        [DataMember]
        public string nextLink { get; set; }
    }

    [DataContract]
    public class Value
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public Attributes attributes { get; set; }
    }

    public class Attributes
    {
        [DataMember]
        public bool enabled { get; set; }
        [DataMember]
        public int created { get; set; }
        [DataMember]
        public int updated { get; set; }
    }
}
