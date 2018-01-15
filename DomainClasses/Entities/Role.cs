using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class Role
    {
        [Key]
        public virtual int roleId { get; set; }

        [MaxLength(20)]
        public virtual string RoleName { get; set; }
    }
}
