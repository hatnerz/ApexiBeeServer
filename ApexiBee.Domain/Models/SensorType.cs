﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Domain.Models
{
    public class SensorType : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MeasureUnit { get; set; }
    }
}
