using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ParsiBin.DomainClasses.Entities;

namespace ParsiBin.DomainClasses
{
    public class Group : General.GeneralClass<Guid>
    {        
        [MaxLength(50)]
        public string Title { get; set; }
        public int? ParentID { get; set; }
        public virtual Group Parent { get; set; }
        public virtual ICollection<Group> GroupsList { get; set; }
        public virtual Tournament tournament { get; set; }
        public int TournamentID { get; set; }
        public virtual ICollection<Participant> ParticipantsList { get; set; }
        public int? SportTypeID { get; set; }
        public virtual SportType SportType { get; set; }
        public virtual ICollection<Match> MatchList { get; set; }
        public int? ParticipantTypeID { get; set; }
        public int? CountryID { get; set; }
    }
}
