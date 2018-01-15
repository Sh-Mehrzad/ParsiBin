using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses
{
    public class TournamentConfig : EntityTypeConfiguration<Tournament>
    {
        public TournamentConfig()
        {
            this.HasOptional(x => x.Groups);            
        }
    }
}
