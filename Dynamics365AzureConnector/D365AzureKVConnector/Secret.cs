﻿using System;
using System. Collections. Generic;
using System. Linq;
using System. Text;
using System. Threading. Tasks;
using System. Runtime. Serialization;


namespace D365AzureKVConnector
{
    [DataContract]
    public class Secret
    {
        [DataMember ( Name = "value" )]
        public string Value { get; set; }
        [DataMember ( Name = "contentType" )]
        public string ContentType { get; set; } = "text/plain";
        [DataMember ( Name = "id" )]
        public string Id { get; set; }
        [DataMember ( Name = "attributes" )]
        public SecretAttributes SecretAttributes { get; set; }
    }

    [DataContract ( Name = "attributes" )]
    public class SecretAttributes
    {
        [DataMember ( Name = "enabled" )]
        public bool Enabled { get; set; }
        [DataMember ( Name = "nbf" )]
        public int Nbf { get; set; }
        [DataMember ( Name = "exp" )]
        public int Exp { get; set; }
        [DataMember ( Name = "created" )]
        public int Created { get; set; }
        [DataMember ( Name = "updated" )]
        public int Updated { get; set; }
        [DataMember ( Name = "recoveryLevel" )]
        public string RecoverLevel { get; set; }
    }
}
