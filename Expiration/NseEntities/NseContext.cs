﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NseEntities
{
    public partial class NseContext
    {
        public NseContext(string dbConnectionString):base(string.Format("name={0}",dbConnectionString))
        {
            
        }
    }
}