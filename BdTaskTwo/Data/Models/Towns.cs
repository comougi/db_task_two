﻿using System;
using System.Collections.Generic;

namespace BdTaskTwo.Data.Models
{
    public partial class Towns
    {
        public Towns()
        {
            Addresses = new HashSet<Addresses>();
        }

        public int TownId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Addresses> Addresses { get; set; }
    }
}
