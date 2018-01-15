using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses
{
    public class GroupConfig : EntityTypeConfiguration<Group>
    {
        public GroupConfig()
        {
            this.HasOptional(x => x.Parent)                
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}
