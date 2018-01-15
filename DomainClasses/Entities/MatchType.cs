using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class MatchType
    {
        public int ID { get; set; }

        [MaxLength(70)]
        public string Title { get; set; }
        public virtual ICollection<Match> Match { get; set; }
    }
}
