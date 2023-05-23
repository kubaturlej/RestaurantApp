using RestaurantApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Domain.Entities
{
    public class Address : EntityBase
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }


        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
