﻿using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    public partial class LocationType
    {
        public LocationType()
        {
            Location = new HashSet<Location>();
        }

        public int LocationTypeId { get; set; }
        public string LocationTypeName { get; set; }

        public virtual ICollection<Location> Location { get; set; }
    }
}