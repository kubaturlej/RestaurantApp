using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Domain.Entities
{
    public class UserRole
    {
        public User User { get; set; }
        public string UserId { get; set; }

        public Role Role{ get; set; }
        public string RoleId { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
