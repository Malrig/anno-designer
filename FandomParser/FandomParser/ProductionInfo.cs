﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FandomParser
{
    [DataContract]
    public class ProductionInfo
    {
        public ProductionInfo()
        {

        }

        [DataMember(Order = 0)]
        public EndProduct EndProduct { get; set; }
    }
}
