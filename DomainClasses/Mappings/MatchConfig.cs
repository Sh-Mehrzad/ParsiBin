using ParsiBin.DomainClasses.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Mappings
{
    public class MatchConfig : EntityTypeConfiguration<Match>
    {
        public MatchConfig()
        {
            //this.HasOptional(x => x.Referee)
            //    .WithRequired(x => x.Match);
        }
    }
}
