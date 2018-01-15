using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParsiBin.DomainClasses;
using ParsiBin.DomainClasses.Entities;

namespace ParsiBin.DomainClasses
{
    public class ParticipantConfig : EntityTypeConfiguration<Participant>
    {
        public ParticipantConfig()
        {
            //this.HasMany(x => x.Match)
            //    .WithMany(x => x.Participant)                
            //    .Map(Map =>
            //    {
            //        Map.MapLeftKey("Particapnt_ID");
            //        Map.MapRightKey("MatchID");
                    
            //        Map.ToTable("ParticipantInMatch");
            //    });
            //this.HasKey()
               
        }
    }
}
