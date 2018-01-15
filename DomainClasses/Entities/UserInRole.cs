using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class UserInRole
    {        
        public virtual int UserInRoleID { get; set; }
        public virtual int userId { get; set; }
        public virtual int roleId { get; set; }
    }
}
